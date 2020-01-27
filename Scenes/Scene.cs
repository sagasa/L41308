using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    //全シーンの基底 初期化はコンストラクタ使って パーティクルは子クラスで描画を呼んでね
    public abstract class Scene
    {
        public readonly Game Game;

        public readonly ParticleManager ParticleManagerBottom;
        public readonly ParticleManager ParticleManagerTop;

        protected Scene(Game game)
        {
            Game = game;
            ParticleManagerBottom = new ParticleManager(this);
            ParticleManagerTop = new ParticleManager(this);
        }

        public virtual Vec2f GetScreenPos(Vec2f pos)
        {
            return pos;
        }
        public abstract void OnLoad();

        public virtual void Update()
        {
            ParticleManagerBottom.Update();
            ParticleManagerTop.Update();
        }

        public abstract void Draw();
        

        public abstract void OnExit();
    }
}