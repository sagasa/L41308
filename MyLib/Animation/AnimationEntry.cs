namespace SAGASALib
{
    //アニメーションの登録
    public class AnimationEntry<T>
    {
        //=== デリゲート ===
        //アップデート処理
        public delegate void AnimationUpdate(T render, float progress, float delta);
        //アップデート処理
        public delegate void AnimationInit(T render);

        //=== 変数 ===
        //注入した処理
        public readonly AnimationUpdate Update;
        public readonly AnimationInit Init;

        //実行時間 フレーム数
        public readonly int Life;

        //アップデート毎の経過量 
        public readonly float Delta;

        //ループするか

        //=== 関数 ===

        public AnimationEntry(int time, AnimationUpdate update)
        {
            Update = update;
            Life = time;
            Delta = 1f / time;
        }
    }
}