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

            x = 500;
            y = 390;

        }

        public override void Draw()
        {
            DX.DrawGraphF(x, y, image);

        }
        public override void Update()
        {
            if(Input.UP.IsHold())
            {
                y -= 2;
            }
            else if(Input.DOWN.IsHold())
            {
                y += 2;
            }
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
    