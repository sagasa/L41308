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
            pos = new Vec2f(3,3);
            velAngle = RotateSpeed/5;
        }

        public bool IsInversion()
        {
            return IsDongle ? velAngle < 0 : vel.X < 0;
        }

        public Vec2f GetCenter()
        {
            return Vec2f.ZERO;
            //    return CheckAndInvert(IsDongle ? DangleCenter : StandCenter);
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

        private Vec2f MakeRotateOffset(Vec2f pivot, float currentAngle, float addAngle)
        {
            //中心から軸へのベクトル
            Vec2f offset = CheckAndInvert(pivot);
            //移動分を計算
            Vec2f screenPos = scene.GetScreenPos(pos);
            Debug.DrawVec2(screenPos, offset.Rotate(currentAngle));
            Debug.DrawVec2(screenPos+ offset.Rotate(currentAngle), offset.Rotate(currentAngle + addAngle)*-1);
            offset = offset.Rotate(currentAngle) - offset.Rotate(currentAngle + addAngle);
            return offset;
        }

        private float animationProgress = 0;

        private uint white = DX.GetColor(200, 200, 200);

        private float count = 0;
        public override void Draw()
        {
            count ++;
            if (60 < count)
                count = -60;
            
            headRotate = MyMath.Deg2Rad*(Math.Abs(count)-30);
            neckRotate = MyMath.Deg2Rad * (Math.Abs(count) - 30)*-1;

            Vec2f _neck = HeadNeckJoint - BodyNeckJoint;

            //頭の回転 
            float _headRotate = IsDongle ? 0 : headRotate+neckRotate;
            //頭のオフセット
            Vec2f _headOffset = IsDongle ? MakeRotateOffset(DangleCenter, 0,GetAngle()) : Vec2f.ZERO;
            //首の回転
            float _neckRotate = IsDongle ? headRotate : neckRotate;
            //首のオフセット
            Vec2f _neckOffset = IsDongle ? MakeRotateOffset(DangleCenter, 0,GetAngle())+ MakeRotateOffset(HeadNeckJoint, GetAngle(), headRotate) : Vec2f.ZERO;
            //胴の回転
            float _bodyRotate = IsDongle ? headRotate+neckRotate : 0;
            //胴のオフセット
            Vec2f _bodyOffset = IsDongle ? MakeRotateOffset(DangleCenter, 0, GetAngle()) + MakeRotateOffset(HeadNeckJoint, GetAngle(), headRotate)
                + MakeRotateOffset(BodyNeckJoint, GetAngle()+headRotate, neckRotate) : Vec2f.ZERO;

            //胴
            Draw(imageBody,_bodyOffset,_bodyRotate);
            //頭
            //Draw(imageHorn);
            Draw(AnimationUtils.GetImage(imageHead,animationProgress),_headOffset,_headRotate);
            //Draw(AnimationUtils.GetImageLoop(imageEar, animationProgress));
            //Draw(AnimationUtils.GetImage(imageLeg, animationProgress));
            //Draw(AnimationUtils.GetImageLoop(imageEye, animationProgress));
            //Draw(AnimationUtils.GetImageLoop(imageTail, animationProgress));
            //首
            Draw(imageNeck,_neckOffset,_neckRotate,1);
            
            Vec2f screenPos = scene.GetScreenPos(pos);
            //Debug.DrawVec2(screenPos + _neckOffset);    
            Debug.DrawVec2(screenPos,"Center");
            DX.DrawBoxAA(X, Y, X+80, Y+80, white,DX.FALSE);
        }

        //軸と角度を指定して回転を追加可能
        private void Draw(int image,Vec2f offset = null,float angle_ = 0,float exY = 1)
        {
            if (offset == null)
                offset = Vec2f.ZERO;

            Vec2f screenPos = scene.GetScreenPos(pos);
            Debug.DrawVec2(screenPos+offset);

            DX.DrawRotaGraph3F(screenPos.X+ offset.X, screenPos.Y+offset.Y, GetCenter().X, GetCenter().Y, 1, exY, angle_ + GetAngle(), image, 1, IsInversion() ? DX.FALSE : DX.TRUE);
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

                //velAngle = MyMath.Lerp(velAngle, -RotateSpeed,0.01f);
            }
            else
            {
                //velAngle = MyMath.Lerp(velAngle, RotateSpeed, 0.01f);
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