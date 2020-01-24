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
        private bool wait = true;
        private int waitCounter = 0;
        private int fadeTime = 180;
        private int StageCount = 0;
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
       
        
        private int y = 502;
        
        public Title(Game game) : base(game)
        {

        }

        public override void Draw()
        {
            if (StageCount == 0)
            {
                DX.DrawGraph(0, 0, titlebg);
                DX.DrawGraph(135, 515, select1);
                DX.DrawGraph(135, 630, select2);
                DX.DrawRectGraphF(7, y, 0, 0, 128, 128, head);
                DX.DrawRectGraphF(7, y, 0, 0, 128, 128, horn);
                DX.DrawRectGraphF(7, y, 0, 0, 128, 128, eye);
                DX.DrawRectGraphF(7, y, 0, 0, 128, 128, ear);
                if (Input.DOWN.IsPush())
                {
                    y = 617;
                }
                if (Input.UP.IsPush())
                {
                    y = 502;

                }
            }
            if(StageCount>=1)
            {
                
                    DX.DrawGraph(treebgPos - Screen.Width, 0, treebg);

                    DX.DrawGraph(UIpos - Screen.Width, 0, stagename);
                
               
            }
            DX.DrawString(100, 100, "" + treebgPos,DX.GetColor(0, 0,0));
        }

        public override void OnExit()
        {
            
        }

        public override void OnLoad()
        {
            wait = true;
            waitCounter = 0;
        }

        public override void Update()
        {
            if (StageCount == 0)
            {
                waitCounter++;
                if (waitCounter == 60)
                {
                    wait = false;
                }
                if (waitCounter <= fadeTime + 10)//BGMのフェード
                {
                    if (Game.bgmManager.currentScene != "none")
                        Game.bgmManager.CrossFade("title", fadeTime);
                    else
                        Game.bgmManager.FadeIn("title", fadeTime);
                }

                if (Input.DOWN.IsPush() || Input.UP.IsPush())
                {
                    Sound.Play("cursor_SE.mp3");
                }

                if (y == 617 && Input.ACTION.IsPush() && !wait)
                {
                    Sound.Play("decision_SE.mp3");
                    Game.bgmManager.currentScene = "title";
                    Game.SetScene(new Tutolal(Game), new Fade(60, true, true));
                    wait = true;
                }
                else if (y == 502 && Input.ACTION.IsPush() && !wait)
                {
                    Sound.Play("decision_SE.mp3");
                      StageCount += 1;
                }
            }

            if (StageCount == 1)
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

                if (treebgPos >Screen.Width)
                {

                }
                else if(treebgPos==0)
                {
                    if(Input.RIGHT.IsPush())
                    {
                        treebgPos -= Screen.Width / 80;
                        UIpos -= Screen.Width / 80;
                    }
                    else if(Input.LEFT.IsPush())
                    {
                        treebgPos += Screen.Width / 80;
                        UIpos += Screen.Width / 80;
                    }
                }
                else if(treebgPos==-Screen.Width)
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
                else if(treebgPos<=-Screen.Width*2)
                {
                   if(Input.LEFT.IsPush())
                    {
                        treebgPos += Screen.Width / 80;
                    }
                }
                else
                {
                    if (isRight == true)
                    {
                        idouCounter++;
                        if (idouCounter % 1==0)
                        {
                            treebgPos -= Screen.Width /80;
                            UIpos -= Screen.Width / 80;
                        }
                       
                    }
                    if (isLeft == true&&treebgPos<Screen.Width)
                    {
                        idouCounter++;
                        if (idouCounter % 1== 0)
                        {
                            treebgPos += Screen.Width / 80;
                            UIpos += Screen.Width / 80;
                        }
                       
                    }
                }
                
                if(Input.BACK.IsPush())
                {
                    StageCount = 0;
                    Sound.Play("cancel_SE.mp3");
                    stagewaittime = 60;

                }
                stagewaittime--;
                if (Input.ACTION.IsPush()&&stagewaittime<=0&&Tutolal.Tutorialcount==0)
                {
                    waitCounter++;
                    if (waitCounter == 60)
                    {
                        wait = false;
                    }
                    if (waitCounter <= fadeTime + 10)
                    {
                        Game.bgmManager.FadeIn("title", fadeTime);
                    }
                    if(wait==true)
                    {

                    }
                    if (!wait && treebgPos == Screen.Width)
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.SetScene(new Tutolal(Game), new Fade(fadeTime, true, true));
                        wait = true;
                        Tutolal.Tutorialcount = 99;
                    }
                    else if (!wait && treebgPos == 0)
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                        wait = true;
                    }
                    else if (treebgPos == -Screen.Width )
                    {

                    }
                    else if (treebgPos == -Screen.Width * 2)
                    {

                    }
                   
                }
            }
        }
    }
}