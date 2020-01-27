using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    //全シーンの基底 初期化はコンストラクタ使って
    public abstract class Scene
    {
        public readonly Game Game;

        public readonly ParticleManager ParticleManagerBottom;

        protected Scene(Game game)
        {
            Game = game;
            ParticleManagerBottom = new ParticleManager(this);
        }

        public virtual Vec2f GetScreenPos(Vec2f pos)
        {
            return pos;
        }
        public abstract void OnLoad();

        public virtual void Update()
        {
            ParticleManagerBottom.Update();
        }

        public virtual void Draw()
        {
            ParticleManagerBottom.Draw();
        }

        public abstract void OnExit();
    }
}