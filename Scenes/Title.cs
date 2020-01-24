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
        private bool fadeAction = false;
        private const int fadeTime = 120;
        private int fadeCounter = 0;
        private int stageCount = 0;
        private int idouCounter;
        private int stagewaittime = 60;
        private bool isRight = false;
        private bool isLeft = false;
        private int treebgPos = 0;
        private int UIpos = 0;
        
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
       
        private int cursorPos = 502;
        private int[] fixedPos = new int[] { 502, 617 };
        
        public Title(Game game) : base(game)
        {

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
            if (stageCount >= 1)
            {
                DX.DrawGraph(treebgPos - Screen.Width, 0, treebg);
                DX.DrawGraph(UIpos - Screen.Width, 0, stagename);
            }
            DX.DrawString(100, 100, "" + treebgPos,DX.GetColor(0, 0,0));
        }
        
        public override void OnLoad()
        {
            fadeCounter = 0;
            fadeAction = true;
        }

        public override void Update()
        {
            fadeCounter++;
            if (fadeCounter == fadeTime)
            {
                fadeAction = false;
            }
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
            if (!fadeAction)
            {
                if (stageCount == 0)
                {
                    if (cursorPos != fixedPos[0] && Input.UP.IsPush())//カーソルが一番上以外の時に↑が押されたら、カーソルを一つ上へ
                    {
                        Sound.Play("cursor_SE.mp3");
                        for (int i = 0; i < fixedPos.Length; i++)
                        {
                            if (cursorPos == fixedPos[i])
                            {
                                cursorPos = fixedPos[i - 1];
                                break;
                            }
                        }
                    }
                    else if (cursorPos != fixedPos[fixedPos.Length - 1] && Input.DOWN.IsPush())//カーソルが一番下以外の時下を押されたら、カーソルが一つ下へ
                    {
                        Sound.Play("cursor_SE.mp3");
                        for (int i = 0; i < fixedPos.Length; i++)
                        {
                            if (cursorPos == fixedPos[i])
                            {
                                cursorPos = fixedPos[i + 1];
                                break;
                            }
                        }
                    }
                    if (cursorPos == fixedPos[0] && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        stageCount = 1;
                    }
                    else if (cursorPos == fixedPos[1] && Input.ACTION.IsPush())
                    {
                        fadeAction = true;
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.SetScene(new Tutolal(Game), new Fade(60, true, true));
                    }
                }
                if(stageCount==1)
                {
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
                    if (treebgPos > Screen.Width)
                    {
                    }
                    else if (treebgPos == 0)
                    {
                        if (Input.RIGHT.IsPush())
                        {
                            treebgPos -= Screen.Width / 80;
                            UIpos -= Screen.Width / 80;
                        }
                        else if (Input.LEFT.IsPush())
                        {
                            treebgPos += Screen.Width / 80;
                            UIpos += Screen.Width / 80;
                        }
                    }
                    else if (treebgPos == -Screen.Width)
                    {
                        if (Input.RIGHT.IsPush())
                        {
                            treebgPos -= Screen.Width / 80;
                            UIpos -= Screen.Width / 80;
                        }
                        else if (Input.LEFT.IsPush())
                        {
                            treebgPos += Screen.Width / 80;
                            UIpos += Screen.Width / 80;
                        }
                    }
                    else if (treebgPos <= -Screen.Width * 2)
                    {
                        if (Input.LEFT.IsPush())
                        {
                            treebgPos += Screen.Width / 80;
                        }
                    }
                    else
                    {
                        if (isRight == true)
                        {
                            idouCounter++;
                            if (idouCounter % 1 == 0)
                            {
                                treebgPos -= Screen.Width / 80;
                                UIpos -= Screen.Width / 80;
                            }
                        }
                        if (isLeft == true && treebgPos < Screen.Width)
                        {
                            idouCounter++;
                            if (idouCounter % 1 == 0)
                            {
                                treebgPos += Screen.Width / 80;
                                UIpos += Screen.Width / 80;
                            }
                        }
                    }
                    if (Input.BACK.IsPush())
                    {
                        stageCount = 0;
                        Sound.Play("cancel_SE.mp3");
                        stagewaittime = 60;
                    }

                    stagewaittime--;
                    if (Input.ACTION.IsPush() && stagewaittime <= 0 && Tutolal.Tutorialcount == 0)
                    {
                        if (treebgPos == Screen.Width)
                        {
                            Sound.Play("decision_SE.mp3");
                            fadeAction = true;
                            Game.bgmManager.currentScene = "title";
                            Game.SetScene(new Tutolal(Game), new Fade(fadeTime, true, true));
                            Tutolal.Tutorialcount = 99;
                        }
                        else if (treebgPos == 0)
                        {
                            Sound.Play("decision_SE.mp3");
                            fadeAction = true;
                            Game.bgmManager.currentScene = "title";
                            Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                        }
                        else if (treebgPos == -Screen.Width)
                        {

                        }
                        else if (treebgPos == -Screen.Width * 2)
                        {

                        }
                    }
                }
            }
        }

        public override void OnExit()
        {

        }
    }
}