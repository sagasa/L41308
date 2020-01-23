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
        private static readonly int imageNeckHead = ResourceLoader.GetGraph("player/neckcircle_1.png");
        private static readonly int imageNeckBody = ResourceLoader.GetGraph("player/neckcircle_2.png");
        private static readonly int[] imageHead = ResourceLoader.GetGraph("player/player_head.png", 3);
        private static readonly int[] imageEye = ResourceLoader.GetGraph("player/player_eye.png", 4);
        private static readonly int[] imageEar = ResourceLoader.GetGraph("player/player_ear.png", 4);
        private static readonly int[] imageLeg = ResourceLoader.GetGraph("player/player_leg.png", 6);
        private static readonly int[] imageTail = ResourceLoader.GetGraph("player/player_tail.png", 5);

        //画像のポジション
        public static readonly Vec2f ImageSize = new Vec2f(128, 128);
        private static readonly Vec2f StandCenterPos = new Vec2f(64, 64) / ImageSize;
        private static readonly Vec2f DangleCenterPos = new Vec2f(114, 50) / ImageSize;
        private static readonly Vec2f HeadNeckJointPos = new Vec2f(78, 47) / ImageSize;
        private static readonly Vec2f BodyNeckJointPos = new Vec2f(78, 90) / ImageSize;
        private static readonly Vec2f NeckCenterPos = (HeadNeckJointPos - BodyNeckJointPos) * 0.5f + BodyNeckJointPos;

        private Vec2f Center { get => CheckAndInvert(State==Player.PlayerState.Dongle ? DangleCenterPos : StandCenterPos); }
        private Vec2f HeadNeckJoint { get => CheckAndInvert(HeadNeckJointPos); }
        private Vec2f BodyNeckJoint { get => CheckAndInvert(BodyNeckJointPos); }
        private Vec2f NeckCenter {get => CheckAndInvert(NeckCenterPos); }
        //スケール
        public Vec2f Scale = new Vec2f(0.8f,0.8f);
        //首スケール
        public float NeckExt = 1;
        //回転
        public float NeckRotate = 0;
        public float HeadRotate = 0;

        private float NeckRotateFix => CheckAndInvert(NeckRotate);
        private float HeadRotateFix => CheckAndInvert(HeadRotate);
        public Player.PlayerState State = Player.PlayerState.Stand;
        //アニメーション
        public float MouthProgress = 0;
        public float EyeProgress = 0;
        public float EarProgress = 0;
        public float LegProgress = 0;
        public float TailProgress = 0;


        //表示対象
        public readonly GameObject Target;

        //Map座標で口の位置取得
        public Vec2f GetMouthOffset()
        {
            switch (State)
            {
                case Player.PlayerState.Dongle:
                    return Vec2f.ZERO;
                case Player.PlayerState.Fly:
                {
                    //テクスチャ座標系で頭の位置を算出
                    Vec2f body = BodyNeckJoint - CheckAndInvert(StandCenterPos);
                    Vec2f neck = HeadNeckJoint - BodyNeckJoint;
                    neck *= NeckExt*0.5f;
                    
                    Vec2f res = (body+neck).Rotate(GetAngle());
                    Vec2f head = CheckAndInvert(DangleCenterPos) - HeadNeckJoint;
                    head = head.Rotate(GetAngle() + HeadRotateFix);
                    res += head;
                    //変換して返す
                    return res * ImageSize * Scale / PlayMap.CellSize;
                }
                case Player.PlayerState.Stand:
                {
                    //テクスチャ座標系で頭の位置を算出
                    Vec2f body = (BodyNeckJoint - CheckAndInvert(StandCenterPos)).Rotate(GetAngle());
                    Vec2f neck = HeadNeckJoint - BodyNeckJoint;
                    neck *= NeckExt;
                    neck = neck.Rotate(GetAngle() + NeckRotateFix);
                    Vec2f head = CheckAndInvert(DangleCenterPos) - HeadNeckJoint;
                    head = head.Rotate(GetAngle() + NeckRotateFix + HeadRotateFix);
                    Vec2f res = body + neck + head;
                    //変換して返す
                    return res * ImageSize * Scale / PlayMap.CellSize;
                }
            }

            Vec2f pos = Target.pos;
            pos += (HeadNeckJoint - Center).Rotate(HeadRotate);
            return Vec2f.ZERO;
        }

        public PlayerRender(GameObject target)
        {
            Target = target;
        }


        private bool IsInversion()
        {
            return 0 == Target.velAngle ? Target.vel.X < 0 : 0 < Target.velAngle;
        }

        
        private Vec2f CheckAndInvert(Vec2f vec)
        {
            if (IsInversion())
                vec = new Vec2f(1f - vec.X, vec.Y);
            return vec;
        }

        private float CheckAndInvert(float value)
        {
            if (IsInversion())
                value = -value;
            return value;
        }

        private float GetAngle()
        {
            return Target.angle;
            //return State==Player.PlayerState.Dongle ? Target.angle + (IsInversion() ? 45*MyMath.Deg2Rad : -45 * MyMath.Deg2Rad) : Target.angle;
        }

        //矢印のリソース
        private static readonly int VectorImage = ResourceLoader.GetGraph("vector.png");

        private readonly MultipleRotationCalc _headCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _neckCalc = new MultipleRotationCalc();
        private readonly MultipleRotationCalc _bodyCalc = new MultipleRotationCalc();
        public  void Draw()
        {
            Vec2f screenPos = Target.scene.GetScreenPos(Target.pos);

            Vec2f _neck = HeadNeckJoint - BodyNeckJoint;
            _neck *= (NeckExt - 1);

            _headCalc.Clear();
            _neckCalc.Clear();
            _bodyCalc.Clear();
            //スケール
            switch (State)
            {
                case Player.PlayerState.Dongle:
                    _neckCalc.Scale(new Vec2f(1, NeckExt), HeadNeckJoint);
                    break;
                case Player.PlayerState.Fly:
                    _neckCalc.Scale(new Vec2f(1, NeckExt), NeckCenter);
                    break;
                case Player.PlayerState.Stand:
                    _neckCalc.Scale(new Vec2f(1, NeckExt), BodyNeckJoint);
                    break;
            }
            //回転
            _headCalc.Rotate(Center, GetAngle());
            _neckCalc.Rotate(Center, GetAngle());
            _bodyCalc.Rotate(Center, GetAngle());

            switch (State)
            {
                case Player.PlayerState.Dongle:
                    _neckCalc.Rotate(HeadNeckJoint, -HeadRotateFix);
                    _bodyCalc.Rotate(HeadNeckJoint, -HeadRotateFix);
                    _bodyCalc.Rotate(BodyNeckJoint, -NeckRotateFix);
                    _neck = _neck * -1;
                    _bodyCalc.Move(_neck.Rotate(GetAngle()+ -HeadRotateFix));
                    break;
                case Player.PlayerState.Fly:
                    _headCalc.Rotate(HeadNeckJoint, HeadRotateFix);
                    _headCalc.Move(_neck.Rotate(GetAngle()) * 0.5f);
                    _neck = _neck * -1;
                    _bodyCalc.Rotate(BodyNeckJoint, -NeckRotateFix);
                    _bodyCalc.Move(_neck.Rotate(GetAngle()) * 0.5f);
                    break;
                case Player.PlayerState.Stand:
                    _neckCalc.Rotate(BodyNeckJoint, NeckRotateFix);
                    _headCalc.Rotate(BodyNeckJoint, NeckRotateFix);
                    _headCalc.Rotate(HeadNeckJoint, HeadRotateFix);
                    _headCalc.Move(_neck.Rotate(GetAngle()+ NeckRotateFix));
                    break;
            }
            //表示サイズに
            Vec2f scale = new Vec2f(128, 128)* Scale;
            //*
            _headCalc.Move(Center * -1);
            _neckCalc.Move(Center * -1);
            _bodyCalc.Move(Center * -1);
            //*/
            _headCalc.Scale(scale, Vec2f.ZERO);
            _neckCalc.Scale(scale, Vec2f.ZERO);
            _bodyCalc.Scale(scale, Vec2f.ZERO);

            //首
            Draw(imageNeckBody, _bodyCalc);
            Draw(imageNeckHead, _headCalc);
            Draw(imageNeck, _neckCalc);
            //胴
            Draw(imageBody, _bodyCalc);
            //頭
            Draw(imageHorn, _headCalc);
            Draw(AnimationUtils.GetImage(imageHead, MouthProgress), _headCalc);
            Draw(AnimationUtils.GetImageLoop(imageEar, EarProgress), _headCalc);
            Draw(AnimationUtils.GetImage(imageLeg, LegProgress), _bodyCalc);
            Draw(AnimationUtils.GetImageLoop(imageEye, EyeProgress), _headCalc);
            Draw(AnimationUtils.GetImageLoop(imageTail, TailProgress), _bodyCalc);
            
            //矢印
            if (State == Player.PlayerState.Dongle)
            {
                const float deg90 = MyMath.PI / 2f;
                Vec2f vectorPos = screenPos + CheckAndInvert(new Vec2f(20, 20)).Rotate(GetAngle());
                DX.DrawRotaGraphF(vectorPos.X, vectorPos.Y, 2f, GetAngle() + (IsInversion()? -deg90: deg90), VectorImage);
            }
            
            //Debug.DrawPos(Vec2f.ZERO, screenPos, "Center");
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