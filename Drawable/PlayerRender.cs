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
        private static readonly Vec2f StandCenter = new Vec2f(64, 64);
        private static readonly Vec2f DangleCenter = new Vec2f(114, 50);
        private static readonly Vec2f HeadNeckJoint = new Vec2f(77, 57);
        private static readonly Vec2f BodyNeckJoint = new Vec2f(77, 74);
        //スケール
        public Vec2f Scale = new Vec2f(1,1);
        //首スケール
        public float NeckExt = 3;
        //回転
        public float NeckRotate = 0;
        public float HeadRotate = 0;
        public bool IsDongle = true;
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

        private Vec2f GetCenter()
        {
            return CheckAndInvert(IsDongle ? DangleCenter : StandCenter);
        }

        private Vec2f CheckAndInvert(Vec2f vec)
        {
            if (IsInversion())
                vec = new Vec2f(128 - vec.X, vec.Y);
            return vec;
        }

        private float GetAngle()
        {

            return IsDongle ? Target.angle + (IsInversion() ? 45 : -45) : Target.angle;
        }

        private readonly MultipleRotationCalc _headCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _neckCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _bodyCalc = new MultipleRotationCalc();
        public  void Draw()
        {
            Vec2f _neck = HeadNeckJoint - BodyNeckJoint;

            _headCalc.Init(Scale, Vec2f.ZERO).Move(GetCenter() * -1);
            _neckCalc.Init(Scale*new Vec2f(1,NeckExt), BodyNeckJoint-GetCenter()).Move(GetCenter() * -1);
            _bodyCalc.Init(Scale, Vec2f.ZERO).Move(GetCenter() * -1);

            Vec2f center = GetCenter();
            _headCalc.AddRotate(center, GetAngle());
            _neckCalc.AddRotate(center, GetAngle());
            _bodyCalc.AddRotate(center, GetAngle());
            if (IsDongle)
            {
                Vec2f head = CheckAndInvert(HeadNeckJoint);
                _neckCalc.AddRotate(head, HeadRotate);
                _bodyCalc.AddRotate(head, HeadRotate);
                Vec2f neck = CheckAndInvert(BodyNeckJoint);
                _bodyCalc.AddRotate(neck, NeckRotate);
            }
            else
            {
                Vec2f head = CheckAndInvert(HeadNeckJoint)+ _neck * (NeckExt - 1);
                _headCalc.AddRotate(head, HeadRotate);
                Vec2f neck = CheckAndInvert(BodyNeckJoint);
                _headCalc.AddRotate(neck, NeckRotate);
                _neckCalc.AddRotate(neck, NeckRotate);


                _headCalc.Move(_neck.Rotate(_headCalc.Rotate) * (NeckExt - 1));
            }

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

            Vec2f screenPos = Target.scene.GetScreenPos(Target.pos);
            Debug.DrawVec2(screenPos, "Center");
        }

        //軸と角度を指定して回転を追加可能
        private void Draw(int image, MultipleRotationCalc calc)
        {
            //表示位置
            Vec2f screenPos = Target.scene.GetScreenPos(Target.pos) + calc.Offset;
            Debug.DrawVec2(screenPos + calc.ScalePivot,"pivot");
            Console.WriteLine(calc.ScalePivot);
            DX.DrawRotaGraph3F(screenPos.X, screenPos.Y, calc.ScalePivot.X, calc.ScalePivot.Y, calc.Scale.X, calc.Scale.Y, calc.Rotate, image, 1, IsInversion() ? DX.TRUE : DX.FALSE);
        }
    }
}