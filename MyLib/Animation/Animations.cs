using System;
using Giraffe;

namespace SAGASALib
{
    public static class Animations
    {
        //=== アニメーション ===
        //口を開ける 10フレーム
        public static readonly AnimationEntry<PlayerRender> MouthOpen = new AnimationEntry<PlayerRender>(10,
            (render, animation) => { render.MouthProgress += animation.Delta; },
            (render, animation) => { animation.Progress = render.MouthProgress; });
        //口を閉じる 10フレーム
        public static readonly AnimationEntry<PlayerRender> MouthClose = new AnimationEntry<PlayerRender>(10,
            (render, animation) => { render.MouthProgress -= animation.Delta; },
            (render, animation) => { animation.Progress = 1f - render.MouthProgress; });
        //ループ前提 x軸速度に応じて足をうごかす
        public static readonly AnimationEntry<PlayerRender> WalkGround = new AnimationEntry<PlayerRender>(60,
            (render, animation) =>
            {
                //歩く速度定数
                const int speed = 2;
                render.LegProgress += Math.Abs(render.Target.vel.X * speed);
                render.TailProgress += Math.Abs(animation.Delta);

                if (animation.Progress < 0.5f)
                {
                    render.HeadRotate += MyMath.Deg2Rad * 3 * animation.DeltaScale;
                    render.NeckRotate -= MyMath.Deg2Rad * 3 * animation.DeltaScale;
                }
                else
                {
                    render.HeadRotate -= MyMath.Deg2Rad * 3 * animation.DeltaScale;
                    render.NeckRotate += MyMath.Deg2Rad * 3 * animation.DeltaScale;
                }

                // Console.WriteLine(render.TailProgress);
                if (1f < render.LegProgress)
                    render.LegProgress = 0;
                if (1f < render.TailProgress)
                    render.TailProgress = 0;
            },null,true);
    }
}