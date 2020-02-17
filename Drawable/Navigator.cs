using System;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class Navigator
    {
        private ScenePlay _scene;

        private float _space = 10;

        private readonly List<Tuple<Vec2f, int>> _list = new List<Tuple<Vec2f, int>>();

        public Navigator(ScenePlay scene)
        {
            _scene = scene;
        }

        public void Update()
        {
            _list.Clear();
        }

        //map座標系で追加
            public void AddTarget(Vec2f vec,int image)
        {
            _list.Add(new Tuple<Vec2f, int>(vec,image));
        }

        Vec2f offset = new Vec2f(10f,10);

        public void Draw()
        {
            _list.ForEach(entry =>
            {
                //画面外なら
                if (!_scene.IsInScreen(entry.Item1))
                {
                   
                }
                Vec2f center = Screen.Size/2;
                Vec2f target = _scene.GetScreenPos(entry.Item1);
              
                Vec2f pos0 = offset;
                Vec2f pos1 = Screen.Size.SetY(0) + offset*new Vec2f(-1,1);
                Vec2f pos2;
                Vec2f pos3;

                Vec2f res = MyMath.GetCrossPos(center, target, pos0,pos1);



        //        Debug.DrawLine(_scene.GetScreenPos(center), _scene.GetScreenPos(entry.Item1));
         //       Debug.DrawLine(_scene.GetScreenPos(pos0), _scene.GetScreenPos(pos1));
          //      Debug.DrawPos(Vec2f.ZERO, _scene.GetScreenPos(entry.Item1));

                if (res != null)
                {
                    Debug.DrawPos(Vec2f.ZERO, res);
                }
            });
        }
    }
}