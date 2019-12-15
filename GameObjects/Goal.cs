﻿using System;
using DxLibDLL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAGASALib;

namespace Giraffe
{
    public  class Goal:GameObject
    {
      
        private int image = ResourceLoader.GetGraph("ゴール.png");
        public Goal(Scene scene,Vec2f vecsf) : base(scene)
        {
            pos = vecsf;
        }
        private CircleCollision[] collisions = new CircleCollision[] { new CircleCollision(new Vec2f(2.5f, 1f), 0.8f) };
        public override void Draw()
        {
            Vec2f screenPos = scene.GetScreenPos(pos);
            DX.DrawGraphF(screenPos.X, screenPos.Y, image);
            base.Draw();
        }
        public override CircleCollision[] GetCollisions()
        {
            return collisions;
        }
        public override void Update()
        {
            base.Update();
        }

        public override void OnInteract(GameObject obj, float extend)
        {
            if (obj is Player)
            {
                Game.isGoal = true;
            }
            
        }

        public override bool IsDead()
        {
            return false;
        }
        
    }
}
