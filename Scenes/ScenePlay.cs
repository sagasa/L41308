using System;
using DxLibDLL;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
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
        }


        public override void Draw()
        {
            gameObjects.ForEach(obj=>obj.Draw());
            player.Draw();

            
            DX.DrawGraph(520, 200, bar);
            DX.DrawGraph(525, 150, Flag);

        }

        public override void OnExit()
        {
        }

        public override void OnLoad()
        {
        }

        public override void Update()
        {
            player.Update();
            gameObjects.ForEach(obj => obj.Update());
            gameObjects.RemoveAll(obj => obj.IsDead());

            if (Game.isGoal==true)//ゴールにプレイヤーが触れたら
            {

            }
        }
    }
}