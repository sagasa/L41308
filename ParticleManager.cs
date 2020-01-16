using System;
using System.Collections.Generic;
using DxLibDLL;
using SAGASALib;

namespace Test
{
   
    public class ParticleManager
    {
        //パーティクルを入れておくためのリスト
        List<Particle> particles = new List<Particle>();

        private int particleGlitter = ResourceLoader.GetGraph("effectitem.png");
        private int particleJump = ResourceLoader.GetGraph("effectjump.png");

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
        public void Glitter(float x, float y)
        {
            particles.Add(new Particle()
            {
                x = x + MyRandom.PlusMinus(20),
                y = y + MyRandom.PlusMinus(20),
                lifeSpan = MyRandom.Range(10, 70),
                imageHndle = particleGlitter,
                vx = MyRandom.PlusMinus(0.5f),
                vy = MyRandom.Range(-1f, -1f),
                startScale = MyRandom.Range(0.4f, 0.8f),
                endScale = MyRandom.Range(0.2f, 0.4f),
                fadeInTime = 0.5f,
                fadeOutTime = 1.0f,
                blendMode = DX.DX_BLENDMODE_ADD,
            });
        }

        //ジャンプ
        public void Jump(float x, float y)
        {
            particles.Add(new Particle()
            {
                x = x,
                y = y,
                lifeSpan = 10,
                imageHndle = particleJump,
                startScale = 0.8f,
                endScale = 1.2f,
                fadeOutTime = 1f,
                //angle = angle,
            });
        }
    }
}
