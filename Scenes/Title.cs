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
        private const int frameX = 285;
        private const int frameY = 100;
        private const int fontInterval = 30;//文字同士の幅
        private const float fontScale1 = 0.18f;//文字の大きさ
        private const float fontScale2 = 0.1f;//タイムボーナス用

        private int selectCursor;
        private const int selectCount = 3;

        private DummyPlayer Dummy;
        private DummyPlayer NeckDummy;
        private const int fadeTime = 90;
        private const int shortFadeTime = 60;
        private int fadeCounter = 0;
        private int stageWaitTime = 60;
        private int stageCounter = 0;
        public float BackgroundFixPosition = 0;
        public float tree = 0;
        const int StageCount = 3;
        public float CursorPos = 502;
        
        public static bool isStageSelect = false;//タイトルと選択画面の移行フラグ
        public static bool isTreeRight =false;

        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
        private int icon = ResourceLoader.GetGraph("キリンアイコン.png");
        private int treebg = ResourceLoader.GetGraph("image_select/select_bg.png");
        private int stagename=ResourceLoader.GetGraph("image_select/select_UI.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");

        void Reset()
        {
            stagePointer = 0;
            selectCursor = 1;
            stageWaitTime = 60;
            BackgroundFixPosition = 0;

        }
        public void TreeFixedPos()
        {
            stageCounter++;
            tree = Screen.Width * stagePointer;
            if (stageCounter % 1 == 0 && isTreeRight)
            {
                if (tree == BackgroundFixPosition)
                {

                }
                else
                {
                    BackgroundFixPosition += Screen.Width / 80;
                }
            }
            else if(stageCounter%1==0&&!isTreeRight)
            {
                if (tree == BackgroundFixPosition||BackgroundFixPosition<0)
                {
                  
                }
                else
                {
                    BackgroundFixPosition -= Screen.Width / 80;
                }
            }
            else if(tree>=BackgroundFixPosition&&Input.RIGHT.IsPush()||Input.LEFT.IsPush())
            {
                
            }
        }
       public void selectPos()
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
            DX.DrawGraphF(0 + CursorPos, 0, icon);
            
        }

        public int stagePointer=0;//ステージのかうんと」てきな
        
        
        public Title(Game game) : base(game)
        {
            Dummy = new DummyPlayer(this);
            NeckDummy = new DummyPlayer(this);
        }

        public override void Draw()
        {
            if (!isStageSelect)//タイトル画面
            {
                
                DX.DrawGraph(0, 0, titlebg);
                DX.DrawGraph(135,450, ResourceLoader.GetGraph("select_" + 0 + ".png"));
                DX.DrawGraph(90, 550, ResourceLoader.GetGraph("select_" + 1 + ".png"));
                DX.DrawGraph(135, 650, ResourceLoader.GetGraph("select_" + 2 + ".png"));
                NeckDummy.Draw();
            }
            if (isStageSelect)//ステージセレクト
            {
                TreeFixedPos();
                DX.DrawGraphF(0-BackgroundFixPosition,0, treebg);
                DX.DrawGraphF(0-BackgroundFixPosition,0, stagename);
                Dummy.Draw();
            }
            DX.DrawString(500, 50, "" + stagePointer, DX.GetColor(0, 0, 0));
            DX.DrawString(500, 100, "" + selectCursor, DX.GetColor(0, 0, 0));
            DX.DrawString(500, 150, "" + BackgroundFixPosition, DX.GetColor(0, 0, 0));
            DX.DrawString(500, 200, "" + tree, DX.GetColor(0, 0, 0));
        }
        
        public override void OnLoad()
        {
            selectCursor =1;
            Dummy.pos = new Vec2f(Screen.Width / 2, Screen.Height - 64);
            NeckDummy.pos = new Vec2f(Screen.Width/8, Screen.Height-64);
            fadeCounter = 0;
        }

        public override void Update()
        {
            Dummy.isDummyNeck = true;
            fadeCounter++;
            if (fadeCounter < fadeTime + 10)//BGMのフェード
            {
                if (Game.bgmManager.currentScene == "title") { }//何もしない
                else if (Game.bgmManager.currentScene == "none")
                {
                    Game.bgmManager.FadeIn("title", fadeTime);
                    if (fadeCounter == fadeTime)
                        Game.fadeAction = false;
                }
                else
                    Game.bgmManager.CrossFade("title", fadeTime);
            }
            
            if (!Game.fadeAction)
            {
                if (!isStageSelect)//タイトル画面
                {
                    selectPos();
                    NeckDummy.Update();
                    if (Input.ACTION.IsPush() && selectCursor == 1)
                    {
                        Sound.Play("decision_SE.mp3");
                        isStageSelect = true;
                        Dummy.isDummyNeck = false;
                    }
                    else if (Input.ACTION.IsPush() && selectCursor == 2)
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.fadeAction = true;
                        Game.SetScene(new Tutolal(Game, new PlayMap("map_0"), "_0"), new Fade(shortFadeTime, true, true));
                        Tutolal.Tutorialcount += 1;
                    }
                    else if (Input.ACTION.IsPush() && selectCursor == 3)
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.fadeAction = true;
                        Game.SetScene(new SceneOption(Game), new Fade(shortFadeTime, true, true));
                    }
                }
                else if (isStageSelect)//ステージセレクト
                {
                   
                    stageWaitTime--;
                    Dummy.Update();

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

                    if (!isStageSelect)
                        Sound.Stop("step_SE.mp3");

                    if (Input.ACTION.IsPush() && stageWaitTime <= 0 && Tutolal.Tutorialcount == 0)
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.fadeAction = true;
                        Game.bgmManager.currentScene = "title";

                        //固有のもの
                        if (stagePointer == 0)//チュートリアル
                        {
                            Tutolal.Tutorialcount += 99;
                            Game.SetScene(new Tutolal(Game, new PlayMap("map_0"), "_0"), new Fade(fadeTime, true, true));
                        }
                        else if (stagePointer == 1)//ステージ1
                        {
                            Game.SetScene(new ScenePlay(Game, new PlayMap("map_1"), "_1"), new Fade(fadeTime, true, true));
                        }
                        else if (stagePointer == 2)
                        {
                            Game.SetScene(new ScenePlay(Game, new PlayMap("map_2"), "_2"), new Fade(fadeTime, true, true));
                        }
                        else if (stagePointer == 3)
                        {
                            Game.SetScene(new ScenePlay(Game, new PlayMap("map_3"), "_3"), new Fade(fadeTime, true, true));
                        }
                    }
                    if (Input.BACK.IsPush())
                    {
                        isStageSelect = false;
                        Reset();
                        Sound.Play("cancel_SE.mp3");
                    }
                }
            }
        }
        
        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}