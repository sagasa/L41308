using Giraffe;

namespace Test.Drawable
{
    //画面に表示するインターフェースの親クラス
    public abstract class GUI
    {
        protected Scene _scene;

        public GUI(Scene scene)
        {
            _scene = scene;
        }
        public abstract void Update();
        public abstract void Draw();
    }
}