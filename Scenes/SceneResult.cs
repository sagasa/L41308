using System;
using DxLibDLL;
using SAGASALib;
using Giraffe.Saves;

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
        private string timeRank = "d";
        private string scoreRank = "d";
        
        private bool blinkMessage = true;//点滅表示用
        private int Counter = 0;//wait,fade,blinkのカウンター
        private const int fadeTime = 120;
        //描画用定数
        private const int frameX = 240;
        private const int frameY = 100;
        private const int fontInterval = 30;//文字同士の幅
        private const float fontScale1 = 0.18f;//文字の大きさ
        private const float fontScale2 = 0.1f;//タイムボーナス用
        private const float rankImageScale = 0.4f;
        private const float rankAnimationSpeed = 0.01f;

        private float rankAnimationScale = 1;//アニメーション用
        private bool rankExpansionAnimation = false;
        private const int cursorWidth = 220;
        private int cursorPosX;
        private const int cursorPosY = Screen.Height - 200;
        private readonly int[] fixedPosX = new int[] { 170, Screen.Width - 170 };
        private int playerPosX;
        private int playerPosY;
        private bool playerMove = false;
        private int playerOnPositon = 0;
        private const int playerMoveSpeed = 4;
        
        private int bg = ResourceLoader.GetGraph("play_bg.png");
        private int result_bg = ResourceLoader.GetGraph("image_result/result_bg.png");
        private int back = ResourceLoader.GetGraph("image_result/r_back.png");
        private int restart = ResourceLoader.GetGraph("image_result/restart.png");
        private int cursor = ResourceLoader.GetGraph("image_result/cursor.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");
        private int newImage = ResourceLoader.GetGraph("image_result/new.png");

        private DummyPlayer dummyPlayer;

        public SceneResult(Game game) : base(game)
        {
            dummyPlayer = new DummyPlayer(this);
        }

        public override void OnLoad()
        {
            blinkMessage = true;
            Counter = 0;
            rankAnimationScale = 1;
            rankExpansionAnimation = false;

            cursorPosX = fixedPosX[1];
            playerPosX = cursorPosX;
            playerPosY = cursorPosY;

            currentScore = Game.currentScore;
            currentTime = Game.currentTime;
            bestScore = Game.bestScore;
            bestTime = Game.bestTime;
            
            for (int i = 0; i < rankTime.Length; i++)//タイムの評価
            {
                if (rankTime[i] >= currentTime[0] * 60 + currentTime[1])
                {
                    currentScore += rankBonus[i];
                    timeRank = ranks[i];
                    break;
                }
            }
            for (int i = 0; i < rankScore.Length; i++)//スコアの評価
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
            if (!playerMove && playerPosX != cursorPosX)
            {
                playerMove = true;
                if (playerPosX == fixedPosX[0])
                    playerOnPositon = fixedPosX[0];
                else if (playerPosX == fixedPosX[1])
                    playerOnPositon = fixedPosX[1];
            }
            else if ((playerOnPositon != fixedPosX[0] && playerPosX == fixedPosX[0]) ||
                     (playerOnPositon != fixedPosX[1] && playerPosX == fixedPosX[1]))
                playerMove = false;

            if (playerMove)
            {
                if (playerOnPositon == fixedPosX[0])
                {
                    dummyPlayer.isDunnyRight = true;
                    playerPosX += playerMoveSpeed;
                    if (playerPosX >= fixedPosX[0] + 50)
                    {
                    }
                }
                else if (playerOnPositon == fixedPosX[1])
                {
                    dummyPlayer.isDunnyRight = false;
                    playerPosX -= playerMoveSpeed;
                }
            }
            else
            {
                if (Input.LEFT.IsPush())
                    dummyPlayer.isDunnyRight = false;
                else if (Input.RIGHT.IsPush())
                    dummyPlayer.isDunnyRight = true;
            }

            if (dummyPlayer.isDunnyRight)
                dummyPlayer.vel = dummyPlayer.vel.SetX(MyMath.Lerp(dummyPlayer.vel.X, 0.01f, 0.1f));
            else
                dummyPlayer.vel = dummyPlayer.vel.SetX(MyMath.Lerp(dummyPlayer.vel.X, -0.01f, 0.1f));
            dummyPlayer.pos = new Vec2f(playerPosX, playerPosY - 85);
            dummyPlayer.Update();
            //player.velAngle = 0;
            //player.angle = 0;

            Counter++;
            if (Counter < fadeTime + 10)
                Game.bgmManager.FadeIn("result", 120);

            if (Counter % 60 == 0)
                blinkMessage = true;
            else if (Counter % 60 == 40)
                blinkMessage = false;
            //ランクのアニメーション?に使用
            if (rankExpansionAnimation)
                rankAnimationScale += rankAnimationSpeed;
            else if (!rankExpansionAnimation)
                rankAnimationScale -= rankAnimationSpeed;
            if (rankAnimationScale > 1)
                rankExpansionAnimation = false;
            else if (rankAnimationScale < 0.75)
                rankExpansionAnimation = true;
            
            if(!Game.fadeAction)
            {
                if (cursorPosX != fixedPosX[0] && Input.LEFT.IsPush())//カーソルが一番左以外の時に←が押されたら、カーソルを一つ左へ
                {
                    Sound.Play("cursor_SE.mp3");
                    for (int i = 0; i < fixedPosX.Length; i++)
                    {
                        if (cursorPosX == fixedPosX[i])
                        {
                            cursorPosX = fixedPosX[i - 1];
                            break;
                        }
                    }
                }
                if (cursorPosX == fixedPosX[fixedPosX.Length - 1] && (Input.RIGHT.IsPush() || Input.RIGHT.IsHold()))
                {
                    if (playerMove)
                    {
                        dummyPlayer.isDunnyRight = true;
                    }
                }
                else if (cursorPosX != fixedPosX[fixedPosX.Length - 1] && Input.RIGHT.IsPush())//カーソルが一番右以外の時→を押されたら、カーソルを一つ右へ
                {
                    Sound.Play("cursor_SE.mp3");
                    for (int i = 0; i < fixedPosX.Length; i++)
                    {
                        if (cursorPosX == fixedPosX[i])
                        {
                            cursorPosX = fixedPosX[i + 1];
                            break;
                        }
                    }
                }
                if (cursorPosX == fixedPosX[0] && Input.ACTION.IsPush())
                {
                    Sound.Play("decision_SE.mp3");
                    Game.fadeAction = true;
                    Game.bgmManager.currentScene = "result";
                    Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                }
                else if (cursorPosX == fixedPosX[1] && Input.ACTION.IsPush())
                {
                    Sound.Play("decision_SE.mp3");
                    Game.fadeAction = true;
                    Game.bgmManager.currentScene = "result";
                    Game.SetScene(new Title(Game), new Fade(fadeTime, true, true));
                }
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, bg);
            DX.DrawGraph(0, 0, result_bg);
            dummyPlayer.Draw();
            DX.DrawRotaGraph(cursorPosX, cursorPosY, 1, 0, cursor);
            DX.DrawRotaGraph(fixedPosX[0], cursorPosY, 1, 0, restart);
            DX.DrawRotaGraph(fixedPosX[1], cursorPosY, 1, 0, back);
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
                    DX.DrawRotaGraph(70, 190, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + ranks[i] + ".png"));

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
                DX.DrawRotaGraph(frameX + 65 + fontInterval * (leftCounter1 + 1) + fontInterval / 2 * leftCounter2, frameY, 1, 0, newImage);
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
                    DX.DrawRotaGraph(70, 95, rankImageScale * rankAnimationScale, 0, ResourceLoader.GetGraph("image_result/rank_" + ranks[i] + ".png"));
                    break;
                }
            }
            //タイムの「new」
            if (blinkMessage && currentTime[0] * 60 + currentTime[1] < bestTime[0] * 60 + bestTime[1])
                DX.DrawRotaGraph(frameX + 65 + fontInterval * (leftCounter1 + 3), frameY + 98, 1, 0, newImage);
        }

        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}