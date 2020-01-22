namespace SAGASALib
{
    //アニメーションの再生時のインスタンス
    public class Animation<T>
    {
        //=== 変数 ===
        public readonly AnimationEntry<T> Entry;
        //0-1の範囲の進捗
        public float Progress { get =>_time / Entry.Life; set => _time = Entry.Life*Progress;}
        //アップデート毎の経過量 アップデートの合計が1
        public float Delta { get; private set;}
        //アップデート毎の経過量 アップデート１回が1
        public float DeltaScale { get; private set; }
        private float _time = 0;

        //=== 関数 ===
        public Animation(AnimationEntry<T> entry) => Entry = entry;
        

        public void Init(T target)
        {
            Delta = 1f / Entry.Life;
            DeltaScale = 1f;
            Entry.Init?.Invoke(target, this);
        }

        public void Update(T target)
        {
            Entry.Update(target, this);
            _time++;
            //ループ
            if (IsEnd() && Entry.Loop)
                _time = 0;
        }

        public bool IsEnd() => 1f<=Progress;

        //処理を1アップデートで終わらせる
        public void Finish(T target)
        {
            Delta = 1f - Progress;
            DeltaScale = Entry.Life-_time;
            Entry.Update(target, this);
            Progress = 1f;
        }
    }
}