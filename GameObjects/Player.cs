using System;
using System.Collections.Generic;
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

        //飛び出すときの速度倍率
        private static readonly Vec2f ShootSpeed = new Vec2f(1,1.2f);

        //接地時の中心の高さ
        private const float StandOffset = 1f;

        //描画
        private readonly PlayerRender _render;

        //アニメーションリスト
        private AnimationManager<PlayerRender> _animation;

        // private int image = ResourceLoader.GetGraph("player.png");
        public enum PlayerState
        {
            Fly,Stand,Dongle,Stop
        }

        private PlayerState _state = PlayerState.Fly;
        
        

        public Player(ScenePlay scene) : base(scene)
        {
            size = 0.6f;
            _render = new PlayerRender(this);
            _animation = new AnimationManager<PlayerRender>(_render);
            angle = MyMath.Deg2Rad * 0;
            //velAngle = RotateSpeed/3;
        }


        private PlayMap GetMap()
        {
            return ((ScenePlay) scene).Map;
        }

        private void Jump()
        {
            vel = vel.SetY(-0.18f);
            scene.ParticleManagerBottom.Jump(pos);
        }

        public void Goal(Vec2f pos)
        {
            _state = PlayerState.Stop;
            this.pos=pos;
            angle = 0;
            vel = Vec2f.ZERO;
            velAngle = 0;
            _render.State = PlayerState.Stand;
            _animation.StopAll();
            _animation.Start(Animations.GoalAngle);
            _animation.Start(Animations.GoalNeck);
        }
        //Mapの1番下(地上)にいるか
        public bool IsOnGround() => GetMap().MapSize.Y <= pos.Y+StandOffset;

        private static readonly Vec2f Gravity = new Vec2f(0,0.005f);
        public override void Update()
        {
            //TODO 仮で画面外に出ないように
            /*
            if (pos.X < 0) 
                pos = pos.SetX(PlayMap.ScreenSize.X);
            if (PlayMap.ScreenSize.X<pos.X)
                pos = pos.SetX(0);
            //*/

            //画面外の検知
            if (!((ScenePlay)scene).IsInScreen(pos))
            {
                ((ScenePlay)scene).Scroll((((ScenePlay)scene).MapPos + PlayMap.ScreenSize / 2 - pos) * -1);
            }

            pos = ((ScenePlay) scene).GetFixedPos(pos);


            if (Screen.Width * 0.8f < scene.GetScreenPos(pos).X)
            {
                float f = (Screen.Width * 0.25f) / (Screen.Width - scene.GetScreenPos(pos).X) *1;
                ((ScenePlay)scene).Scroll(new Vec2f(0.05f * f, 0));
            }
            if (scene.GetScreenPos(pos).X< Screen.Width * 0.2f)
            {
                float f = (Screen.Width * 0.25f) / scene.GetScreenPos(pos).X*1f;
                ((ScenePlay)scene).Scroll(new Vec2f(-0.05f * f, 0));
            }
            //スクロール
            if (scene.GetScreenPos(pos).Y < Screen.Height * 0.4f)
            {
                float f = (Screen.Height * 0.4f) / scene.GetScreenPos(pos).Y;
                ((ScenePlay)scene).Scroll(new Vec2f(0, -0.03f * f));
            }
            if (Screen.Height * 0.8f < scene.GetScreenPos(pos).Y)
            {
                float f = scene.GetScreenPos(pos).Y / (Screen.Height * 0.8f)*2;
                ((ScenePlay)scene).Scroll(new Vec2f(0, 0.1f * f));
            }


            /*
            if (_state == PlayerState.Dongle && Input.ACTION.IsPush())
                _state = PlayerState.Fly;
            else if (_state == PlayerState.Fly && Input.ACTION.IsPush())
                _state = PlayerState.Stand;
            else if (_state == PlayerState.Stand && Input.ACTION.IsPush())
                _state = PlayerState.Dongle;
            _render.State = _state;
            base.Update();
            return;
            //*/
            //首の伸縮
            if (_state != PlayerState.Stop)
            {
                if (Input.UP.IsHold() && _render.NeckExt < 2f)
                {
                    _render.NeckExt += 0.1f;
                }

                if (Input.DOWN.IsHold() && 0.85f < _render.NeckExt)
                {
                    _render.NeckExt -= 0.1f;
                }
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
                    //飛び出す 過度な加速はしない
                    vel = (new Vec2f(-1, 0) * velAngle).Rotate(angle);
                    vel *= ShootSpeed;
                    if (Math.Abs(velAngle) < RotateSpeed * 2)
                        velAngle *= 2.5f;

                    //姿勢変更時に中心の移動分補完
                    _state = PlayerState.Fly;
                    _render.State = _state;
                    pos -= _render.GetMouthOffset();
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
                    Jump();
                    Sound.Play("jump_SE.mp3");
                    //ジャンプ時の姿勢変更
                    _animation.Start(Animations.MouthOpen);
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
            else if(_state == PlayerState.Fly)
            {
                //飛翔
                //重力
                vel += Gravity/3;
                //左右移動 微妙に動く
                //左右操作
                if (Input.LEFT.IsHold())
                {
                    vel += new Vec2f(-0.0001f,0);
                }
                if (Input.RIGHT.IsHold())
                {
                    vel += new Vec2f(0.0001f, 0);
                }

                //回転の減速
                velAngle = MyMath.Lerp(velAngle, velAngle < 0 ? -RotateSpeed : RotateSpeed, 0.02f);
                //接地判定
                if (IsOnGround())
                {
                    angle = 0;
                    velAngle = 0;
                    _state = PlayerState.Stand;
                    _render.State = _state;
                    //地上歩行アニメーション
                    _animation.Start(Animations.StandAngle);
                    _animation.StartNext(Animations.StandAngle,Animations.IdleAnimation);
                    _animation.Start(Animations.WalkGround);

                    _animation.Start(Animations.MouthClose);
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

                    //口が開いているなら閉める
                    if (0 < _render.MouthProgress)
                        _animation.Start(Animations.MouthClose);
                }

                //葉の接触
                if (currentLeaf != null && Input.ACTION.IsHold())
                {
                    if (currentLeaf.score != 0)
                    {
                        ScenePlay.score += currentLeaf.score;
                        currentLeaf.RemoveScore();
                       
                        //パーティクル
                        scene.ParticleManagerTop.Glitter(pos);
                        scene.ParticleManagerBottom.Swaying2(pos);
                        scene.ParticleManagerTop.GetPoint(pos);
                    }
                    else
                    {
                        //パーティクル
                            scene.ParticleManagerBottom.Swaying(pos);
                    }
                    Sound.Play("leaf_bite_SE.mp3");
                    //姿勢変更時に中心の移動分補完
                    pos += _render.GetMouthOffset();

                    //立っていた状態からの場合初速を与える
                    if (_state == PlayerState.Stand)
                        velAngle = 0 <= vel.X ? RotateSpeed * -0.3f : RotateSpeed * 0.3f;
                    _state = PlayerState.Dongle;
                    _render.State = _state;
                    
                    vel = Vec2f.ZERO;
                    //ぶら下がり時の姿勢変更
                    _animation.Start(Animations.MouthClose);
                    _animation.Stop(Animations.WalkGround);
                    _animation.Stop(Animations.IdleAnimation);
                    _animation.Start(Animations.DongleAngle);
                }
            }

            if (IsOnGround()&&((Input.LEFT.IsPush() && !Input.RIGHT.IsHold()) || (Input.RIGHT.IsPush() && !Input.LEFT.IsHold())))
            {
                Sound.Loop("step_SE.mp3");
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
            /*
            count++;
            if (60 < count)
                count = -60;

            _render.HeadRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30);
            _render.NeckRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30) * -1;
            _render.NeckExt = (Math.Abs(count)/20f)+1;
            //*/
           
            //Debug.DrawVec2(scene.GetScreenPos(pos),(new Vec2f(-1,0)*velAngle).Normal().Rotate(angle)*50);
            _render.Draw();
            base.Draw();

            //アニメーションアップデート ほんとにここでいいか不明
            _animation.Update();
        }

        public override bool IsDead()
        {
            return false;
        }

        private CircleCollision[] collision = new CircleCollision[]{ new CircleCollision(Vec2f.ZERO, 0.1f) };
        public override CircleCollision[] GetCollisions()
        {
            collision[0] = new CircleCollision(_render.GetMouthOffset(),0.1f);
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