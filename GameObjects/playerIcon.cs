using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAGASALib;
using DxLibDLL;

namespace Giraffe
{
    public class playerIcon:GameObject
    {
        private int image = ResourceLoader.GetGraph("キリンアイコン.png");
        
        public playerIcon(Scene scene) : base(scene)
        {


        }

        public override void Draw()
        {
            DX.DrawGraphF(X, Y, image);

        }
        public override void Update()
        {
           // if(Input.UP.IsHold())
            {
               // Y += 2;
            }
           // else if(Input.DOWN.IsHold())
            {
               // Y += 2;
            }
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
    