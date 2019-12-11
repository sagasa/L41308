namespace SAGASALib
{
    //複数の回転を追加する計算系
    public class MultipleRotationCalc
    {
        

        public Vec2f Offset { get; private set; } = Vec2f.ZERO;
        public float Rotate { get; private set; } = 0f;

        private float scaleX;

        public void AddRotate(Vec2f off, float angle)
        {
            Offset += off;

            Rotate += angle;
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