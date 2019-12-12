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
            return mapPos * PlayMap.CellSize;
        }

        public bool IsInScreen(Vec2f pos)
        {
            return pos.IsInBetween(MapPos, MapPos + PlayMap.ScreenSize);
        }

        private Player player;
        private Leaf leaf;
        public playerIcon playerIcon;

        private int Flag = ResourceLoader.GetGraph("ハタアイコン.png");
        private int bar = ResourceLoader.GetGraph("マップ.png");

        private PlayMap map;

        //表示中の領域のMap座標の右上
        public Vec2f MapPos;

        public List<GameObject> gameObjects=new List<GameObject>();

        public ScenePlay(Game game) : base(game)
        {
            player = new Player(this);
            leaf = new Leaf(this, new Vec2f(10, 10));

            map = new PlayMap(this, "map1_leaf");
        }


        public override void Draw()
        {
            player.Draw();
            leaf.Draw();
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
            leaf.Update();

        }
    }
}