using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe 
{
    public class Leaf : GameObject
    {
 

        //private int branch = ResourceLoader.GetGraph("branch2.png");
        private int[] leafImages = ResourceLoader.GetGraph("leafes_1.png", 5);
        private int[] scoreLeafImages = ResourceLoader.GetGraph("s_leafes_1.png", 5);
        public readonly AnimationManager<Leaf> AnimationManager; 

        public int score { get; private set; }

        public Leaf(ScenePlay scene, Vec2f vec2f, int score) : base(scene)
        {
            leafImages = ResourceLoader.GetGraph("leafes"+scene.ResourcesName+".png", 5);
            scoreLeafImages = ResourceLoader.GetGraph("s_leafes" + scene.ResourcesName + ".png", 5);
            pos = vec2f;
            this.score = score;
            AnimationManager = new AnimationManager<Leaf>(this);
            if (score != 0)
            {
                AnimationManager.Start(Animations.GliterParticle);
            }
        }

        //使用したならプレイヤーから呼ぶ
        public void RemoveScore()
        {
            if (score !=0)
            {
                score = 0;
                AnimationManager.Stop(Animations.GliterParticle);
            }
               
        }

        private CircleCollision[] collisions = new CircleCollision[]{new CircleCollision(new Vec2f(1,1.1f), 0.6f) };

        public override CircleCollision[] GetCollisions()
        {
            return collisions;
        }


        public override void Draw()
        {
            Vec2f screenPos = scene.GetScreenPos(pos);
            if (score == 0)
            {
                int index = MyMath.Clamp((int)(screenPos.X / Screen.Width * leafImages.Length), 0, leafImages.Length-1);
                DX.DrawGraphF(screenPos.X, screenPos.Y, leafImages[index]);
                //DX.DrawGraph(screenPos.X, screenPos.Y, branch);
                base.Draw();
            }
            else if (score != 0)
            {
              //  scene.ParticleManagerTop.Glitter(pos);

                int index = MyMath.Clamp((int)(screenPos.X / Screen.Width * scoreLeafImages.Length), 0, scoreLeafImages.Length - 1);
                DX.DrawGraphF(screenPos.X, screenPos.Y, scoreLeafImages[index]);
                //DX.DrawGraph(screenPos.X, screenPos.Y, branch);
                base.Draw();
            }
            AnimationManager.Update();
        }

        public override bool IsDead()
        {
            return false;
        }

        public override void OnInteract(GameObject obj, float extend)
        {
            // パーティクル
            //scene.ParticleManagerBottom.Swaying(pos);
        }

        public override void Update()
        {

            base.Update();
        }
    }
}