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
        int iconpos;
        private int image = ResourceLoader.GetGraph("キリンアイコン.png");
        
        public playerIcon(Scene scene) : base(scene)
        {


        }

        public override void Draw()
        {
            DX.DrawGraphF(520, Y+iconpos, image);

        }
        public override void Update()
        {
            iconpos = 460;

            if (Input.RIGHT.IsHold())
            {
                pos = pos + new Vec2f(0, -0.5f);
            }
            else if (Input.LEFT.IsHold())
            {
                pos = pos + new Vec2f(0, -0.5f);
            }

            if (pos.Y <= -300)
            {
                if (Input.RIGHT.IsHold())
                {
                    pos = new Vec2f(0, -300);
                }
                else if (Input.LEFT.IsHold())
                {
                    pos = new Vec2f(0, -300);
                }

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
    