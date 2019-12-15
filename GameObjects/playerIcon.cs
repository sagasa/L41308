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
        int a;
        private int image = ResourceLoader.GetGraph("キリンアイコン.png");
        
        public playerIcon(Scene scene) : base(scene)
        {


        }

        public override void Draw()
        {
            DX.DrawGraphF(520, Y+a, image);

        }
        public override void Update()
        {
            a = 460;
           
             if (Input.UP.IsHold())
            {
                pos = pos + new Vec2f(0, -1f);
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
    