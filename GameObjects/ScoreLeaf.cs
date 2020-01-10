using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    class ScoreLeaf : GameObject
    {
        private int image = ResourceLoader.GetGraph("scoreleaf.png");

        public ScoreLeaf(ScenePlay scene, Vec2f vec2f) : base(scene)
        {
            pos = vec2f;
        }

        private CircleCollision[] collisions = new CircleCollision[] { new CircleCollision(new Vec2f(1, 1.1f), 0.6f) };

        public override CircleCollision[] GetCollisions()
        {
            return collisions;
        }

        public override void Draw()
        {
            Vec2f screenPos = scene.GetScreenPos(pos);
            DX.DrawGraphF(screenPos.X, screenPos.Y, image);
            //DX.DrawGraph(125, 125, branch);
            base.Draw();
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