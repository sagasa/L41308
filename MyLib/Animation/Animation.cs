namespace SAGASALib
{
    //アニメーションの再生時のインスタンス
    public class Animation<T>
    {
        //=== 変数 ===
        public readonly AnimationEntry<T> Entry;
        //0-1の範囲の進捗
        public float Progress { get =>Time / Entry.Life; set => Time = Entry.Life*Progress;}
        //アップデート毎の経過量 アップデートの合計が1
        public float Delta { get; private set;}
        public AnimationEntry<T> NextAnimation = null;
        public float Time { get; private set; } = 0;

        //=== 関数 ===
        public Animation(AnimationEntry<T> entry) => Entry = entry;
        

        public void Init(T target)
        {
            Delta = 1f / Entry.Life;
            Entry.Init?.Invoke(target, this);
        }

        public void Update(T target)
        {
            Entry.Update(target, this);
            Time++;
            //ループ
            if (IsEnd() && Entry.Loop)
                Time = 0;
        }

        public bool IsEnd() => 1f<=Progress;

        //処理を1アップデートで終わらせる
        public void Finish(T target)
        {
            while (Progress<1)
            {
                Entry.Update(target, this);
                Time++;
            }
        }
    }
}