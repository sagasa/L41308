using System;
using System.Collections.Generic;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
   
    public class ParticleManager
    {
        //パーティクルを入れておくためのリスト
        List<Particle> particles = new List<Particle>();

        private int particleGlitter = ResourceLoader.GetGraph("image_effect/effectitem.png");
        private int particleJump = ResourceLoader.GetGraph("image_effect/effectjump.png");
        private int particleSwaying = ResourceLoader.GetGraph("image_effect/effectleaf_1.png");

        private Scene scene;

        public ParticleManager (Scene scene)
        {
            this.scene = scene;
        }
        //全パーティクルを更新する
        public void Update()
        {
            foreach (Particle particle in particles)
            {
                particle.Update();
            }

            //死んでいる粒子はリストから除去
            particles.RemoveAll(p => p.state == State.Dead);
        }

        //全パーティクルを描画する
        public void Draw()
        {
            foreach (Particle particle in particles)
            {
                particle.Draw();
            }
        }

        //キラキラ
        public void Glitter(Vec2f pos_)
        {
            particles.Add(new Particle(scene)
            {
                pos= pos_+ new Vec2f(MyRandom.PlusMinus(20), MyRandom.PlusMinus(20)),
                lifeSpan = MyRandom.Range(10, 70),
                imageHndle = particleGlitter,
                vel = new Vec2f(MyRandom.PlusMinus(0.5f), MyRandom.Range(-1f, -1f)),
                startScale = MyRandom.Range(0.4f, 0.8f),
                endScale = MyRandom.Range(0.2f, 0.4f),
                fadeInTime = 0.5f,
                fadeOutTime = 1.0f,
                blendMode = DX.DX_BLENDMODE_ADD,
            });
        }

        //ジャンプ
        public void Jump(Vec2f pos)
        {
            particles.Add(new Particle(scene)
            {
                pos= pos,
                lifeSpan = 10,
                imageHndle = particleJump,
                startScale = 0.8f,
                endScale = 1.2f,
                fadeOutTime = 1f,
                
            });
        }

        //葉
        public void Swaying(Vec2f pos)
        {
            particles.Add(new Particle(scene)
            {

                pos = pos,
                lifeSpan = 1,
                imageHndle = particleSwaying,
                vel = new Vec2f(0, -5.5f),
                force = new Vec2f(0, 0.08f),
                fadeInTime = 50,
                blendMode = DX.DX_BLENDMODE_ADD,
                OnDeath = (p) =>
                {
                    for (int i = 0; i < 25; i++)
                    {
                        float angle = MyRandom.PlusMinus(MyMath.PI);
                        float speed = MyRandom.Range(2f, 8f);

                        particles.Add(new Particle(scene)
                        {
                            pos = p.pos,
                            lifeSpan = MyRandom.Range(40, 70),
                            imageHndle = particleSwaying,
                            vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed),
                            force=new Vec2f(0, 0.55f) ,
                            damp = 0.92f,
                            endScale = 0.5f,
                            fadeOutTime = 0.8f,
                            blendMode = DX.DX_BLENDMODE_ADD,

                        });
                    }
                }
            });
        }
    }
}
