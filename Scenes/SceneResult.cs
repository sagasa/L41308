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

        int interval = 43;
        int interval2 = 28;

        bool wait=true;
        int waitCounter = 0;

        private int bg = ResourceLoader.GetGraph("image_result/result_bg.png");

        public SceneResult(Game game) : base(game)
        {
        }

        public override void OnLoad()
        {
            wait = true;
            waitCounter = 0;
            currentScore = Game.currentScore;
            currentTime = Game.currentTime;
            bestScore = Game.bestScore;
            bestTime = Game.bestTime;
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

            if (Game.currentScore > Game.bestScore)
            {
                Game.bestScore = Game.currentScore;
            }
            if (Game.currentTime[0] * 60 + Game.currentTime[1] < Game.bestTime[0] * 60 + Game.bestTime[1])
            {
                Game.bestTime = Game.currentTime;
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

            currentScore = 88888;// 検証
            currentTime[0] = 5;
            currentTime[1] = 34;
            
            //スコア
            int digit = 10000;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (currentScore / digit % 10 == j && currentScore / digit != 0)
                    {
                        DX.DrawRotaGraph(240 + interval * i, 110, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        break;
                    }
                }
                digit /= 10;
            }
            //タイム
            digit = 10;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)//分
                {
                    if (currentTime[0] / digit % 10 == j && (currentTime[0] / digit != 0 || digit == 1))
                    {
                        DX.DrawRotaGraph(250 + interval * i, 180, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        break;
                    }
                }
                for (int j = 0; j < 10; j++)//秒
                {
                    if (currentTime[1] / digit % 10 == j)
                    {
                        DX.DrawRotaGraph(300 + interval * i, 180, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                        break;
                    }
                }
                digit /= 10;
            }

            //if (currentScore >= bestScore)
            //{
            //    //「new」とか「new record」とかの文字
            //    //点滅させる
            //}
            //else
            if (true)
            {
                //「ハイスコア」とか「ベストタイム」の文字

                int digit2 = 10000;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (bestScore / digit2 % 10 == j && bestScore / digit2 != 0)
                        {
                            DX.DrawRotaGraph(460 + interval2 * i, 120, 0.2, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                            break;
                        }
                    }
                    digit2 /= 10;
                }
            }

            if (currentTime[0] * 60 + currentTime[1] <= bestTime[0] * 60 + bestTime[1])
            {
                //「new」とか「new record」とかの文字
                //点滅させる
            }
            else
            {
                //「ベストタイム」とかの文字

                for (int i = 0; i < 10; i++)
                {
                    //if()
                }
            }
            //DX.DrawString(300, 150, "最速タイム: " + fastestTime[0] + ":" + fastestTime[1], DX.GetColor(0, 0, 0));
            //DX.DrawString(300, 165, "今のタイム: " + currentTime[0] + ":" + currentTime[1], DX.GetColor(0, 0, 0));
        }

        public override void OnExit()
        {
        }
    }
}