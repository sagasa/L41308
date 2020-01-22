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

        int aRankTime = 60;
        int bRankTime = 180;
        int cRankTime = 300;

        int aRankBonus = 500;
        int bRankBonus = 300;
        int cRankBonus = 100;


        private int interval = 30;
        private float fontScale = 0.18f;

        bool wait = true;
        int waitCounter = 0;

        bool blinkMessage = true;//点滅表示用
        int blinkCounter = 0;

        private int bg = ResourceLoader.GetGraph("play_bg.png");
        private int result_bg = ResourceLoader.GetGraph("image_result/result_bg.png");
        private int back = ResourceLoader.GetGraph("image_result/r_back.png");
        private int restart = ResourceLoader.GetGraph("image/restart.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");
        private int newImage = ResourceLoader.GetGraph("image_result/new.png");

        public SceneResult(Game game) : base(game)
        {
        }

        public override void OnLoad()
        {
            //検証
            Game.currentScore = 10000;
            Game.bestScore = 10300;
            Game.currentTime = new int[] { 3, 0, 50 };
            Game.bestTime = new int[] { 1, 0, 30 };
            //

            wait = true;
            waitCounter = 0;
            blinkMessage = true;
            blinkCounter = 0;

            currentScore = Game.currentScore;
            currentTime = Game.currentTime;
            bestScore = Game.bestScore;
            bestTime = Game.bestTime;
            bonusScore = currentScore;

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
            blinkCounter++;
            if (blinkCounter % 60 == 0)
            {
                blinkMessage = true;
            }
            else if (blinkCounter % 60 == 40)
            {
                blinkMessage = false;
            }
            if (wait)
            {
                waitCounter++;
            }
            if (waitCounter == 120)
            {
                wait = false;
            }

            if (Input.ACTION.IsPush() && !wait)
            {
                wait = true;
                Game.SetScene(new Title(Game), new Fade(180, true, true));
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, bg);
            DX.DrawGraph(0, 0, result_bg);

            DX.DrawRotaGraph(240 + interval * 2, 200, 0.2, 0, coron);
            DX.DrawRotaGraph(240 + interval * 2, 246, 0.2, 0, coron);
            //スコア
            int digit = 10000;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == bonusScore / digit % 10 && bonusScore / digit != 0)//現在のスコア
                        DX.DrawRotaGraph(240 + interval * i, 104, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestScore / digit % 10 && bestScore / digit != 0)//ハイスコア
                        DX.DrawRotaGraph(240 + interval * i, 148, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j >= bonusScore / digit % 10 && j >= bestScore / digit % 10)
                        break;
                }
                digit /= 10;
            }
            //タイム
            digit = 10;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == currentTime[0] / digit % 10 && (currentTime[0] / digit != 0 || digit == 1))//現在のタイム,分
                        DX.DrawRotaGraph(240 + interval * i, 200, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == currentTime[1] / digit % 10)//現在のタイム,秒
                        DX.DrawRotaGraph(240 + interval * 3 + interval * i, 200, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestTime[0] / digit % 10 && (bestTime[0] / digit != 0 || digit == 1))//ベストタイム,分
                        DX.DrawRotaGraph(240 + interval * i, 246, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestTime[1] / digit % 10)//ベストタイム,秒
                        DX.DrawRotaGraph(240 + interval * 3 + interval * i, 246, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j >= currentTime[0] / digit % 10 && j >= currentTime[1] / digit % 10 && j >= bestTime[0] / digit % 10 && j >= bestTime[1] / digit % 10)
                        break;
                }
                digit /= 10;
            }

            if (timeRank == "a")
            {
                // (+○○○)　の()と+を表示

                digit = 100;
                for (int i = 0; i < 3; i++)
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