using System;
using SAGASALib;
using DxLibDLL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    public class Title : Scene
    {

　　　　public int TitleCount = 0;//タイトルの状態管理の値。0がタイトル、1がステージセレクト画面です。

        
        private const int frameX = 285;
        private const int frameY = 100;
        private const int fontInterval = 30;//文字同士の幅
        private const float fontScale1 = 0.18f;//文字の大きさ
        private const float fontScale2 = 0.1f;//タイムボーナス用
        
        private int selectCursor;
        private const int selectCount = 3;
       
        private DummyPlayer Dummy;//ステージセレクト画面用ダミー
        private DummyPlayer NeckDummy;//タイトル画面用のダミー
        private const int fadeTime = 90;
        private const int shortFadeTime = 60;
        private int TreeFixedCount = 0;
        public float BackgroundFixPosition = 0;//ステージセレクト画面の背景の座標
        public float tree = 0;
        const int StageCount = 3;
        public float CursorPos = 502;

        public bool isStageSelect = false;//タイトルと選択画面の移行フラグ
        public static bool isTreeRight = false;

        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
        private int icon = ResourceLoader.GetGraph("キリンアイコン.png");
        private int treebg = ResourceLoader.GetGraph("image_select/select_bg.png");
        private int stagename = ResourceLoader.GetGraph("image_select/select_UI.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");

        void Reset()//ステージセレクト画面からタイトル画面に戻るときの処理
        {
            stagePointer = 0;
            selectCursor = 1;
            BackgroundFixPosition = 0;
            TitleCount = 0;
        }
        public void TreeFixedPos()//ステージセレクト画面の背景の処理
        {
            tree = Screen.Width * stagePointer;
            if (tree == BackgroundFixPosition || BackgroundFixPosition < 0) {}

            if (TreeFixedCount % 1 == 0 && isTreeRight)//⇨押したら右移動
            {
                if (tree == BackgroundFixPosition) { }
                else
                {
                    BackgroundFixPosition += Screen.Width / 80;
                }
            }
            else if (TreeFixedCount % 1 == 0 && !isTreeRight)//⇦押したら左移動
            {
                if (tree == BackgroundFixPosition || BackgroundFixPosition < 0) { }
                else
                {
                    BackgroundFixPosition -= Screen.Width / 80;
                }
            }
            else if (tree >= BackgroundFixPosition && Input.RIGHT.IsPush() || Input.LEFT.IsPush()) {}
        }
        public void selectPos()//タイトル画面での状態の処理
        {
            if (Input.UP.IsPush() && selectCursor > 1)//カーソルが一番上以外の時に↑が押されたら、カーソルを一つ上へ
            {
                Sound.Play("cursor_SE.mp3");
                selectCursor -= 1;
            }
            else if (Input.DOWN.IsPush() && selectCursor < 3)//カーソルが一番下以外の時下を押されたら、カーソルが一つ下へ
            {
                Sound.Play("cursor_SE.mp3");
                selectCursor += 1;
            }
        }
        public void DummyDirection()//ダミーの方向転換
        {
            if (Input.RIGHT.IsPush())
            {
                isTreeRight = true;
                Dummy.isDunnyRight = true;
            }
            else if (Input.LEFT.IsPush())
            {
                isTreeRight = false;
                Dummy.isDunnyRight = false;
            }

            if (Dummy.isDunnyRight)
            {
                Dummy.vel = Dummy.vel.SetX(MyMath.Lerp(Dummy.vel.X, 0.01f, 0.1f));
            }
            else if (!Dummy.isDunnyRight)
            {
                Dummy.vel = Dummy.vel.SetX(MyMath.Lerp(Dummy.vel.X, -0.01f, 0.1f));
            }
        }

        public int stagePointer = 0;//ステージのかうんと」てきな


        public Title(Game game,int TitleStage) : base(game)
        {
            Dummy = new DummyPlayer(this);
            NeckDummy = new DummyPlayer(this);
            NeckDummy.Render.NeckExt = 7.5f;
            TitleCount =TitleStage;
        }
        
        public override void Draw()
        {
            if (TitleCount==0)//タイトル画面
            {
                DX.DrawGraph(0, 0, titlebg);//背景
                DX.DrawGraph(135, 450, ResourceLoader.GetGraph("select_" + 0 + ".png"));//ステージセレクト画面の画像
                DX.DrawGraph(90, 550, ResourceLoader.GetGraph("select_" + 1 + ".png"));//画面説明画面の画像
                DX.DrawGraph(135, 650, ResourceLoader.GetGraph("select_" + 2 + ".png"));//設定画面の画像
                NeckDummy.Draw();
            }
            if (TitleCount==1)//ステージセレクト
            {
                TreeFixedPos();
                DX.DrawGraphF(0 - BackgroundFixPosition, 0, treebg);
                DX.DrawGraphF(0 - BackgroundFixPosition, 0, stagename);
                Dummy.Draw();
            }
            DX.DrawString(500, 50, "" + stagePointer, DX.GetColor(0, 0, 0));//確認用
            DX.DrawString(500, 100, "" + selectCursor, DX.GetColor(0, 0, 0));//確認用
            DX.DrawString(500, 150, "" + BackgroundFixPosition, DX.GetColor(0, 0, 0));//確認用
            DX.DrawString(500, 200, "" + tree, DX.GetColor(0, 0, 0));//確認用
            DX.DrawString(500, 250, "" + Dummy.Render.NeckExt, DX.GetColor(0, 0, 0));
        }

        public override void OnLoad()
        {
            selectCursor = 1;
            Dummy.pos = new Vec2f(Screen.Width / 2, Screen.Height - 64);
            NeckDummy.pos = new Vec2f(Screen.Width / 8, Screen.Height - 64);
        }

        public override void Update()
        {
            if (!Game.fadeAction)
            {
                if (TitleCount==0)//タイトル画面
                {
                    selectPos();
                    NeckDummy.Update();
                    NeckDummy.isDummyNeck = true;
                    if (Input.ACTION.IsPush())//タイトル画面で決定ボタンが押されたら
                    {
                        Sound.Play("decision_SE.mp3");
                        if (selectCursor == 1)//ステージセレクト画面へ
                        {
                            TitleCount = 1;
                            NeckDummy.isDummyNeck = false;
                        }
                        else if (selectCursor == 2)//画面説明へ
                        {
                            Game.fadeAction = true;
                            Game.bgmManager.Set(shortFadeTime, "tutorial", "title");
                            Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.CrossFade);
                            Game.SetScene(new Tutolal(Game, new PlayMap("map_0"), "_0"), new Fade(shortFadeTime, true, true));
                            Tutolal.Tutorialcount += 1;
                        }
                        else if (selectCursor == 3)//オプション画面へ
                        {
                            Game.fadeAction = true;
                            Game.SetScene(new SceneOption(Game), new Fade(shortFadeTime, true, true));
                        }
                    }
                }
                else if (TitleCount==1)//ステージセレクト
                {
                    Dummy.Update();
                    DummyDirection();
                    if (Input.RIGHT.IsPush() && stagePointer < 3)//一番右側以外にいるとき、→が押されたら右へ
                    {
                        if (!Sound.CheckPlaySound("step_SE.mp3"))
                            Sound.Loop("step_SE.mp3");
                        stagePointer += 1;
                    }
                    else if (Input.LEFT.IsPush() && stagePointer > 0)//一番左側以外にいるとき、←が押されたら左へ
                    {
                        if (!Sound.CheckPlaySound("step_SE.mp3"))
                            Sound.Loop("step_SE.mp3");
                        stagePointer -= 1;
                    }
                    //画面の位置がステージの間にあるならtreeMoveをtrue
                    if (Input.ACTION.IsPush() && BackgroundFixPosition == tree)
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.fadeAction = true;
                        //固有のもの
                        if (stagePointer == 0)//チュートリアル
                        {
                            Tutolal.Tutorialcount += 99;
                            Game.bgmManager.Set(fadeTime, "tutorial", "title");
                            Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.CrossFade);
                            Game.SetScene(new Tutolal(Game, new PlayMap("map_0"), "_0"), new Fade(fadeTime, true, true));
                        }
                        else//プレイシーン
                        {
                            Game.bgmManager.Set(fadeTime, "play", "title");
                            Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.CrossFade);
                            Game.SetScene(new ScenePlay(Game, new PlayMap("map_" + stagePointer), "_" + stagePointer, stagePointer), new Fade(fadeTime, true, true));
                        }
                    }
                    if (Input.BACK.IsPush())//Xを押したらタイトル画面へ
                    {
                        Reset();
                        Sound.Play("cancel_SE.mp3");
                    }
                }

                if (BackgroundFixPosition<tree|| Sound.CheckPlaySound("step_SE.mp3"))
                {
                    Sound.Stop("step_SE.mp3");
                }
            }
        }

        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}