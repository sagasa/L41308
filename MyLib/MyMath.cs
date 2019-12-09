using System;

namespace SAGASALib
{
    public static class MyMath
    {
        public const float PI = (float)Math.PI;
        public const float Deg2Rad = PI / 180f; // 度からラジアンに変換する定数

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
        

        public static float Distance(float x0,float y0,float x1,float y1)
        {
            return (float) Math.Sqrt((x0 - x1) * (x0 - x1) + (y0 - y1) * (y0 - y1));
        }
    }
}
