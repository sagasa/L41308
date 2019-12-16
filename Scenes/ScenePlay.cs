using System;
using DxLibDLL;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
        int goolTimer = 60;
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


        public PlayMap Map { get; private set; }

        //表示中の領域の左上のMap座標
        public Vec2f MapPos;

        public List<GameObject> gameObjects=new List<GameObject>();

        public ScenePlay(Game game) : base(game)
        {
            Map = new PlayMap(this, "map1_leaf");
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);

            player = new Player(this);
            player.pos = MapPos+new Vec2f(2,2);
            playerIcon = new playerIcon(this);
        }


        public override void Draw()
        {
            Vec2f pos = GetScreenPos(Vec2f.ZERO);
            DX.DrawGraph((int)pos.X, (int)pos.Y, playbg);
            gameObjects.ForEach(obj=>obj.Draw());
            player.Draw();

            
            DX.DrawGraph(520, 200, bar);
            DX.DrawGraph(525, 150, Flag);
            playerIcon.Draw();

        }

        public override void OnExit()
        {
            Game.soundManager.Remove("play");
        }

        public override void OnLoad()
        {
        }

        public override void Update()
        {
            Game.soundManager.CrossFade("title", "play",3);
            gameObjects.ForEach(obj=> player.CalcInteract(obj));
            player.Update();
            playerIcon.Update();
            gameObjects.ForEach(obj => obj.Update());
            gameObjects.RemoveAll(obj => obj.IsDead());

            if (Game.isGoal==true)//ゴールにプレイヤーが触れたら
            {
                player.pos = player.oldPos;
                goolTimer--;
                if (goolTimer <= 0)
                {
                    Game.SetScene(new Title(Game));
                }
               
            }
        }
    }
}