using System;
using System.Collections.Generic;
using System.IO;
using DxLibDLL;
using SAGASALib;
using Giraffe.Saves;

namespace Giraffe
{
    public class Game
    {
        //回転テスト 邪魔なら消して
        List<StaticMapObject> objList = new List<StaticMapObject>();

        public BgmManager bgmManager = new BgmManager();
        public static bool fadeAction = true;

        public static bool ShowCollision = true;
        
        public static int currentScore = 0;//現在のスコア
        public static int[] currentTime = new int[] { 0, 0, 0 };//現在のタイム

        public Settings settings;
        private const string SETTINGS = "settings";
        public HightScore hightScore;
        private const string HIGHTSCORE = "hightscore";

        public void Init()
        {
            //設定の読み込み
            settings = SaveManager.Load<Settings>(SETTINGS);
            if (settings == null)
                settings = new Settings();
            bgmManager.playOn = settings.bgmPlayOn;
            Sound.playOn = settings.sePlayOn;
            //ハイスコアとベストタイムの読み込み
            hightScore = SaveManager.Load<HightScore>(HIGHTSCORE);
            if (hightScore == null)
                hightScore = new HightScore();

            DX.SetBackgroundColor(200, 200, 200);
            bgmManager.Init();
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

            #if DEBUG
            Debug.Draw();
            #endif
            NumberDraw.TimeDraw(DateTime.Now, 100,100, "image_result/result_num_",10,0.1f);

            //bgmManager.Debug();
        }
    }
}