using System;
using Giraffe;

namespace Giraffe
{
    /**アニメーション*/
    public class PlayerAnimation
    {
        //注入した処理
        private readonly AnimationUpdate _update;

        //実行時間 フレーム数
        private readonly int _life;

        //アップデート毎の経過量 
        private readonly float _delta;

        //0からインクリメント
        private float _time;

        //実行対象
        private PlayerRender _render;

        public PlayerAnimation(int time, AnimationUpdate update)
        {
            _update = update;
            _life = time;
            _delta = 1f / time;
        }

        public void Init(PlayerRender render)
        {
            _render = render;
            _time = 0;
        }

        public void Update()
        {
            _update(_render, _time / _life, _delta);
            _time++;
        }

        public bool IsEnd() => _life <= _time;

        public delegate void AnimationUpdate(PlayerRender render, float progress, float delta);

        //============ アニメーション ============
        public static readonly PlayerAnimation MouthOpen = new PlayerAnimation(30, (render, progress, delta) =>
        {
            if (progress == 0)
                render.MouthProgress = 0;
            render.MouthProgress += delta;
            Console.WriteLine(progress + " " + delta+" "+render.MouthProgress);
        });
    }
}