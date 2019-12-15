using System;
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
        Game game;
        private int image = ResourceLoader.GetGraph("ゴール.png");
        public Goal(Scene scene,Vec2f vecsf) : base(scene)
        {
            pos = vecsf;
        }
        
        public override void Draw()
        {
            Vec2f screenPos = scene.GetScreenPos(pos);
            DX.DrawGraphF(screenPos.X, screenPos.Y, image);
            base.Draw();
        }
        public override void Update()
        {
            base.Update();
        }

        public override void OnInteract(GameObject obj, float extend)
        {
            game.isGoal = true;
        }

        public override bool IsDead()
        {
            return false;
        }
        
    }
}
