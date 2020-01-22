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
        public float IconPos;
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

            pos = new Vec2f(0, -IconPos);
            if (IconPos >= 460)
            {
                IconPos = 460;
            }
            else if (IconPos >= 160)
            {
                IconPos = 160;
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
    