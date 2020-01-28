using System;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class Navigator
    {
        private ScenePlay _scene;
        private readonly List<Tuple<Vec2f, int>> _list = new List<Tuple<Vec2f, int>>();

        public Navigator(ScenePlay scene)
        {
            _scene = scene;
        }

        public void AddTarget(Vec2f vec,int image)
        {
            _list.Add(new Tuple<Vec2f, int>(vec,image));
        }

        public void Draw()
        {
            _list.ForEach(entry =>
            {
                //画面外なら
                if (_scene.IsInScreen(entry.Item1))
                {
                    
                }
            });
        }
    }
}