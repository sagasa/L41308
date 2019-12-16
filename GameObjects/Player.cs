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

        //接地時の中心の高さ
        private const float StandOffset = 1f;

        // private int image = ResourceLoader.GetGraph("player.png");
        enum PlayerState
        {
            Fly,Stand,Dongle
        }

        private PlayerState _state = PlayerState.Stand;
        
        private readonly PlayerRender render;

        public Player(ScenePlay scene) : base(scene)
        {
            size = 0.6f;
            render = new PlayerRender(this);
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
        public bool IsOnGround() => GetMap().MapSize.Y <= pos.Y+StandOffset;

        private static readonly Vec2f Gravity = new Vec2f(0,0.005f);
        public override void Update()
        {
            //TODO 仮で画面外に出ないように
            if (pos.X < 0) 
                pos = pos.SetX(PlayMap.ScreenSize.X);
            if (PlayMap.ScreenSize.X<pos.X)
                pos = pos.SetX(0);
            //スクロール
            if (Screen.Height * 0.8f< scene.GetScreenPos(pos).Y)
            {
                ((ScenePlay) scene).MapPos = ((ScenePlay) scene).MapPos.SetY(((ScenePlay) scene).MapPos.Y+0.03f);
            }
            if (scene.GetScreenPos(pos).Y < Screen.Height * 0.4f)
            {
                ((ScenePlay)scene).MapPos = ((ScenePlay)scene).MapPos.SetY(((ScenePlay)scene).MapPos.Y - 0.03f);
            }
            if (Screen.Height * 0.9f < scene.GetScreenPos(pos).Y)
            {
                ((ScenePlay)scene).MapPos = ((ScenePlay)scene).MapPos.SetY(((ScenePlay)scene).MapPos.Y + 0.1f);
            }
            if (scene.GetScreenPos(pos).Y < Screen.Height * 0.2f)
            {
                ((ScenePlay)scene).MapPos = ((ScenePlay)scene).MapPos.SetY(((ScenePlay)scene).MapPos.Y - 0.3f);
            }

            //操作系+状態変更
            if (_state==PlayerState.Dongle)
            {
                //ぶら下がり
                if ((Input.RIGHT.IsHold() || velAngle< 0) && !Input.LEFT.IsHold())
                {

                    velAngle = MyMath.Lerp(velAngle, -RotateSpeed, 0.1f);
                }
                else
                {
                    velAngle = MyMath.Lerp(velAngle, RotateSpeed, 0.1f);
                }

                if (!Input.ACTION.IsHold())
                {
                    _state = PlayerState.Fly;
                    vel = (new Vec2f(-1, 0) * velAngle).Rotate(angle);
                    Sound.Play("jump_SE.mp3");
                }
            }
            else if(_state == PlayerState.Stand)
            {
                //地上
                //重力
                vel += Gravity;
                if (Input.ACTION.IsHold()&&IsOnGround())
                {
                    Jamp();
                    Sound.Play("jump_SE.mp3");
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

                if ((Input.LEFT.IsPush() && !Input.RIGHT.IsHold()) || (Input.RIGHT.IsPush() && !Input.LEFT.IsHold()))
                {
                    Sound.Loop("step_SE.mp3");
                }
            }
            else if(_state == PlayerState.Fly)
            {
                //飛翔
                //重力
                vel += Gravity/3;
                //接地判定
                if (IsOnGround())
                {
                    angle = 0;
                    velAngle = 0;
                    _state = PlayerState.Stand;
                    render.IsDongle = false;
                }
                
            }
            //重力とぶら下がるフラグ
            if (_state == PlayerState.Fly || _state == PlayerState.Stand)
            {
                
                //接地
                if (IsOnGround())
                {
                    pos = pos.SetY(GetMap().MapSize.Y- StandOffset);
                    //下向きの速度を0に
                    if(vel.Y>0)
                        vel = vel.SetY(0);
                }

                //葉の接触
                if (currentLeaf != null && Input.ACTION.IsHold())
                {
                    Sound.Play("leaf_bite_SE.mp3");
                    vel = Vec2f.ZERO;
                    _state = PlayerState.Dongle;
                    render.IsDongle = true;
                }
            }

            if (!IsOnGround() || (!Input.LEFT.IsHold() && !Input.RIGHT.IsHold()))
            {
                Sound.Stop("step_SE.mp3");
            }

            base.Update();
            currentLeaf = null;
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
            Debug.DrawVec2(scene.GetScreenPos(pos),(new Vec2f(-1,0)*velAngle).Normal().Rotate(angle)*50);
            render.Draw();
            base.Draw();
        }

        public override bool IsDead()
        {
            return false;
        }

        private CircleCollision[] collision = new CircleCollision[]{ new CircleCollision(Vec2f.ZERO, 0.1f) };
        public override CircleCollision[] GetCollisions()
        {
            return collision;
        }

        //このアップデートで触れている葉
        private Leaf currentLeaf = null;

        public override void OnInteract(GameObject obj, float extend)
        {
            if (obj is Leaf)
            {
                currentLeaf = (Leaf)obj;
            }
        }
    }
}