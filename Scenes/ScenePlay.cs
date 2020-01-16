using System;
using DxLibDLL;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
        int score = 0;
        int[] time = new int[] { 0, 0, 0 };
        
        

        int goalTimer = 300;
        
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
        
        private int stageName = ResourceLoader.GetGraph("image_play/stagename_1.png");
        private int watch = ResourceLoader.GetGraph("tokei.png");

        private int font0 = ResourceLoader.GetGraph("image_effect/time_0.png");
        private int font1 = ResourceLoader.GetGraph("image_effect/time_1.png");
        private int font2 = ResourceLoader.GetGraph("image_effect/time_2.png");
        private int font3 = ResourceLoader.GetGraph("image_effect/time_3.png");
        private int font4 = ResourceLoader.GetGraph("image_effect/time_4.png");
        private int font5 = ResourceLoader.GetGraph("image_effect/time_5.png");
        private int font6 = ResourceLoader.GetGraph("image_effect/time_6.png");
        private int font7 = ResourceLoader.GetGraph("image_effect/time_7.png");
        private int font8 = ResourceLoader.GetGraph("image_effect/time_8.png");
        private int font9 = ResourceLoader.GetGraph("image_effect/time_9.png");

        private int fadeTime = 180;

        public PlayMap Map { get; private set; }

        //表示中の領域の左上のMap座標
        public Vec2f MapPos;

        public List<GameObject> gameObjects=new List<GameObject>();

        public ScenePlay(Game game) : base(game)
        {
            Map = new PlayMap(this, "map_2");
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);

            player = new Player(this);
            player.pos = MapPos+new Vec2f(2,2);
            playerIcon = new playerIcon(this);
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

            DX.DrawRotaGraph(128, 22, 0.8, 0, stageName);
            DX.DrawRotaGraph(Screen.Width / 2 + 30, 22, 0.8, 0, scoreImage);
            DX.DrawRotaGraph(Screen.Width - 150, 22, 0.6, 0, watch);

            DX.DrawString(Screen.Width / 2 + 30, 15, score + "", DX.GetColor(0, 0, 0));
            for (int i = 0; i < 10; i++)
            {
                if (time[0] / 10 == i)//10分
                {
                    DX.DrawRotaGraph(Screen.Width - 118, 22, 0.5, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                }
                if (time[0] % 10 == i)//1分
                {
                    DX.DrawRotaGraph(Screen.Width - 90, 22, 0.5, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                }
                if (time[1] / 10 == i)//10秒
                {
                    DX.DrawRotaGraph(Screen.Width - 53, 22, 0.5, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                }
                if (time[1] % 10 == i)//1秒
                {
                    DX.DrawRotaGraph(Screen.Width - 25, 22, 0.5, 0, ResourceLoader.GetGraph("image_effect/time_" + i + ".png"));
                }
            }
        }

        public override void OnExit()
        {
        }

        public override void OnLoad()
        {
            Game.isGoal = false;
        }

        public override void Update()
        {
            time[2]++;
            if(time[2]>=60)
            {
                time[1]++;
                time[2] = 0;
            }
            if(time[1]>=60)
            {
                time[0]++;
                time[1] = 0;
            }

            if (!Game.isGoal)
            {
                Game.bgmManager.CrossFade("title", "play",fadeTime);
            }

            gameObjects.ForEach(obj=> player.CalcInteract(obj));
            player.Update();
            playerIcon.Update();
            gameObjects.ForEach(obj => obj.Update());
            gameObjects.RemoveAll(obj => obj.IsDead());

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
                    Game.SetScene(new Title(Game),new Fade(fadeTime,true,true));
                }
            }
        }
    }
}