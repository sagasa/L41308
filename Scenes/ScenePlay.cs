using System;
using DxLibDLL;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
        public static int score = 0;
        int[] time = new int[] { 0, 0, 0 };//分,秒,フレーム
        
        int goalTimer = 300;
        int fadeTime = 180;
        //Map座標からScreen座標へ変換する
        public override Vec2f GetScreenPos(Vec2f mapPos)
        {
            return (mapPos- MapPos) * PlayMap.CellSize;
        }

        public bool IsInScreen(Vec2f pos)
        {
            return pos.IsInBetween(MapPos, MapPos + PlayMap.ScreenSize);
        }

        private Player player;
        public playerIcon playerIcon;

        private int Flag = ResourceLoader.GetGraph("ハタアイコン.png");
        private int bar = ResourceLoader.GetGraph("マップ.png");
        private int playbg = ResourceLoader.GetGraph("play_bg.png"); //背景描画
        private int scoreImage = ResourceLoader.GetGraph("image_play/score.png");
        private int stageName = ResourceLoader.GetGraph("image_play/stagename_" + 1 + ".png");
        private int watch = ResourceLoader.GetGraph("tokei.png");
        private int colon = ResourceLoader.GetGraph("image_play/colon.png");
        
        public PlayMap Map { get; private set; }
         
        //表示中の領域の左上のMap座標
        public Vec2f MapPos;

        ParticleManager ParticleManager;

        public List<GameObject> gameObjects=new List<GameObject>();

        public ScenePlay(Game game) : base(game)
        {
            Map = new PlayMap(this, "map_2");
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);

            player = new Player(this);
            player.pos = MapPos+new Vec2f(2,2);
            playerIcon = new playerIcon(this);

            ParticleManager = new ParticleManager();
        }


        public override void Draw()
        {
            Vec2f pos = GetScreenPos(Vec2f.ZERO);
            DX.DrawGraph((int)pos.X, (int)pos.Y, playbg);
            gameObjects.ForEach(obj => obj.Draw());
            player.Draw();
            
            DX.DrawGraph(520, 200, bar);
            DX.DrawGraph(525, 150, Flag);
            playerIcon.Draw();

            DX.DrawRotaGraph(100, 23 , 0.6, 0, stageName);
            DX.DrawRotaGraph(Screen.Width / 2 - 22, 23, 0.6, 0, scoreImage);
            DX.DrawRotaGraph(Screen.Width - 155, 25, 0.55, 0, watch);
            DX.DrawRotaGraph(Screen.Width - 75, 25, 0.7, 0, colon);
            for (int i = 0; i < 10; i++)
            {
                //スコア
                if (score / 10000 == i)//10000
                    DX.DrawRotaGraph(Screen.Width / 2 - 10, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                if (score / 1000 % 10 == i)//1000
                    DX.DrawRotaGraph(Screen.Width / 2 + 15, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                if (score / 100 % 10 == i)//100
                    DX.DrawRotaGraph(Screen.Width / 2 + 40, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                if (score / 10 % 10 == i)//10
                    DX.DrawRotaGraph(Screen.Width / 2 + 65, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                if (score % 10 == i)//1
                    DX.DrawRotaGraph(Screen.Width / 2 + 90, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                //タイム
                if (time[0] / 10 == i)//10分
                    DX.DrawRotaGraph(Screen.Width - 120, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                if (time[0] % 10 == i)//1分
                    DX.DrawRotaGraph(Screen.Width - 95, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                if (time[1] / 10 == i)//10秒
                    DX.DrawRotaGraph(Screen.Width - 55, 25, 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                if (time[1] % 10 == i)//1秒
                    DX.DrawRotaGraph(Screen.Width - 30, 25 , 0.45, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
            }

            ParticleManager.Draw();
        }

        public override void OnExit()
        {
        }

        public override void OnLoad()
        {
            Game.isGoal = false;
            time[0] = 0;
            time[1] = 0;
            time[2] = 0;
            score = 0;
        }

        public override void Update()
        {
            if (!Game.isGoal)
            {
                Game.bgmManager.CrossFade("title", "play",fadeTime);

                time[2]++;
                if (time[2] >= 60)
                {
                    time[1]++;
                    time[2] = 0;
                }
                if (time[1] >= 60)
                {
                    time[0]++;
                    time[1] = 0;
                }
            }

            gameObjects.ForEach(obj=> player.CalcInteract(obj));
            player.Update();
            playerIcon.Update();
            gameObjects.ForEach(obj => obj.Update());
            gameObjects.RemoveAll(obj => obj.IsDead());

            ParticleManager.Update();

            if (Game.isGoal)//ゴールにプレイヤーが触れたら
            {
                player.pos = player.oldPos;
                if (goalTimer > 240)
                {
                    Game.bgmManager.FadeOut("play", 30);
                }

                if (goalTimer == 300)
                {
                    Sound.Play("goal_jingle.mp3");
                }
                goalTimer--;
                if (goalTimer == 0)
                {
                    Game.currentScore = score;
                    Game.currentTime = time;
                    Game.SetScene(new SceneResult(Game),new Fade(fadeTime,true,true));
                }
            }
        }
    }
}