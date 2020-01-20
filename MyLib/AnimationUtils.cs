using System;

namespace SAGASALib
{
    public static class AnimationUtils
    {
        public static int GetImage(int[] images, float progress,int end = -1)
        {
            progress = MyMath.Clamp(progress);
            if (end != -1)
                return images[(int)Math.Round((end - 1) * progress)];
            else
                return images[(int)Math.Round((images.Length - 1) * progress)];
        }
        public static int GetImageLoop(int[] images, float progress, int end = -1)
        {
            progress = MyMath.Clamp(progress);
            progress = 1 - (Math.Abs(progress - 0.5f) * 2);
            if (end != -1)
                return images[(int)Math.Round((end - 1) * progress)];
            else
                return images[(int)Math.Round((images.Length - 1) * progress)];
        }
    }
}
