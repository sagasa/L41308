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
        private DummyPlayer Dummy;
        public List<GameObject> gameObjects = new List<GameObject>();
        private bool fadeAction = false;
        private int fadeTime = 120;
        private int fadeCounter = 0;
        private int stageCount = 0;
        private int stagewaittime = 60;
        private bool isRight = false;
        private bool isLeft = false;
        private int treebgPos = 0;
        private int[] treeFixedPos = new int[] { Screen.Width, 0, -Screen.Width, -Screen.Width * 2 };
        private const int treebgMoveWidth = Screen.Width / 80;
        private bool treeMove = false;
        private int UIpos = 0;
        private int cursorPos = 502;
        private int[] cursorFixedPos = new int[] { 502, 617 };
        

        private int head = ResourceLoader.GetGraph("player/player_head.png");
        private int horn = ResourceLoader.GetGraph("player/horn.png");
        private int eye = ResourceLoader.GetGraph("player/player_eye.png");
        private int ear = ResourceLoader.GetGraph("player/player_ear.png");
        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
        private int select1 = ResourceLoader.GetGraph("select_1.png");
        private int select2 = ResourceLoader.GetGraph("select_2.png");
        private int icon = ResourceLoader.GetGraph("キリンアイコン.png");
        private int treebg = ResourceLoader.GetGraph("image_select/select_bg.png");
        private int stagename=ResourceLoader.GetGraph("image_select/select_UI.png");
        
        public Title(Game game) : base(game)
        {
            Dummy = new DummyPlayer(this);
            Dummy.pos = new Vec2f(Screen.Width / 2, Screen.Height-64);
        }

        public override void Draw()
        {
            if (stageCount == 0)
            {
                DX.DrawGraph(0, 0, titlebg);
                DX.DrawGraph(135, 515, select1);
                DX.DrawGraph(135, 630, select2);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, head);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, horn);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, eye);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, ear);
            }
            if (stageCount == 1)
            {
                DX.DrawGraph(treebgPos - Screen.Width, 0, treebg);
                DX.DrawGraph(UIpos - Screen.Width, 0, stagename);
                gameObjects.ForEach(obj => obj.Draw());
                Dummy.Draw();
            }
        }
        
        public override void OnLoad()
        {
            fadeCounter = 0;
            fadeAction = true;
        }

        public override void Update()
        {
            fadeCounter++;
            if (fadeCounter < fadeTime + 10)//BGMのフェード
            {
                if (Game.bgmManager.currentScene != "none")
                {
                    Game.bgmManager.CrossFade("title", fadeTime);
                }
                else
                {
                    Game.bgmManager.FadeIn("title", fadeTime);
                }
            }
            if (fadeCounter == fadeTime)
            {
                fadeAction = false;
            }

            if (stageCount == 0)
            {
                if (cursorPos != cursorFixedPos[0] && Input.UP.IsPush())//カーソルが一番上以外の時に↑が押されたら、カーソルを一つ上へ
                {
                    Sound.Play("cursor_SE.mp3");
                    for (int i = 0; i < cursorFixedPos.Length; i++)
                    {
                        if (cursorPos == cursorFixedPos[i])
                        {
                            cursorPos = cursorFixedPos[i - 1];
                            break;
                        }
                    }
                }
                else if (cursorPos != cursorFixedPos[cursorFixedPos.Length - 1] && Input.DOWN.IsPush())//カーソルが一番下以外の時下を押されたら、カーソルが一つ下へ
                {
                    Sound.Play("cursor_SE.mp3");
                    for (int i = 0; i < cursorFixedPos.Length; i++)
                    {
                        if (cursorPos == cursorFixedPos[i])
                        {
                            cursorPos = cursorFixedPos[i + 1];
                            break;
                        }
                    }
                }
                if (cursorPos == cursorFixedPos[0] && Input.ACTION.IsPush() && !fadeAction)
                {
                    Sound.Play("decision_SE.mp3");
                    stageCount = 1;
                }
                else if (cursorPos == cursorFixedPos[1] && Input.ACTION.IsPush() && !fadeAction)
                {

                    Sound.Play("decision_SE.mp3");
                    Game.bgmManager.currentScene = "title";
                    fadeAction = true;
                    Game.SetScene(new Tutolal(Game), new Fade(60, true, true));
                }
            }
            if (stageCount == 1)
            {
                Dummy.Update();
                
                gameObjects.ForEach(obj => obj.Draw());
                gameObjects.ForEach(obj => obj.Update());
                if (Input.RIGHT.IsPush())
                {
                    isRight = true;
                    isLeft = false;
                }
                else if (Input.LEFT.IsPush())
                {
                    isLeft = true;
                    isRight = false;
                }

                for (int i = 0; i < treeFixedPos.Length; i++)
                {
                    if (Input.RIGHT.IsPush())//一番右側以外にいるとき、→が押されたら右へ
                    {
                        if (treebgPos == treeFixedPos[treeFixedPos.Length - 1])
                            break;
                        else if (treebgPos == treeFixedPos[i])
                        {
                            treebgPos -= treebgMoveWidth;
                            UIpos -= treebgMoveWidth;
                            break;
                        }
                    }
                    else if (Input.LEFT.IsPush())//一番左側以外にいるとき、←が押されたら左へ
                    {
                        if (treebgPos == treeFixedPos[0])
                            break;
                        else if (treebgPos == treeFixedPos[i])
                        {
                            treebgPos += treebgMoveWidth;
                            UIpos += treebgMoveWidth;
                            break;
                        }
                    }
                }
                //画面の位置がステージの間にあるならtreeMoveをtrue
                for (int i = 0; i < treeFixedPos.Length; i++)
                {
                    if (treebgPos == treeFixedPos[i])
                    {
                        treeMove = false;
                        break;
                    }
                    else if (i == treeFixedPos.Length - 1)
                    {
                        treeMove = true;
                    }
                }
                if (treeMove)
                {
                    if (isRight)
                    {
                        treebgPos -= treebgMoveWidth;
                        UIpos -= treebgMoveWidth;
                        //idouCounter++;
                        //if (idouCounter % 1 == 0) { }
                    }
                    if (!isRight)
                    {
                        treebgPos += treebgMoveWidth;
                        UIpos += treebgMoveWidth;
                        //idouCounter++;
                        //if (idouCounter % 1 == 0) { }
                    }
                }
                
                if (Input.BACK.IsPush())
                {
                    stageCount = 0;
                    Sound.Play("cancel_SE.mp3");
                    stagewaittime = 60;
                }

                stagewaittime--;
                if (!fadeAction && Input.ACTION.IsPush() && stagewaittime <= 0 && Tutolal.Tutorialcount == 0)
                {
                    if (treebgPos == treeFixedPos[0])
                    {
                        Sound.Play("decision_SE.mp3");
                        fadeAction = true;
                        Game.bgmManager.currentScene = "title";
                        Game.SetScene(new Tutolal(Game), new Fade(fadeTime, true, true));
                        Tutolal.Tutorialcount = 0;
                    }
                    else if (treebgPos == treeFixedPos[1])
                    {
                        Sound.Play("decision_SE.mp3");
                        fadeAction = true;
                        Game.bgmManager.currentScene = "title";
                        Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                    }
                    else if (treebgPos == treeFixedPos[2])
                    {

                    }
                    else if (treebgPos == treeFixedPos[3])
                    {

                    }
                }
            }
        }
        
        public override void OnExit()
        {

        }
    }
}