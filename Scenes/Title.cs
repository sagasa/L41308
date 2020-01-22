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
        private int treebgX = 0;
        private int UIX =0;
        private int idouCounter;
        private int stagewaittime = 60;
        

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
                DX.DrawGraph(treebgX, 0, treebg);
                if (Input.LEFT.IsHold() || Input.RIGHT.IsHold())
                {
                   
                }
                else if(treebgX==0||treebgX==-633||treebgX==-1281)
                {
                    DX.DrawGraph(UIX, 0, stagename);
                }
                if (treebgX <=-632 && treebgX >= -1000)//stage2
                {
                    idouCounter++;
                    if (Input.RIGHT.IsHold()||Input.LEFT.IsHold())
                    {

                    }
                    else if (idouCounter % 1 == 0 )
                    {
                        treebgX += 3;
                        UIX += 3;
                    }
                    
                }
                if(treebgX<=-400&&treebgX>=-631)//stage2
                {
                    idouCounter++;
                    if(Input.RIGHT.IsHold()||Input.LEFT.IsHold())
                    {

                    }
                    else if(idouCounter%1==0&&treebgX<=-400)
                    {
                        treebgX -= 3;
                        UIX -= 3;
                    }
                }
                if(treebgX>=-400&&treebgX<=0)//stage1
                {
                    idouCounter++;
                    if(Input.RIGHT.IsHold()||Input.LEFT.IsHold())
                    {

                    }
                    else if(idouCounter%1==0&&treebgX>=-400)
                    {
                        treebgX += 3;
                        UIX += 3;
                    }
                }
                if(treebgX<=-1000)//stage3
                {
                    idouCounter++;
                    if(Input.RIGHT.IsHold()||Input.LEFT.IsHold())
                    {

                    }
                    else if(idouCounter%1==0&&treebgX<=-1000)
                    {
                        treebgX -= 3;
                        UIX -= 3;
                    }
                }
            }
            DX.DrawString(100, 50, "" + treebgX, DX.GetColor(0, 0, 0));
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
                if (waitCounter <= fadeTime + 10)
                {
                    Game.bgmManager.FadeIn("title", fadeTime);
                }

                if (Input.DOWN.IsPush() || Input.UP.IsPush())
                {
                    Sound.Play("cursor_SE.mp3");
                }

                if (y == 617 && Input.ACTION.IsPush() && !wait)
                {
                    Sound.Play("decision_SE.mp3");
                    Game.SetScene(new Tutolal(Game), new Fade(60, true, true));
                    Game.bgmManager.CrossFade("title", "tutorial", fadeTime);
                    wait = true;
                }
                else if (y == 502 && Input.ACTION.IsPush() && !wait)
                {
                    Sound.Play("decision_SE.mp3");
                    //Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                    //Game.bgmManager.CrossFade("title", "play", fadeTime);
                    //wait = true;
                   
                        StageCount += 1;
                   
                   
                }
            }

            if (StageCount == 1)
            {
                
                if (Input.RIGHT.IsHold())
                {
                    treebgX -= 3;
                    UIX -= 3;
                }
                if (Input.LEFT.IsHold())
                {
                    treebgX += 3;
                    UIX += 3;
                }
                if (treebgX <= -1281)
                {
                    treebgX = -1281;
                    UIX = -1281;
                }
                if (treebgX > 0)
                {
                    treebgX = 0;
                    UIX = 0;
                }
                if(Input.BACK.IsPush())
                {
                    StageCount = 0;
                    Sound.Play("cancel_SE.mp3");
                    stagewaittime = 60;

                }
                stagewaittime--;
                if (Input.ACTION.IsPush()&&stagewaittime<=0)
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
                    if (treebgX == 0)//stage1
                    {
                         Sound.Play("decision_SE.mp3");
                        Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                        Game.bgmManager.CrossFade("title", "play", fadeTime);
                        wait = true;
                    }
                    if (treebgX == -633)//stage2
                    {
                       
                    }
                    if (treebgX == -1281)
                    {
                       
                    }
                }
             



            }
        }
    }
}