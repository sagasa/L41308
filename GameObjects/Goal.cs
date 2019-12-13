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
        public Goal(Scene scene,float x,float y) : base(scene)
        {
            pos = new Vec2f(x,y);
        }
        
        public override void Draw()
        {
            DX.DrawGraphF(X, Y, image);
            
        }
        public override void Update()
        {
            
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
