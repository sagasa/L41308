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

        bool wait=false;

        private int bg = ResourceLoader.GetGraph("image_result/result_bg.png");

        public SceneResult(Game game) : base(game)
        {
        }

        public override void OnLoad()
        {
            wait = false;
            currentScore = Game.currentScore;
            currentTime = Game.currentTime;
            bestScore = Game.bestScore;
            bestTime = Game.bestTime;
        }
        
        public override void Update()
        {
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

            currentScore = 02345;

            for (int i = 0; i < 10; i++)
            {
                //スコア
                if (currentScore / 10000 == i)//10000
                    DX.DrawRotaGraph(250, 110, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                if (currentScore / 1000 % 10 == i)//1000
                    DX.DrawRotaGraph(295, 110, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                if (currentScore / 100 % 10 == i)//100
                    DX.DrawRotaGraph(340, 110, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                if (currentScore / 10 % 10 == i)//10
                    DX.DrawRotaGraph(385, 110, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                if (currentScore % 10 == i)//1
                    DX.DrawRotaGraph(430, 110, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                //タイム
                if (currentTime[0] / 10 == i)//10分
                    DX.DrawRotaGraph(250, 180, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                if (currentTime[0] % 10 == i)//1分
                    DX.DrawRotaGraph(295, 180, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                if (currentTime[1] / 10 == i)//10秒
                    DX.DrawRotaGraph(385, 180, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                if (currentTime[1] % 10 == i)//1秒
                    DX.DrawRotaGraph(430, 180, 0.3, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
            }

            if (currentScore >= bestScore)
            {
                //「new」とか「new record」とかの文字
                //点滅させる
            }
            else
            {
                //「ハイスコア」とか「ベストタイム」の文字
                //その下にハイスコア表示
                for (int i = 0; i < 10; i++)
                {

                }
            }

            if (currentTime[0] * 60 + currentTime[1] <= bestTime[0] * 60 + bestTime[1])
            {
                //「new」とか「new record」とかの文字
                //点滅させる
            }
            else
            {
                //「ベストタイム」
            }
            //DX.DrawString(300, 150, "最速タイム: " + fastestTime[0] + ":" + fastestTime[1], DX.GetColor(0, 0, 0));
            //DX.DrawString(300, 165, "今のタイム: " + currentTime[0] + ":" + currentTime[1], DX.GetColor(0, 0, 0));
        }

        public override void OnExit()
        {
        }
    }
}