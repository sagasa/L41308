using System;
using DxLibDLL;
using SAGASALib;


namespace Giraffe
{
    public class PlayerRender
    {
        private static readonly int imageBody = ResourceLoader.GetGraph("player/body.png");
        private static readonly int imageHorn = ResourceLoader.GetGraph("player/horn.png");
        private static readonly int imageNeck = ResourceLoader.GetGraph("player/neck.png");
        private static readonly int[] imageHead = ResourceLoader.GetGraph("player/player_head.png", 4);
        private static readonly int[] imageEye = ResourceLoader.GetGraph("player/player_eye.png", 4);
        private static readonly int[] imageEar = ResourceLoader.GetGraph("player/player_ear.png", 4);
        private static readonly int[] imageLeg = ResourceLoader.GetGraph("player/player_leg.png", 6);
        private static readonly int[] imageTail = ResourceLoader.GetGraph("player/player_tail.png", 4);

        //画像のポジション
        public static readonly Vec2f ImageSize = new Vec2f(128, 128);
        private static readonly Vec2f StandCenterPos = new Vec2f(64, 64) / ImageSize;
        private static readonly Vec2f DangleCenterPos = new Vec2f(114, 50) / ImageSize;
        private static readonly Vec2f HeadNeckJointPos = new Vec2f(77, 57) / ImageSize;
        private static readonly Vec2f BodyNeckJointPos = new Vec2f(77, 74) / ImageSize;

        private Vec2f Center { get => CheckAndInvert(IsDongle ? DangleCenterPos : StandCenterPos); }
        private Vec2f HeadNeckJoint { get => CheckAndInvert(HeadNeckJointPos); }
        private Vec2f BodyNeckJoint { get => CheckAndInvert(BodyNeckJointPos); }
        //スケール
        public Vec2f Scale = new Vec2f(1,1);
        //首スケール
        public float NeckExt = 1;
        //回転
        public float NeckRotate = 0;
        public float HeadRotate = 0;
        public bool IsDongle;
        //アニメーション
        public float MouthProgress = 0;
        public float EyeProgress = 0;
        public float EarProgress = 0;
        public float LegProgress = 0;
        public float TailProgress = 0;

        

        //表示対象
        public readonly GameObject Target;

        public PlayerRender(GameObject target)
        {
            Target = target;
        }


        private bool IsInversion()
        {
            return IsDongle ? 0 < Target.velAngle : Target.vel.X < 0;
        }

        
        private Vec2f CheckAndInvert(Vec2f vec)
        {
            if (IsInversion())
                vec = new Vec2f(1f - vec.X, vec.Y);
            return vec;
        }

        private float GetAngle()
        {

            return IsDongle ? Target.angle + (IsInversion() ? 45*MyMath.Deg2Rad : -45 * MyMath.Deg2Rad) : Target.angle;
        }

        private readonly MultipleRotationCalc _headCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _neckCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _bodyCalc = new MultipleRotationCalc();
        public  void Draw()
        {
            Vec2f screenPos = Target.scene.GetScreenPos(Target.pos);

            Vec2f _neck = HeadNeckJoint - BodyNeckJoint;
            if (IsDongle)
            {
                _neck *= -1;
            }
            Vec2f extPivot = IsDongle? HeadNeckJoint - Center : BodyNeckJoint - Center;
            

            

            _headCalc.Clear();
            _neckCalc.Clear();
            _bodyCalc.Clear();
            //スケール
            if (IsDongle)
            {
                _neckCalc.Scale(new Vec2f(1, NeckExt), HeadNeckJoint);
            }
            else
            {
                _neckCalc.Scale(new Vec2f(1, NeckExt), BodyNeckJoint);
            }
            //回転
            _headCalc.Rotate(Center, GetAngle());
            _neckCalc.Rotate(Center, GetAngle());
            _bodyCalc.Rotate(Center, GetAngle());
            
            if (IsDongle)
            {
                _neckCalc.Rotate(HeadNeckJoint, HeadRotate);
            }
            else
            {
                _neckCalc.Rotate(BodyNeckJoint, NeckRotate);
            }
            //表示サイズに
            Vec2f scale = new Vec2f(128, 128);
            //*
            _headCalc.Move(Center * -1);
            _neckCalc.Move(Center * -1);
            _bodyCalc.Move(Center * -1);
            //*/
            _headCalc.Scale(scale, Vec2f.ZERO);
            _neckCalc.Scale(scale, Vec2f.ZERO);
            _bodyCalc.Scale(scale, Vec2f.ZERO);


            //胴
            Draw(imageBody, _bodyCalc);
            //頭
            Draw(imageHorn, _headCalc);
            Draw(AnimationUtils.GetImage(imageHead, MouthProgress), _headCalc);
            Draw(AnimationUtils.GetImageLoop(imageEar, EarProgress), _headCalc);
            Draw(AnimationUtils.GetImage(imageLeg, LegProgress), _bodyCalc);
            Draw(AnimationUtils.GetImageLoop(imageEye, EyeProgress), _headCalc);
            Draw(AnimationUtils.GetImageLoop(imageTail, TailProgress), _bodyCalc);
            //首
            Draw(imageNeck, _neckCalc);

            Debug.DrawVec2(screenPos, "Center");
        }

        //軸と角度を指定して回転を追加可能
        private void Draw(int image, MultipleRotationCalc calc)
        {
            //表示位置
            Vec2f screenPos = Target.scene.GetScreenPos(Target.pos);
            calc.Draw(screenPos,image,IsInversion());
        }
    }
}