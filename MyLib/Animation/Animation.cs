namespace SAGASALib
{
    //アニメーションの再生時のインスタンス
    public class Animation<T>
    {
        private AnimationEntry<T> _entry;

        public void Init(T target)
        {
            _entry.Init(target);
        }

        public void Update(T target)
        {
           
        }
    }
}