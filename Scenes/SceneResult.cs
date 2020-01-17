using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class SceneResult : Scene
    {
        int currentScore = 0;
        int[] currentTime = new int[] { 0, 0, 0 };
        int highScore = 0;
        int[] fastestTime = new int[] { 0, 0, 0 };

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
            highScore = Game.highScore;
            fastestTime = Game.fastestTime;
        }
        
        public override void Update()
        {
            if (Game.currentScore > Game.highScore)
            {
                Game.highScore = Game.currentScore;
            }
            if (Game.currentTime[0] * 60 + Game.currentTime[1] < Game.fastestTime[0] * 60 + Game.fastestTime[1])
            {
                Game.fastestTime = Game.currentTime;
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

            //スコア、タイムが機能しているかの確認、消して良いです
            DX.DrawString(300, 100, "ハイスコア: " + highScore, DX.GetColor(0, 0, 0));
            DX.DrawString(300, 115, "今のスコア: " + currentScore, DX.GetColor(0, 0, 0));
            DX.DrawString(300, 150, "最速タイム: " + fastestTime[0] + ":" + fastestTime[1], DX.GetColor(0, 0, 0));
            DX.DrawString(300, 165, "今のタイム: " + currentTime[0] + ":" + currentTime[1], DX.GetColor(0, 0, 0));
        }

        public override void OnExit()
        {
        }
    }
}