using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class SceneResult : Scene
    {
        int currentScore = 0;
        int bonusScore = 0;
        int bestScore = 0;
        int[] currentTime = new int[] { 0, 0, 0 };
        int[] bestTime = new int[] { 0, 0, 0 };

        string timeRank = "d";
        string scoreRank = "d";

        public int aRankTime = 60;
        public int bRankTime = 120;
        public int cRankTime = 180;

        public int aRankBonus = 1000;
        public int bRankBonus = 500;
        public int cRankBonus = 200;
        
        private bool wait = true;//操作ミス防止用+2重フェード対策
        private bool blinkMessage = true;//点滅表示用
        private int Counter = 0;//wait,fade,blinkのカウンター

        private int fadeTime = 180;

        private const int fontInterval = 30;//文字同士の幅
        private const float fontScale = 0.18f;//文字の大きさ

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
        private int aRankImage = ResourceLoader.GetGraph("rank_3.png");
        private int bRankImage = ResourceLoader.GetGraph("rank_2.png");
        private int cRankImage = ResourceLoader.GetGraph("rank_1.png");

        public SceneResult(Game game) : base(game)
        {
        }

        public override void OnLoad()
        {
            //検証用
            //Game.currentScore = 12345;
            //Game.bestScore = 1234;
            //Game.currentTime = new int[] { 1, 23, 0 };
            //Game.bestTime = new int[] { 12, 34, 0 };

            wait = true;
            blinkMessage = true;
            Counter = 0;

            currentScore = Game.currentScore;
            currentTime = Game.currentTime;
            bestScore = Game.bestScore;
            bestTime = Game.bestTime;
            bonusScore = currentScore;

            fixedPos = new int[] { cursorInterval, Screen.Width - (cursorWidth + cursorInterval) };
            cursorPos = fixedPos[1];

            if (currentTime[0] * 60 + currentTime[1] <= aRankTime)
            {
                bonusScore += aRankBonus;
                timeRank = "a";
            }
            else if (currentTime[0] * 60 + currentTime[1] <= bRankTime)
            {
                bonusScore += bRankBonus;
                timeRank = "b";
            }
            else if (currentTime[0] * 60 + currentTime[1] <= cRankTime)
            {
                bonusScore += cRankBonus;
                timeRank = "c";
            }
            if (bonusScore > bestScore)
                Game.bestScore = bonusScore;
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
            int digit = 10000;
            int leftCounter1 = 0;
            int leftCounter2 = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == bonusScore / digit % 10 && bonusScore / digit != 0)//現在のスコア
                    {
                        DX.DrawRotaGraph(240 + fontInterval * leftCounter1, 104, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter1++;
                    }
                    if (j == bestScore / digit % 10 && bestScore / digit != 0)//ハイスコア
                    {
                        DX.DrawRotaGraph(240 + fontInterval * leftCounter2, 148, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter2++;
                    }
                    if (j >= bonusScore / digit % 10 && j >= bestScore / digit % 10)
                        break;
                }
                digit /= 10;
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
                        DX.DrawRotaGraph(240 + fontInterval * leftCounter1, 200, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter1++;
                    }
                    if (j == bestTime[0] / digit % 10 && (bestTime[0] / digit != 0 || digit == 1))//ベストタイム,分
                    {
                        DX.DrawRotaGraph(240 + fontInterval * leftCounter2, 246, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        leftCounter2++;
                    }
                    if (j >= currentTime[0] / digit % 10 && j >= bestTime[0] / digit % 10)
                        break;
                }
                for (int j = 0; j < 10; j++)
                {
                    if (j == currentTime[1] / digit % 10)//現在のタイム,秒
                        DX.DrawRotaGraph(240 + fontInterval * (2 + leftCounter1), 200, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestTime[1] / digit % 10)//ベストタイム,秒
                        DX.DrawRotaGraph(240 + fontInterval * (2 + leftCounter2), 246, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j >= currentTime[1] / digit % 10 && j >= bestTime[1] / digit % 10)
                        break;
                }
                digit /= 10;
            }
            DX.DrawRotaGraph(240 + fontInterval * leftCounter1, 200, 0.2, 0, coron);//現在のタイムのコロン
            DX.DrawRotaGraph(240 + fontInterval * leftCounter2, 246, 0.2, 0, coron);//ベストタイムのコロン


            if (timeRank == "a")
            {
                //　「タイムボーナス」
                // (+○○○)　の()と+を表示
                DX.DrawRotaGraph(70, 200, 0.3, 0, aRankImage);
                digit = 1000;
                leftCounter1 = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (j == aRankBonus / digit % 10 && aRankBonus / digit != 0)
                        {
                            DX.DrawRotaGraph(420 + 20 * i, 110, 0.1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                            break;
                        }
                    }
                    digit /= 10;
                }
            }
            else if (timeRank == "b")
            {
                
            }
            else if (timeRank == "c")
            {

            }



            if (blinkMessage && bonusScore > bestScore)
            {
                DX.DrawGraph(470, 72, newImage);
            }

            if (blinkMessage && currentTime[0] * 60 + currentTime[1] < bestTime[0] * 60 + bestTime[1])
            {
                DX.DrawGraph(420, 169, newImage);
            }
        }

        public override void OnExit()
        {
        }
    }
}