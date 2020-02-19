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
        private Scene scene;

        private int particleGlitter = ResourceLoader.GetGraph("image_effect/effectitem.png");
        private int particleJump = ResourceLoader.GetGraph("image_effect/effectjump.png");
        private int[] particleSwaying = ResourceLoader.GetGraph("image_effect/effectleaf_0.png",5);
        private int[] particleSwaying2 = ResourceLoader.GetGraph("image_effect/s_leffecteaf_0.png", 5);
        private int particlePoint = ResourceLoader.GetGraph("image_effect/effectscore.png");
        private int FierWark = ResourceLoader.GetGraph("image_effect /particle_dot_1.png ");
        private int FierWark2 = ResourceLoader.GetGraph("image_effect /particle_dot_3.png ");

        private int particleSelect = ResourceLoader.GetGraph("image_effect/effectselect_1.png");
        private int palticleTutorial = ResourceLoader.GetGraph("image_effect/effectselect_0.png");

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

        //キラキラ
        public void Glitter2(Vec2f pos_)
        {
            particles.Add(new Particle(scene)
            {
                pos = pos_ + new Vec2f(MyRandom.PlusMinus(45f), MyRandom.PlusMinus(12f)) * scale,
                lifeSpan = MyRandom.Range(10, 80),
                imageHndle = particleGlitter,
                vel = new Vec2f(MyRandom.PlusMinus(0.5f), MyRandom.Range(-1f, -1f)) * scale,
                startScale = MyRandom.Range(0.5f, 1.0f),
                endScale = MyRandom.Range(0.3f, 0.6f),
                fadeInTime = 0.5f,
                fadeOutTime = 1.5f,
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
        public void Swaying(ScenePlay scenePlay, Vec2f pos)
        {
            if (scenePlay.ResourcesName == "_0")
            {
                for (int i = 0; i < 15; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos + new Vec2f(0, MyRandom.PlusMinus(40f)) * scale,
                        lifeSpan = MyRandom.Range(40, 70),
                        imageHndle = ResourceLoader.GetGraph("image_effect/effectleaf" + scenePlay.ResourcesName + ".png"),  //particleSwaying,
                        vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(0, 0.55f) * scale,
                        damp = 0.86f,
                        endScale = 0.3f,
                        fadeOutTime = 0.8f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        red = 252,
                        green = 174,
                        blue = 242,
                    });
                }
            }

            if (scenePlay.ResourcesName == "_1")
            {
                for (int i = 0; i < 15; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos + new Vec2f(0, MyRandom.PlusMinus(40f)) * scale,
                        lifeSpan = MyRandom.Range(40, 70),
                        imageHndle = ResourceLoader.GetGraph("image_effect/effectleaf" + scenePlay.ResourcesName + ".png"),  //particleSwaying,
                        vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(0, 0.55f) * scale,
                        damp = 0.86f,
                        endScale = 0.3f,
                        fadeOutTime = 0.8f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        red = 10,
                        green = 222,
                        blue = 3,
                    });
                }
            }

            if (scenePlay.ResourcesName == "_2")
            {
                for (int i = 0; i < 15; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos + new Vec2f(0, MyRandom.PlusMinus(40f)) * scale,
                        lifeSpan = MyRandom.Range(40, 70),
                        imageHndle = ResourceLoader.GetGraph("image_effect/effectleaf" + scenePlay.ResourcesName + ".png"),  //particleSwaying,
                        vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(0, 0.55f) * scale,
                        damp = 0.86f,
                        endScale = 0.3f,
                        fadeOutTime = 0.8f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        red = 210,
                        green = 68,
                        blue = 49,
                    });
                }
            }

            if (scenePlay.ResourcesName == "_3")
            {
                for (int i = 0; i < 15; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos + new Vec2f(0, MyRandom.PlusMinus(40f)) * scale,
                        lifeSpan = MyRandom.Range(40, 70),
                        imageHndle = ResourceLoader.GetGraph("image_effect/effectleaf" + scenePlay.ResourcesName + ".png"),  //particleSwaying,
                        vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(0, 0.55f) * scale,
                        damp = 0.86f,
                        endScale = 0.3f,
                        fadeOutTime = 0.8f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        red = 119,
                        green = 74,
                        blue = 40,
                    });
                }
            }

        }

        

        //枯れ葉
        public void Swaying2(ScenePlay scenePlay, Vec2f pos)
        {
            if (scenePlay.ResourcesName == "_1")
            {
                for (int i = 0; i < 30; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos,
                        lifeSpan = MyRandom.Range(40, 70),
                        imageHndle = ResourceLoader.GetGraph("image_effect/s_leffecteaf" + scenePlay.ResourcesName + ".png"),
                        vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(0, 0.55f) * scale,
                        damp = 0.86f,
                        endScale = 0.3f,
                        fadeOutTime = 0.8f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        red =213,
                        green = 222,
                        blue = 83,
                    });
                }
            }

            if (scenePlay.ResourcesName == "_2")
            {
                for (int i = 0; i < 30; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos,
                        lifeSpan = MyRandom.Range(40, 70),
                        imageHndle = ResourceLoader.GetGraph("image_effect/s_leffecteaf" + scenePlay.ResourcesName + ".png"),
                        vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(0, 0.55f) * scale,
                        damp = 0.86f,
                        endScale = 0.3f,
                        fadeOutTime = 0.8f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        red = 213,
                        green = 222,
                        blue = 83,
                    });
                }
            }

            if (scenePlay.ResourcesName == "_3")
            {
                for (int i = 0; i < 30; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos,
                        lifeSpan = MyRandom.Range(40, 70),
                        imageHndle = ResourceLoader.GetGraph("image_effect/s_leffecteaf" + scenePlay.ResourcesName + ".png"),
                        vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(0, 0.55f) * scale,
                        damp = 0.86f,
                        endScale = 0.3f,
                        fadeOutTime = 0.8f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        red = 255,
                        green = 255,
                        blue = 255,
                    });
                }
            }
        }

        //
        public void Swaying3(ScenePlay scenePlay, Vec2f pos)
        {
            for (int i = 0; i < 30; i++)
            {
                float angle = MyRandom.PlusMinus(MyMath.PI);
                float speed = MyRandom.Range(2f, 8f);

                particles.Add(new Particle(scene)
                {
                    pos = pos + new Vec2f(0, MyRandom.PlusMinus(40f)) * scale,
                    lifeSpan = MyRandom.Range(40, 70),
                    imageHndle = ResourceLoader.GetGraph("image_effect/effectleaf" + scenePlay.ResourcesName + ".png"),
                    vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                    force = new Vec2f(0, 0.55f) * scale,
                    angularDamp=0.98f,
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

        //クリア花火
        public void FireWorks(Vec2f pos)
        {
            //int[,] color =
            //{
            //    {255, 170, 170},
            //    {170, 170, 255},
            //    {240, 240, 160},
            //    {255, 170, 170},
            //};

            //int colorPattern = MyRandom.Range(0, 2);

            particles.Add(new Particle(scene)
            {
                //ヒューと上に向かって飛ぶ1つ
                pos = pos + new Vec2f(MyRandom.PlusMinus(120), MyRandom.PlusMinus(20)) * scale,
                lifeSpan = 60,
                imageHndle = FierWark,
                vel = new Vec2f(0, -5.5f) * scale,
                force = new Vec2f(0, 0.08f) * scale,
                startScale = 0.18f,
                endScale = 0.14f,
                blendMode = DX.DX_BLENDMODE_ADD,
                OnDeath = (p) =>
                {
                    //消滅時に、周囲に50個の粒子を飛ばす
                    for (int i = 0; i < 15; i++)
                    {
                        float angle = MyRandom.PlusMinus(MyMath.PI);
                        float speed = MyRandom.Range(2f, 8f);

                        particles.Add(
                            new Particle(scene)
                            {
                                pos = p.pos,
                                lifeSpan = MyRandom.Range(40, 70),
                                imageHndle = FierWark2,
                                vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                                damp = 0.95f,
                                force = new Vec2f(0, 0.06f) * scale,
                                startScale = 0.2f,
                                endScale = 0.15f,
                                fadeOutTime = 0.7f,
                                red = 255,
                                green = 170,
                                blue = 170,
                                //red = color[colorPattern * 2, 0],
                                //green = color[colorPattern * 2, 1],
                                //blue = color[colorPattern * 2, 2],
                                blendMode = DX.DX_BLENDMODE_ADD,
                                OnUpdate = (p2) =>
                                {
                                        //毎フレーム粒子をその場に残す
                                        particles.Add(new Particle(scene)
                                    {
                                        pos = p2.pos,
                                        lifeSpan = 50,
                                        imageHndle = FierWark2,
                                        force = new Vec2f(0, 0.05f) * scale,
                                        damp = 0.94f,
                                        startScale = 0.16f,
                                        endScale = 0.11f,
                                        alpha = p2.currentAlpha,
                                        fadeOutTime = 1f,
                                        red = 255,
                                        green = 170,
                                        blue = 170,
                                        //    red = color[colorPattern * 2, 0],
                                        //green = color[colorPattern * 2, 1],
                                        //blue = color[colorPattern * 2, 2],
                                        blendMode = DX.DX_BLENDMODE_ADD,
                                    });
                                },
                            });
                    }
                    for (int i = 0; i < 30; i++)
                    {
                        float angle = MyRandom.PlusMinus(MyMath.PI);
                        float speed = MyRandom.Range(2f, 8f);

                        particles.Add(
                            new Particle(scene)
                            {
                                pos = p.pos,
                                lifeSpan = MyRandom.Range(40, 50),
                                imageHndle = FierWark2,
                                vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                                damp = 0.95f,
                                force = new Vec2f(0, 0.06f) * scale,
                                startScale = 0.2f,
                                endScale = 0.15f,
                                fadeOutTime = 0.7f,
                                red = 170,
                                green = 170,
                                blue = 255,
                                //red = color[colorPattern * 2 + 1, 0],
                                //green = color[colorPattern * 2 + 1, 1],
                                //blue = color[colorPattern * 2 + 1, 2],
                                blendMode = DX.DX_BLENDMODE_ADD,
                                OnUpdate = (p2) =>
                                {
                                    particles.Add(new Particle(scene)
                                    {
                                        pos = p2.pos,
                                        lifeSpan = 50,
                                        imageHndle = FierWark2,
                                        force = new Vec2f(0, 0.05f) * scale,
                                        damp = 0.94f,
                                        startScale = 0.16f,
                                        endScale = 0.11f,
                                        alpha = p2.currentAlpha,
                                        fadeOutTime = 1f,
                                        red = 170,
                                        green = 170,
                                        blue = 255,
                                        //red = color[colorPattern * 2 + 1, 0],
                                        //green = color[colorPattern * 2 + 1, 1],
                                        //blue = color[colorPattern * 2 + 1, 2],
                                        blendMode = DX.DX_BLENDMODE_ADD,
                                    });
                                },
                            });
                    }

                },
                OnUpdate = (p2) =>
                {
                    particles.Add(new Particle(scene)
                    {
                        pos = p2.pos,
                        lifeSpan = 30,
                        imageHndle = FierWark2,
                        force = new Vec2f(0, 0.05f) * scale,
                        damp = 0.94f,
                        startScale = 0.16f,
                        endScale = 0.11f,
                        alpha = p2.currentAlpha,
                        fadeOutTime = 2.5f,
                        red = 255,
                        green = 255,
                        blue = 0,
                        blendMode = DX.DX_BLENDMODE_ADD,
                    });
                },

            });
            
            
        }

        //セレクトエフェクト
        public void Serekut(Title title, Vec2f pos)
        {
            if (title.ResourceName == "_1")
            {
                for (int i = 0; i < 15; i++)
                {
                    float angle = MyRandom.PlusMinus(MyMath.PI);
                    //float anglex = MyRandom.Range(-0.08f, -0.12f);
                    //float angley = MyRandom.Range(-0.08f, 0.02f);
                    float speed = MyRandom.Range(2f, 8f);

                    particles.Add(new Particle(scene)
                    {
                        pos = pos + new Vec2f(MyRandom.PlusMinus(25), 0) * scale,
                        lifeSpan = MyRandom.Range(60, 120),
                        imageHndle = ResourceLoader.GetGraph("image_effect/effectleaf" + title.ResourceName + ".png"),
                        vel = new Vec2f(0, -3f) * scale,
                        //vel = new Vec2f((float)Math.Cos(angle) * speed, (float)Math.Sin(angle) * speed) * scale,
                        force = new Vec2f(MyRandom.Range(-0.08f, -0.12f), MyRandom.Range(-0.08f, 0.02f)) * scale,
                        damp = 0.86f,
                        fadeInTime = 0.1f,
                        fadeOutTime = 1.4f,
                        blendMode = DX.DX_BLENDMODE_ADD,
                        angle = MyRandom.PlusMinus(MyMath.PI),
                        angularDamp = 0.98f,
                        //damp = 0.95f,
                        red = 252,
                        green = 174,
                        blue = 242,
                    });
                }
            }
        }



    }
}
