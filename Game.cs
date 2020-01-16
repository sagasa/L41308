using System;
using System.Collections.Generic;
using System.IO;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class Game
    {
        //回転テスト 邪魔なら消して
        List<StaticMapObject> objList = new List<StaticMapObject>();

        public BgmManager bgmManager = new BgmManager();

        public static bool isGoal = false;//ゴールする判定

        public static bool ShowCollision = true;

        public void Init()
        {
            DX.SetBackgroundColor(200, 200, 200);
            bgmManager.Load();
            SetScene(new Title(this));
            for (int i = 0; i < 1; i++)
            {
                //これ消せばクルクル消えます
                //objList.Add(new StaticMapObject( i, 0 ));
            }
            Vec2f vec2 = new Vec2f(1,1);
            Vec2f test = vec2 * 2;
        }

        public Scene CurrentScene { get; private set; }
        private Scene _oldScene;
        private List<SceneChanger> _changer = new List<SceneChanger>();

        public void SetScene(Scene next, params SceneChanger[] change)
        {
            if (0 < change.Length&&CurrentScene!=null)
            {
                _changer = new List<SceneChanger>(change);
                _oldScene = CurrentScene;
                CurrentScene = next;
                CurrentScene.OnLoad();
            }
            else
            {
                CurrentScene?.OnExit();
                CurrentScene = next;
                CurrentScene.OnLoad();
            }
        }

        public void Update()
        {
            Input.Update();

            CurrentScene.Update();
            //シーン切り替え関連
            if (0<_changer.Count)
            {
                _oldScene.Update();
                for (var i = _changer.Count - 1; i >= 0; i--)
                {
                    _changer[i].Update();
                    if (1 <= _changer[i].Progress)
                    {
                        _changer.RemoveAt(i);
                        if (_changer.Count == 0)
                            _oldScene.OnExit();
                    }
                }               
            }            
        }

        public void Draw()
        {
            if (0 < _changer.Count)
            {
                _changer.ForEach(change=>change.PreDrawOld());
                _oldScene.Draw();
                _changer.ForEach(change => change.PostDraw());
                _changer.ForEach(change => change.PreDrawNew());
                CurrentScene.Draw();
                _changer.ForEach(change => change.PostDraw());

            }
            else
                CurrentScene?.Draw();

            objList.ForEach(obj=>obj.Draw());

            Debug.Draw();

            //bgmManager.Debug();
        }
    }
}
