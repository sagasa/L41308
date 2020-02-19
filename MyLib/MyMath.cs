using System;

namespace SAGASALib
{
    public static class MyMath
    {
        public const float PI = (float)Math.PI;
        public const float Deg2Rad = PI / 180f; // 度からラジアンに変換する定数
        public const float Rad2Deg = 180f / PI; // ラジアンから度に変換する定数

        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * t;
        }

        public static float LerpEX(float from, float to, float progress, float downProgress = -1)
        {
            if (Math.Abs(to) < Math.Abs(from) && 0 < downProgress)
                progress = downProgress;
            return from + (to - from) * progress;
        }
        //角度の補完
        public static float LerpAngle(float from, float to, float progress)
        {
            //-は殺す
            float sub = to - from;
            while (sub < 0)
                sub += 2 * (float)Math.PI;
            if (Math.PI < sub)
                sub -= (float)Math.PI*2;
            return from + sub * progress;
        }

        //0-1にクランプ 範囲外は0or1に
        public static float Clamp(float value)
        {
            if (value < 0)
                return 0f;
            if (1 < value)
                return 1f;
            return value;
        }

        public static float Clamp(float value,float max,float min)
        {
            if (value < min)
                return min;
            if (max < value)
                return max;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (max < value)
                return max;
            return value;
        }

        public static float Distance(float x0,float y0,float x1,float y1)
        {
            return (float) Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1));
        }

        //線分の交点
        public static Vec2f GetCrossPos(Vec2f p0, Vec2f p1, Vec2f p2, Vec2f p3)
        {
            Vec2f a = p1 - p0;
            Vec2f c = p2 - p0;
            Vec2f b = p3 - p2;

            float ab = a.Cross(b);
            float t1 = c.Cross(a) / ab;
            float t2 = c.Cross(b) / ab;

            if ((0 < t1 && t1 < 1) && (0 < t2 && t2 < 1))
            {
                return p0 +  a* t2;
            }
            else
                return Vec2f.ZERO;
        }
    }
}
