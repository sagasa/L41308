using System;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class Player : GameObject
    {
        //立ってる場合はfalse
        private bool IsDongle = true;

        //回転速度
        private const float RotateSpeed = (float)Math.PI / 50f;

        //スケール
        private float scale = 1f;

        //首の長さ
        private float neckExtension = 0;

       // private int image = ResourceLoader.GetGraph("player.png");
        private int imageBody = ResourceLoader.GetGraph("player/body.png");
        private int imageHorn = ResourceLoader.GetGraph("player/horn.png");
        private int imageNeck = ResourceLoader.GetGraph("player/neck.png");
        private int[] imageHead = ResourceLoader.GetGraph("player/player_head.png", 4);
        private int[] imageEye = ResourceLoader.GetGraph("player/player_eye.png", 4);
        private int[] imageEar = ResourceLoader.GetGraph("player/player_ear.png", 4);
        private int[] imageLeg = ResourceLoader.GetGraph("player/player_leg.png", 6);
        private int[] imageTail = ResourceLoader.GetGraph("player/player_tail.png", 4);

        public Player(Scene scene) : base(scene)
        {
            velAngle = RotateSpeed/5;
        }

        public bool IsInversion()
        {
            return IsDongle ? velAngle < 0 : vel.X < 0;
        }

        public Vec2f GetCenter()
        {
            return CheckAndInvert(IsDongle ? DangleCenter : StandCenter);
        }

        public Vec2f CheckAndInvert(Vec2f vec)
        {
            if (!IsInversion())
                vec = new Vec2f(128 - vec.X, vec.Y);
            return vec;
        }

        public float GetAngle()
        {

            return IsDongle ? angle + (IsInversion() ? -45 : 45) : angle;
        }

        private float animationProgress = 0;

        private uint white = DX.GetColor(200, 200, 200);
        
        private readonly MultipleRotationCalc _headCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _neckCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _bodyCalc = new MultipleRotationCalc();
        private float count = 0;
        public override void Draw()
        {
            count ++;
            if (60 < count)
                count = -60;
            
            headRotate = MyMath.Deg2Rad*(Math.Abs(count)-30);
            neckRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30)*-1;

            Vec2f _neck = HeadNeckJoint - BodyNeckJoint;

            Vec2f scale = new Vec2f(0.5f, 0.5f);
            _headCalc.Init(scale,Vec2f.ZERO);
            _neckCalc.Init(scale, Vec2f.ZERO);
            _bodyCalc.Init(scale, Vec2f.ZERO);
            if (IsDongle)
            {
                Vec2f center = CheckAndInvert(DangleCenter);
                _headCalc.AddRotate(center, GetAngle());
                _neckCalc.AddRotate(center, GetAngle());
                _bodyCalc.AddRotate(center, GetAngle());
                Vec2f head = CheckAndInvert(HeadNeckJoint);
                _neckCalc.AddRotate(head, headRotate);
                _bodyCalc.AddRotate(head, headRotate);
                Vec2f neck = CheckAndInvert(BodyNeckJoint);
                _bodyCalc.AddRotate(neck, neckRotate);
            }
            //胴
            Draw(imageBody, _bodyCalc);
            //頭
            Draw(imageHorn,_headCalc);
            Draw(AnimationUtils.GetImage(imageHead,animationProgress),_headCalc);
            Draw(AnimationUtils.GetImageLoop(imageEar, animationProgress), _headCalc);
            Draw(AnimationUtils.GetImage(imageLeg, animationProgress),_bodyCalc);
            Draw(AnimationUtils.GetImageLoop(imageEye, animationProgress),_headCalc);
            Draw(AnimationUtils.GetImageLoop(imageTail, animationProgress),_bodyCalc);
            //首
            Draw(imageNeck,_neckCalc);
            
            Vec2f screenPos = scene.GetScreenPos(pos);
            //Debug.DrawVec2(screenPos + _neckOffset);    
            Debug.DrawVec2(screenPos,"Center");
        }

        //軸と角度を指定して回転を追加可能
        private void Draw(int image,MultipleRotationCalc calc)
        {
            //表示位置
            Vec2f screenPos = scene.GetScreenPos(pos)+calc.Offset;
            DX.DrawRotaGraph3F(screenPos.X, screenPos.Y, calc.ScalePivot.X, calc.ScalePivot.Y, calc.Scale.X, calc.Scale.X, calc.Rotate, image, 1, IsInversion() ? DX.FALSE : DX.TRUE);
        }

        //回転
        private float neckRotate = 0;
        private float headRotate = MyMath.Deg2Rad * 30;

        //画像のポジション
        private static readonly Vec2f StandCenter = new Vec2f(64, 64);
        private static readonly Vec2f DangleCenter = new Vec2f(114, 50);
        private static readonly Vec2f HeadNeckJoint = new Vec2f(77, 57);
        private static readonly Vec2f BodyNeckJoint = new Vec2f(77, 74);
        

        public Vec2f GetHeadPos;

        public override void Update()
        {
            animationProgress += 0.02f;
            animationProgress = animationProgress % 1f;
            if ((Input.RIGHT.IsHold() || IsInversion())&&!Input.LEFT.IsHold())
            {

                velAngle = MyMath.Lerp(velAngle, -RotateSpeed,0.01f);
            }
            else
            {
                velAngle = MyMath.Lerp(velAngle, RotateSpeed, 0.01f);
            }

            if (Input.UP.IsHold())
            {
                pos = pos + new Vec2f(0, -0.1f);
            }

            if (Input.LEFT.IsHold())
            {
                pos = pos + new Vec2f(-0.1f, 0);
            }
            if (Input.DOWN.IsHold())
            {
                pos = pos + new Vec2f(0, 0.1f);
            }

            if (Input.RIGHT.IsHold())
            {
                pos = pos + new Vec2f(0.1f, 0);
            }
            base.Update();
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