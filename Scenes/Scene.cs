using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    //全シーンの基底 初期化はコンストラクタ使って
    public abstract class Scene
    {
        public readonly Game Game;
        protected Scene(Game game)
        {
            Game = game;
        }

        public virtual Vec2f GetScreenPos(Vec2f pos)
        {
            return pos;
        }
        public abstract void OnLoad();

        public abstract void Update();

        public abstract void Draw();

        public abstract void OnExit();
    }
}