using System;
using Giraffe;

namespace SAGASALib
{
    //アニメーションの登録
    public class AnimationEntry<T>
    {
        //=== デリゲート ===
        public delegate void AnimationAction(T render, Animation<T> animation);

        //=== 変数 ===
        //注入した処理
        public readonly AnimationAction Update;
        public readonly AnimationAction Init;

        //実行時間 フレーム数
        public readonly int Life;

        //ループするか
        public readonly bool Loop;

        //=== 関数 ===

        public AnimationEntry(int time, AnimationAction update, AnimationAction init,bool loop = false)
        {
            Update = update;
            Init = init;
            Loop = loop;
            Life = time;
        }
    }
}