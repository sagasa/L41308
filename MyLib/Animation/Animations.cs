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

                if (animation.Progress < 0.5f )
                {
                    render.HeadRotate += MyMath.Deg2Rad * 2;
                    render.NeckRotate -= MyMath.Deg2Rad * 2;
                }
                else
                {
                    render.HeadRotate -= MyMath.Deg2Rad * 2;
                    render.NeckRotate += MyMath.Deg2Rad * 2;
                }

                // Console.WriteLine(render.TailProgress);
                if (1f < render.LegProgress)
                    render.LegProgress = 0;
                if (1f < render.TailProgress)
                    render.TailProgress = 0;
            },null,true);

        
        //首と頭の角度を0に
        public static readonly AnimationEntry<PlayerRender> DefaultAngle = SetRotate(0,0,30);

        public static readonly AnimationEntry<PlayerRender> DongleAngle = SetRotate(MyMath.Deg2Rad * 50, MyMath.Deg2Rad * -50, 30);

        public static readonly AnimationEntry<PlayerRender> StandAngle = SetRotate(MyMath.Deg2Rad * 30, MyMath.Deg2Rad * -30, 15);

        private static AnimationEntry<PlayerRender> SetRotate(float neck, float head, int time)=> new AnimationEntry<PlayerRender>(time,
        (render, animation) =>
        {
            render.HeadRotate += (head - render.HeadRotate) / (animation.Entry.Life - animation.Time);
            render.NeckRotate += (neck - render.NeckRotate) / (animation.Entry.Life - animation.Time);
        }, null);

    }
}