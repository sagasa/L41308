﻿using System;
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
        private int particlePoint = ResourceLoader.GetGraph("image_effect/effectscore.png");
        private int particleSelect = ResourceLoader.GetGraph("image_effect/effectselect_1.png");
        private int palticleTutorial = ResourceLoader.GetGraph("image_effect/effectselect_2.png");


        private Scene scene;

        private static readonly Vec2f scale = new Vec2f(1,1) / PlayMap.CellSize;

        public ParticleManager (Scene scene)
        {
            this.scene = scene;
        }
        //全パーティクルを更新する
        public void Update()
        {
            int count = particles.Count;
            for (int i = 0; i < count; i++)
            {
                particles[i].Update();
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
            for (int i = 0; i < 6; i++)
            {
                particles.Add(new Particle(scene)
                {
                    pos = pos_ + new Vec2f(MyRandom.PlusMinus(20), MyRandom.PlusMinus(60f)) * scale,
                    lifeSpan = MyRandom.Range(10, 90),
                    imageHndle = particleGlitter,
                    vel = new Vec2f(MyRandom.PlusMinus(0.5f), MyRandom.Range(-1f, -1f)) * scale,
                    startScale = MyRandom.Range(0.7f, 1.4f),
                    endScale = MyRandom.Range(0.3f, 0.6f),
                    fadeInTime = 0.5f,
                    fadeOutTime = 1.5f,
                    blendMode = DX.DX_BLENDMODE_ADD,
                });
            }
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
            for (int i = 0; i < 15; i++)
            {
                float angle = MyRandom.PlusMinus(MyMath.PI);
                float speed = MyRandom.Range(2f, 8f);

                particles.Add(new Particle(scene)
                {
                    pos = pos,
                    lifeSpan = MyRandom.Range(40, 70),
                    imageHndle = particleSwaying,
                    vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                    force = new Vec2f(0, 0.55f) * scale,
                    damp = 0.86f,
                    endScale = 0.3f,
                    fadeOutTime = 0.8f,
                    blendMode = DX.DX_BLENDMODE_ADD,
                    red = 0,
                    green = 255,
                    blue = 0,

                });
            }
               
        }

        //枯れ葉
        public void Swaying2(Vec2f pos)
        {
            for (int i = 0; i < 30; i++)
            {
                float angle = MyRandom.PlusMinus(MyMath.PI);
                float speed = MyRandom.Range(2f, 8f);

                particles.Add(new Particle(scene)
                {
                    pos = pos,
                    lifeSpan = MyRandom.Range(40, 70),
                    imageHndle = particleSwaying,
                    vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                    force = new Vec2f(0, 0.55f) * scale,
                    damp = 0.86f,
                    endScale = 0.3f,
                    fadeOutTime = 0.8f,
                    blendMode = DX.DX_BLENDMODE_ADD,
                    red = 94,
                    green = 69,
                    blue =0,

                });
            }
               
        }

        //ボーナススコア
        public void GetPoint(Vec2f pos)
        {
            particles.Add(new Particle(scene)
            {
                pos = pos + new Vec2f(0, MyRandom.PlusMinus(40f)) * scale,
                lifeSpan=50,
                imageHndle=particlePoint,
                vel=new Vec2f(0,-3f)*scale,
                startScale=1.2f,
                endScale=0.4f,
                fadeInTime=0.7f,
                fadeOutTime=1.3f,
                blendMode=DX.DX_BLENDMODE_ADD,
            });
        }

        //ステージセレクト
        public void Select(Vec2f pos)
        {
            particles.Add(new Particle(scene)
            {
                pos=pos,
                lifeSpan=60,
                imageHndle=particleSelect,
            });
        }

        //チュートリアル
        public void Tutorial(Vec2f pos)
        {
            particles.Add(new Particle(scene)
            {
                pos=pos,
                lifeSpan=10,
                imageHndle=palticleTutorial,
            });
        }
    }
}
