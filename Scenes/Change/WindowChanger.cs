using DxLibDLL;

namespace Giraffe
{
    public class WindowChanger : SceneChanger
    {
        public delegate void GetViewArea(out int x, out int y, out int x1, out int y1,float progress);

        private readonly bool useOld, useNew;
        private readonly GetViewArea viewArea;
        public WindowChanger(int time, bool fadeOld, bool fadeNew,GetViewArea area) : base(time)
        {
            useOld = fadeOld;
            useNew = fadeNew;
            viewArea = area;
        }

        public override void PreDrawOld()
        {
            if (useOld)
            {
                viewArea(out int x, out int y, out int x1, out int y1, Progress);
                DX.SetDrawArea(x, y, x1, y1);
            }
        }
        public override void PreDrawNew()
        {
            if (useNew)
            {
                viewArea(out int x, out int y, out int x1, out int y1, Progress);
                DX.SetDrawArea(x, y, x1, y1);
            }
        }
        public override void PostDraw()
        {
            DX.SetDrawAreaFull();
        }
    }
}