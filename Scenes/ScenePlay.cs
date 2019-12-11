﻿using System;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
        //Map座標からScreen座標へ変換する
        public override Vec2f GetScreenPos(Vec2f mapPos)
        {
            return mapPos*PlayMap.CellSize;
        }

        public bool IsInScreen(Vec2f pos)
        {
            return pos.IsInBetween(MapPos,MapPos+PlayMap.ScreenSize);
        }

        private Player player;
        private Leaf leaf;

        private PlayMap map;

        //表示中の領域のMap座標の右上
        public Vec2f MapPos;

        public List<GameObject> gameObjects;

        public ScenePlay(Game game) : base(game)
        {
            player = new Player(this);
            leaf = new Leaf(this,new Vec2f(10,10));
        }


        public override void Draw()
        {
            player.Draw();
            leaf.Draw();
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