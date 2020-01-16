using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe 
{
    class Leaf : GameObject
    {
        private int leafImage = ResourceLoader.GetGraph("leaf4.png");
        private int scoreLeafImage = ResourceLoader.GetGraph("scoreleaf.png");
        //private int branch = ResourceLoader.GetGraph("branch2.png");

        public int score;

        public Leaf(ScenePlay scene, Vec2f vec2f, int scoreValue) : base(scene)
        {
            pos = vec2f;
            score = scoreValue;
        }
        public Leaf(Tutolal tutolal, Vec2f vec2) : base(tutolal)
        {
            pos = vec2;
            score = 0;
        }

        private CircleCollision[] collisions = new CircleCollision[]{new CircleCollision(new Vec2f(1,1.1f), 0.6f) };

        public override CircleCollision[] GetCollisions()
        {
            return collisions;
        }

        public override void Draw()
        {
            if (score == 0)
            {
                Vec2f screenPos = scene.GetScreenPos(pos);
                DX.DrawGraphF(screenPos.X, screenPos.Y, leafImage);
                //DX.DrawGraph(screenPos.X, screenPos.Y, branch);
                base.Draw();
            }
            else if (score != 0)
            {
                Vec2f screenPos = scene.GetScreenPos(pos);
                DX.DrawGraphF(screenPos.X, screenPos.Y, scoreLeafImage);
                //DX.DrawGraph(screenPos.X, screenPos.Y, branch);
                base.Draw();
            }
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