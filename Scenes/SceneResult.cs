using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class SceneResult : Scene
    {
        int currentScore = 0;
        int bestScore = 0;
        int[] currentTime = new int[] { 0, 0, 0 };
        int[] bestTime = new int[] { 0, 0, 0 };

        private string[] ranks = new string[] { "a", "b", "c" };
        private int[] rankScore = new int[] { 1100, 700, 400 };//スコア評価用,数値は仮
        private int[] rankTime = new int[] { 60, 120, 180 };//タイム評価用
        private int[] rankBonus = new int[] { 1000, 500, 200 };//タイムボーナス用

        string timeRank = "d";
        string scoreRank = "d";

        private bool wait = true;//操作ミス防止用+2重フェード対策
        private bool blinkMessage = true;//点滅表示用
        private int Counter = 0;//wait,fade,blinkのカウンター
        private int fadeTime = 180;

        //描画系
        private int frameX = 240;
        private int frameY = 100;
        private const int fontInterval = 30;//文字同士の幅
        private const float fontScale1 = 0.18f;//文字の大きさ
        private const float fontScale2 = 0.1f;//タイムボーナス用
        private const float rankImageScale = 0.4f;
        private int cursorWidth = 220;
        private int cursorInterval = 60;
        private int cursorPos;
        private int[] fixedPos;

        private int bg = ResourceLoader.GetGraph("play_bg.png");
        private int result_bg = ResourceLoader.GetGraph("image_result/result_bg.png");
        private int back = ResourceLoader.GetGraph("image_result/r_back.png");
        private int restart = ResourceLoader.GetGraph("image_result/restart.png");
        private int cursor = ResourceLoader.GetGraph("image_result/cursor.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");
        private int newImage = ResourceLoader.GetGraph("image_result/new.png");

        public SceneResult(Game game) : base(game)
        {
        }

        public override void OnLoad()
        {
            //検証用
            //Game.currentScore = 300;
            //Game.bestScore = 700;
            //Game.currentTime = new int[] { 1, 23, 0 };
            //Game.bestTime = new int[] { 12, 34, 0 };

            wait = true;
            blinkMessage = true;
            Counter = 0;

            fixedPos = new int[] { cursorInterval, Screen.Width - (cursorWidth + cursorInterval) };
            cursorPos = fixedPos[1];

            currentScore = Game.currentScore;
            currentTime = Game.currentTime;
            bestScore = Game.bestScore;
            bestTime = Game.bestTime;
            
            for (int i = 0; i < rankTime.Length; i++)
            {
                if (rankTime[i] >= currentTime[0] * 60 + currentTime[1])
                {
                    currentScore += rankBonus[i];
                    timeRank = ranks[i];
                    break;
                }
            }
            for (int i = 0; i < rankScore.Length; i++)
            {
                if (rankScore[i] <= currentScore)
                {
                    scoreRank = ranks[i];
                    break;
                }
            }

            if (currentScore > bestScore)
                Game.bestScore = currentScore;
            if (currentTime[0] * 60 + currentTime[1] < bestTime[0] * 60 + bestTime[1])
                Game.bestTime = currentTime;
        }

        public override void Update()
        {
            Counter++;
            if (Counter == fadeTime)
            {
                wait = false;
            }

            if (Counter < fadeTime + 10)
            {
                Game.bgmManager.FadeIn("result", 120);
            }

            if (Counter % 60 == 0)
            {
                blinkMessage = true;
            }
            else if (Counter % 60 == 40)
            {
                blinkMessage = false;
            }

            if (cursorPos != fixedPos[0] && Input.LEFT.IsPush())//カーソルが一番左以外の時に←が押されたら、カーソルを一つ左へ
            {
                Sound.Play("cursor_SE.mp3");
                for (int i = 0; i < fixedPos.Length; i++)
                {
                    if (cursorPos == fixedPos[i])
                    {
                        cursorPos = fixedPos[i - 1];
                        break;
                    }
                }
            }
            else if (cursorPos != fixedPos[fixedPos.Length - 1] && Input.RIGHT.IsPush())//カーソルが一番右以外の時→を押されたら、カーソルを一つ右へ
            {
                Sound.Play("cursor_SE.mp3");
                for (int i = 0; i < fixedPos.Length; i++)
                {
                    if (cursorPos == fixedPos[i])
                    {
                        cursorPos = fixedPos[i + 1];
                        break;
                    }
                }
            }
            
            if (!wait && cursorPos == fixedPos[0] && Input.ACTION.IsPush())
            {
                wait = true;
                Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
            }
            else if (!wait && cursorPos == fixedPos[1] && Input.ACTION.IsPush())
            {
                wait = true;
                Game.SetScene(new Title(Game), new Fade(fadeTime, true, true));
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, bg);
            DX.DrawGraph(0, 0, result_bg);
            DX.DrawGraph(cursorPos, Screen.Height - 250, cursor);
            DX.DrawGraph(fixedPos[0], Screen.Height - 250, restart);
            DX.DrawGraph(fixedPos[1], Screen.Height - 250, back);
            //スコア
            int digit = 1000;
            int leftCounter1 = 0;
            int leftCounter2 = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == currentScore / digit % 10 && currentScore / digit != 0)//現在のスコア
                    {
                        DX.DrawRotaGraph(frameX + fontInterval * leftCounter1, frameY, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter1++;
                    }
                    if (j == bestScore / digit % 10 && bestScore / digit != 0)//ハイスコア
                    {
                        DX.DrawRotaGraph(frameX + fontInterval * leftCounter2, frameY + 50, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter2++;
                    }
                    if (j >= currentScore / digit % 10 && j >= bestScore / digit % 10)
                        break;
                }
                digit /= 10;
            }
            //タイムボーナス+スコア評価
            leftCounter2 = 0;
            for (int i = 0; i < ranks.Length; i++)
            {
                if (timeRank == ranks[i])
                {
                    DX.DrawRotaGraph(70, 190, rankImageScale, 0, ResourceLoader.GetGraph("image_result/rank_" + ranks[i] + ".png"));

                    //「タイムボーナス」

                    digit = 1000;
                    for (int j = 0; j < 4; j++)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            if (k == rankBonus[i] / digit % 10 && rankBonus[i] / digit != 0)
                            {
                                DX.DrawRotaGraph(frameX + fontInterval / 2 + fontInterval * leftCounter1 + fontInterval / 2 * leftCounter2,
                                                 frameY+5, fontScale2, 0, ResourceLoader.GetGraph("image_result/result_num_" + k + ".png"));
                                leftCounter2++;
                                break;
                            }
                        }
                        digit /= 10;
                    }
                    DX.DrawRotaGraph(frameX + fontInterval * leftCounter1 - fontInterval / 3, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets1.png"));
                    DX.DrawRotaGraph(frameX + fontInterval * leftCounter1, frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/plus.png"));
                    DX.DrawRotaGraph(frameX + fontInterval * leftCounter1 + fontInterval / 2 * (leftCounter2 + 1), frameY + 5, fontScale2, 0, ResourceLoader.GetGraph("image_result/brackets2.png"));
                    break;
                }
            }
            if (blinkMessage && currentScore > bestScore)//スコアの「new」
            {
                DX.DrawRotaGraph(frameX + 65 + fontInterval * (leftCounter1 + 1) + fontInterval / 2 * leftCounter2, frameY, 1, 0, newImage);
            }
            //タイム
            digit = 10;
            leftCounter1 = 0;
            leftCounter2 = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == currentTime[0] / digit % 10 && (currentTime[0] / digit != 0 || digit == 1))//現在のタイム,分
                    {
                        DX.DrawRotaGraph(frameX + fontInterval * leftCounter1, 200, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter1++;
                    }
                    if (j == bestTime[0] / digit % 10 && (bestTime[0] / digit != 0 || digit == 1))//ベストタイム,分
                    {
                        DX.DrawRotaGraph(frameX + fontInterval * leftCounter2, 246, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter2++;
                    }
                    if (j >= currentTime[0] / digit % 10 && j >= bestTime[0] / digit % 10)
                        break;
                }
                for (int j = 0; j < 10; j++)
                {
                    if (j == currentTime[1] / digit % 10)//現在のタイム,秒
                        DX.DrawRotaGraph(frameX + fontInterval * (2 + leftCounter1), 200, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestTime[1] / digit % 10)//ベストタイム,秒
                        DX.DrawRotaGraph(frameX + fontInterval * (2 + leftCounter2), 246, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j >= currentTime[1] / digit % 10 && j >= bestTime[1] / digit % 10)
                        break;
                }
                digit /= 10;
            }
            DX.DrawRotaGraph(240 + fontInterval * leftCounter1, 200, 0.2, 0, coron);//現在のタイムのコロン
            DX.DrawRotaGraph(240 + fontInterval * leftCounter2, 246, 0.2, 0, coron);//ベストタイムのコロン

            for (int i = 0; i < ranks.Length; i++)//タイム評価
            {
                if (scoreRank == ranks[i])
                {
                    DX.DrawRotaGraph(70, 95, rankImageScale, 0, ResourceLoader.GetGraph("image_result/rank_" + ranks[i] + ".png"));
                    break;
                }
            }
            //タイムの「new」
            if (blinkMessage && currentTime[0] * 60 + currentTime[1] < bestTime[0] * 60 + bestTime[1])
            {
                DX.DrawRotaGraph(frameX + 65 + fontInterval * (leftCounter1 + 3), frameY + 98, 1, 0, newImage);
            }
        }

        public override void OnExit()
        {
        }
    }
}