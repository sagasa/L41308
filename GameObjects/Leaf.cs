using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe 
{
    class Leaf : GameObject
    {

        private int image = ResourceLoader.GetGraph("leaf4.png");
        //private int branch = ResourceLoader.GetGraph("branch2.png");

        public Leaf(Scene scene):base(scene)
        {
            x = 100;
            y = 100;
        }

        public override void Draw()
        {
            DX.DrawGraph(100, 100, image);
            //DX.DrawGraph(125, 125, branch);
        }

        public override bool IsDead()
        {
            return false;
        }

        public override void OnInteract(GameObject obj, float extend)
        {
          
        }

        public override void Update()
        {
            base.Update();
        }
      
    }
}
