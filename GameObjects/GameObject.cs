using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public abstract class GameObject
    {
        public float angle, velAngle, size=1;
        public Vec2f pos = Vec2f.ZERO;
        public Vec2f oldPos = Vec2f.ZERO;
        public Vec2f vel = Vec2f.ZERO;
        public Vec2f oldVel = Vec2f.ZERO;

        public float X
        {
            get => pos.X;
        }
        public float Y
        {
            get => pos.Y;
        }

        public readonly Scene scene;

        protected GameObject(Scene scene)
        {
            this.scene = scene;
        }

        public virtual CircleCollision[] GetCollisions()
        {
            return new CircleCollision[]{new CircleCollision(Vec2f.ZERO, size)};
        }

        //接触判定を実行しメゾットを呼ぶ Sceneから呼ぶため
        public void CalcInteract(GameObject obj, float extend = 0)
        {
            foreach (var c0 in GetCollisions())
            {
                foreach (var c1 in obj.GetCollisions())
                {
                    float distance = (pos+c0.pos - obj.pos-c1.pos).Length();
                    if (distance < c0.radius + c1.radius + extend)
                    {
                        //キャンセル可能に
                        if (this.PreInteract(obj, extend) && obj.PreInteract(this, extend))
                        {
                            this.OnInteract(obj, extend);
                            obj.OnInteract(this, extend);
                        }
                        return;
                    } 
                }
            }
        }
        //接触するかを返す
        public bool TryInteract(GameObject obj, float extend = 0)
        {
            float distance = (pos - obj.pos).Length();
            if (distance < size + obj.size + extend)
                //キャンセル可能に
                if (this.PreInteract(obj, extend) && obj.PreInteract(this, extend))
                    return true;
            return false;
        }

        public virtual void Update()
        {
            oldPos = pos;
            oldVel = vel;
            pos = pos + vel;
            angle += velAngle;
        }

        public virtual bool PreInteract(GameObject obj, float extend)
        {
            return true;
        }
        public abstract void OnInteract(GameObject obj,float extend);

        private uint collisionColor = DX.GetColor(0,0,200);
        public virtual void Draw()
        {
            if(Game.ShowCollision)
                foreach (var collision in GetCollisions())
                {
                    Debug.DrawCircle(scene.GetScreenPos(pos+ collision.pos), collision.radius*PlayMap.CellSize.X, collisionColor);
                }
        }
        public abstract bool IsDead();
    }
}