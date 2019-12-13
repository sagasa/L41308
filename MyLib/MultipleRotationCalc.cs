namespace SAGASALib
{
    //複数の回転を追加する計算系
    public class MultipleRotationCalc
    {

        public Vec2f Scale { get; private set; } = Vec2f.ZERO;
        public Vec2f ScalePivot { get; private set; } = Vec2f.ZERO;
        public Vec2f Offset { get; private set; } = Vec2f.ZERO;
        public float Rotate { get; private set; } = 0f;

        //スケール等を設定して初期化
        public MultipleRotationCalc Init(Vec2f scale,Vec2f scalePivot)
        {
            Scale = scale;
            ScalePivot = scalePivot;
            Offset = scalePivot;
            Rotate = 0;
            return this;
        }

        public MultipleRotationCalc Move(Vec2f move)
        {
            Offset += move*Scale;
            return this;
        }

        public MultipleRotationCalc AddRotate(Vec2f pivot, float angle)
        {
            pivot = pivot - ScalePivot;
            pivot *= Scale;
            //移動分を計算
            pivot = pivot.Rotate(Rotate) - pivot.Rotate(Rotate + angle);
            Offset += pivot;
            Rotate += angle;
            return this;
        }
        

        public static Vec2f MakeRotateOffset(Vec2f pivot, float currentAngle, float addAngle)
        {
            //中心から軸へのベクトル
            //移動分を計算
            pivot = pivot.Rotate(currentAngle) - pivot.Rotate(currentAngle + addAngle);
            return pivot;
        }
    }
}