using System;

namespace SAGASALib
{
    public static class AnimationUtils
    {
        public static int GetImage(int[] images, float progress,int end = -1)
        {
            progress = MyMath.Clamp(progress);
            if (end != -1)
                return images[(int)((end - 1) * progress)];
            else
                return images[(int)((images.Length - 1) * progress)];
        }
        public static int GetImageLoop(int[] images, float progress, int end = -1)
        {
            progress = Math.Abs(progress - 0.5f) * 2;
            progress = progress % 1;
            if (end != -1)
                return images[(int)((end-1) * progress)];
            else
                return images[(int)((images.Length - 1) * progress)];
        }
    }
}
