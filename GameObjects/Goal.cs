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

        private int image;

        public Goal(ScenePlay scene, Vec2f vecsf) : base(scene)
        {
            image = ResourceLoader.GetGraph("goal" + scene.ResourcesName + ".png");
            pos = vecsf;
        }

        private static readonly Vec2f Offset = new Vec2f(2f, 1.8f);
        private CircleCollision[] collisions = new CircleCollision[] { new CircleCollision(Offset, 1.3f) };
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
        private static readonly int marker = ResourceLoader.GetGraph("marker.png");
        public override void Update()
        {
#if DEBUG
            if (Input.BACK.IsPush())
            {
                ((ScenePlay)scene).Goal(pos);
            }
#endif
            ((ScenePlay)scene).Navi.AddTarget(pos+ Offset, marker);
            base.Update();
        }

        public override void OnInteract(GameObject obj, float extend)
        {
            if (obj is Player)
            {
                ((ScenePlay)scene).Goal(pos);
            }
            
        }


        public override bool IsDead()
        {
            return false;
        }
        
    }
}
