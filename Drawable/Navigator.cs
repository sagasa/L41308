using System;
using System.Collections.Generic;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class Navigator
    {
        private ScenePlay _scene;

        private static readonly float _space = 20;

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

        private static readonly Vec2f offset = new Vec2f(_space, _space);

        private static readonly Vec2f upperOffset = new Vec2f(0, 35);


        public void Draw()
        {
            _list.ForEach(entry =>
            {
                

                //画面外なら
                if (!_scene.IsInScreen(entry.Item1))
                {
                    Vec2f center = Screen.Size / 2;
                    Vec2f target = _scene.GetScreenPos(entry.Item1);

                    
                    Vec2f pos0 = offset+ upperOffset;
                    Vec2f pos1 = Screen.Size.SetY(0) + offset * new Vec2f(-1, 1)+ upperOffset;
                    Vec2f pos2 = Screen.Size + offset * new Vec2f(-1, -1);
                    Vec2f pos3 = Screen.Size.SetX(0) + offset * new Vec2f(1, -1);


                    Vec2f vec = (center-target)* new Vec2f(-1, 1);
                    float angle =(float) Math.Atan2(vec.X, vec.Y);
                    //描画
                    void CheckAndDraw(Vec2f start, Vec2f end)
                    {
                        Vec2f res = MyMath.GetCrossPos(center, target, start, end);
                        if (res != null)
                        {
                            DX.DrawRotaGraphF(res.X, res.Y, 0.8f, angle, entry.Item2);
                        }
                            
                    }

                    CheckAndDraw(pos0,pos1);
                    CheckAndDraw(pos1, pos2);
                    CheckAndDraw(pos2, pos3);
                    CheckAndDraw(pos3, pos0);
                }
                




        //        Debug.DrawLine(_scene.GetScreenPos(center), _scene.GetScreenPos(entry.Item1));
         //       Debug.DrawLine(_scene.GetScreenPos(pos0), _scene.GetScreenPos(pos1));
          //      Debug.DrawPos(Vec2f.ZERO, _scene.GetScreenPos(entry.Item1));

             
            });
        }
    }
}