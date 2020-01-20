using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class SceneResult : Scene
    {
        int currentScore = 0;
        int[] currentTime = new int[] { 0, 0, 0 };
        int bestScore = 0;
        int[] bestTime = new int[] { 0, 0, 0 };

        int aRankTime = 60;
        int bRankTime = 180;
        int cRankTime = 300;

        int aRankBonus = 500;
        int bRankBonus = 300;
        int cRankBonus = 100;


        private int interval = 43;
        private float fontScale = 0.2f;

        bool wait = true;
        int waitCounter = 0;

        private int bg = ResourceLoader.GetGraph("play_bg.png");
        private int result_bg = ResourceLoader.GetGraph("image_result/result_bg.png");
        private int back= ResourceLoader.GetGraph("image_result/r_back.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");
        private int newImage = ResourceLoader.GetGraph("image_result/new.png");

        public SceneResult(Game game) : base(game)
        {
        }

        public override void OnLoad()
        {
            wait = true;
            waitCounter = 0;
            bestScore = Game.bestScore;
            bestTime = Game.bestTime;
            currentScore = Game.currentScore;
            currentTime = Game.currentTime;

            if (currentTime[0] * 60 + currentTime[1] <= aRankTime)
            {
                Game.currentScore += aRankBonus;
            }
            else if (currentTime[0] * 60 + currentTime[1] <= bRankTime)
            {
                Game.currentScore += bRankBonus;
            }
            else if (currentTime[0] * 60 + currentTime[1] <= cRankTime)
            {
                Game.currentScore += cRankBonus;
            }
            
            if (Game.currentScore > Game.bestScore)
            {
                Game.bestScore = Game.currentScore;
            }
            if (Game.currentTime[0] * 60 + Game.currentTime[1] < Game.bestTime[0] * 60 + Game.bestTime[1])
            {
                Game.bestTime = Game.currentTime;
            }
        }
        
        public override void Update()
        {
            if(wait)
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

            DX.DrawGraph(0, 0, coron);
            //スコア
            int digit = 10000;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == currentScore / digit % 10 && currentScore / digit != 0)//現在のスコア
                        DX.DrawRotaGraph(240 + interval * i, 100, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestScore / digit % 10 && bestScore / digit != 0)//ハイスコア
                        DX.DrawRotaGraph(240 + interval * i, 150, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j >= currentScore / digit % 10 && j >= bestScore / digit % 10)
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
                        DX.DrawRotaGraph(290 + interval * i, 200, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestTime[0] / digit % 10 && (bestTime[0] / digit != 0 || digit == 1))//ベストタイム,分
                        DX.DrawRotaGraph(240 + interval * i, 250, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j == bestTime[1] / digit % 10)//ベストタイム,秒
                        DX.DrawRotaGraph(290 + interval * i, 250, fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                    if (j >= currentTime[0] / digit % 10 && j >= currentTime[1] / digit % 10 && j >= bestTime[0] / digit % 10 && j >= bestTime[1] / digit % 10)
                        break;
                }
                digit /= 10;
            }

            if (currentScore >= bestScore)
            {
                DX.DrawRotaGraph(0, 0, 1, 0, newImage);
            }

            if (currentTime[0] * 60 + currentTime[1] <= bestTime[0] * 60 + bestTime[1])
            {
                DX.DrawRotaGraph(0, 0, 1, 0, newImage);
            }
        }

        public override void OnExit()
        {
        }
    }
}