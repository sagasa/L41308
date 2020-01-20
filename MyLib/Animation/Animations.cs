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
        public static readonly AnimationEntry<PlayerRender> WalkGround = new AnimationEntry<PlayerRender>(5,
            (render, animation) =>
            {
                //歩く速度定数
                const int speed = 10;
                render.LegProgress += Math.Abs(animation.Delta * render.Target.vel.X * speed);

                render.TailProgress += Math.Abs(animation.Delta * render.Target.vel.X * 0.2f);
                // Console.WriteLine(render.TailProgress);
                if (1f < render.LegProgress)
                    render.LegProgress = 0;
                if (1f < render.TailProgress)
                    render.TailProgress = 0;
            },null,true);
    }
}