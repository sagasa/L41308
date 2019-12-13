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
            render = new PlayerRender(this);
            angle = MyMath.Deg2Rad * 45;
            //   velAngle = RotateSpeed/5;
        }

        private PlayMap GetMap()
        {
            return ((ScenePlay) scene).Map;
        }

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
                //接地判定
                if (GetMap().MapSize.Y< pos.Y)
                {
                    pos = pos.SetY(GetMap().MapSize.Y);
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

            if (Input.UP.IsHold())
            {
                pos = pos + new Vec2f(0, -0.1f);
            }

            
            if (Input.DOWN.IsHold())
            {
                pos = pos + new Vec2f(0, 0.1f);
            }

           
            base.Update();
        }


        private float count = 0;
        public override void Draw()
        {
            count++;
            if (60 < count)
                count = -60;

            render.HeadRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30);
            render.NeckRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30) * -1;
            render.NeckExt = (Math.Abs(count)/30f)+1;

            render.Draw();
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