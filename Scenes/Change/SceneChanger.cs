namespace Giraffe
{
    public abstract class SceneChanger
    {
        /// <summary>
        /// 0～1で進捗表現
        /// </summary>
        public float Progress => (float)_time / _count;
        protected SceneChanger(int time)
        {
            _time = time;
        }

        private readonly int _time;
        private int _count = 0;

        public void Update()
        {
            _count++;
        }

        public abstract void PreDrawOld();
        public abstract void PreDrawNew();
        public abstract void PostDraw();
    }
}