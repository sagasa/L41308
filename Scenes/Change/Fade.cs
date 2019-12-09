using SAGASALib;

namespace Giraffe
{
    public class Fade : SceneChanger
    {
        private readonly bool useOld, useNew;
        public Fade(int time,bool fadeOld, bool fadeNew) : base(time)
        {
            useOld = fadeOld;
            useNew = fadeNew;
        }

        public override void PreDrawNew()
        {
            if(useNew)
                DxHelper.SetAlpha((int)(Progress * 256));
        }

        public override void PreDrawOld()
        {
            if(useOld)
              DxHelper.SetAlpha((int)((1-Progress) * 256));
        }
        public override void PostDraw()
        {
            DxHelper.SetAlpha(256);
        }
    }
}