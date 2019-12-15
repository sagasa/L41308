using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    //プレイシーン内専用
    public class Player : GameObject
    {
        //立ってる場合はfalse
        private bool IsDongle = false;

        //回転速度
        private const float RotateSpeed = (float)Math.PI / 30f;
        //移動速度
        private const float WalkSpeed = 0.1f;

        // private int image = ResourceLoader.GetGraph("player.png");
        
       private readonly PlayerRender render;

        public Player(ScenePlay scene) : base(scene)
        {
            render = new PlayerRender(this,ref IsDongle);
            angle = MyMath.Deg2Rad * 0;
            //velAngle = RotateSpeed/10;
        }

        private PlayMap GetMap()
        {
            return ((ScenePlay) scene).Map;
        }

        private void Jamp()
        {
            vel = vel.SetY(-0.2f);
        } 
        //Mapの1番下(地上)にいるか
        public bool IsOnGround() => GetMap().MapSize.Y <= pos.Y;

        private static readonly Vec2f Gravity = new Vec2f(0,0.005f);
        public override void Update()
        {
            if (IsDongle)
            {
                if ((Input.RIGHT.IsHold() || velAngle< 0) && !Input.LEFT.IsHold())
                {

                    velAngle = MyMath.Lerp(velAngle, -RotateSpeed, 0.1f);
                }
                else
                {
                    velAngle = MyMath.Lerp(velAngle, RotateSpeed, 0.1f);
                }
            }
            else
            {
                //重力
                vel += Gravity;
                //接地判定
                if (IsOnGround())
                {
                    pos = pos.SetY(GetMap().MapSize.Y);
                    vel = vel.SetY(0);
                }
                if (Input.UP.IsHold()&&IsOnGround())
                {
                    Jamp();
                }

                //左右操作
                if (Input.LEFT.IsHold())
                {
                    vel = vel.SetX(MyMath.Lerp(vel.X, -WalkSpeed, 0.1f));
                }
                if (Input.RIGHT.IsHold())
                {
                    vel = vel.SetX(MyMath.Lerp(vel.X, WalkSpeed, 0.1f));
                }

                if (!Input.LEFT.IsHold() && !Input.RIGHT.IsHold())
                {
                    vel = vel.SetX(MyMath.Lerp(vel.X, 0, 0.2f));
                }
            }

            base.Update();
        }


        private float count = 0;
        public override void Draw()
        {
            count++;
            if (60 < count)
                count = -60;

            //render.HeadRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30);
            //render.NeckRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30) * -1;
            //render.NeckExt = (Math.Abs(count)/30f)+1;

            render.Draw();
            base.Draw();
        }

        public override bool IsDead()
        {
            return false;
        }

        public override void OnInteract(GameObject obj, float extend)
        {

        }
    }
}