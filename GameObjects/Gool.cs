using System;
using DxLibDLL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAGASALib;

namespace Giraffe
{
    public  class Gool:GameObject
    {
        private int image = ResourceLoader.GetGraph("ゴール.png");
        public Gool(Scene scene,float x,float y) : base(scene)
        {
            imageWidth = 320;
            imageHeight = 200;
            this.x=x;
            this.y =y;
        }
        
        public override void Draw()
        {
            DX.DrawGraphF(x, y, image);
            
        }
        public override void Update()
        {
            
        }
        public override void OnCollision(GameObject other)
        {
          
        }

        public override void OnInteract(GameObject obj, float extend)
        {
           
        }

        public override bool IsDead()
        {
            return false;
        }
        
    }
}
