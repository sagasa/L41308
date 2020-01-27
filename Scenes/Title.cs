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
        private const int frameX = 200;
        private const int frameY = 100;
        private const int fontInterval = 30;//文字同士の幅
        private const float fontScale1 = 0.18f;//文字の大きさ
        private const float fontScale2 = 0.1f;//タイムボーナス用

        private DummyPlayer Dummy;
        private int fadeTime = 60;
        private int fadeCounter = 0;
        private int stageCount = 0;
        private int stageWaitTime = 60;
        private bool isRight = false;
        private bool isLeft = false;
        private bool walk = false;
        private int treebgPos = 0;
        private int[] treeFixedPos = new int[] { Screen.Width, 0, -Screen.Width, -Screen.Width * 2 };
        private const int treebgMoveWidth = Screen.Width / 80;
        private bool treeMove = false;
        private int UIpos = 0;
        private int cursorPos;
        private int[] cursorFixedPos = new int[] { 450, 550, 650 };
        int[]  bestTime1 =new int[] {0,0,0};
        int bestscore1 = 0;
        

        private int head = ResourceLoader.GetGraph("player/player_head.png");
        private int horn = ResourceLoader.GetGraph("player/horn.png");
        private int eye = ResourceLoader.GetGraph("player/player_eye.png");
        private int ear = ResourceLoader.GetGraph("player/player_ear.png");
        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
        private int select1 = ResourceLoader.GetGraph("select_1.png");
        private int select2 = ResourceLoader.GetGraph("select_2.png");
        private int option = ResourceLoader.GetGraph("option.png");
        private int icon = ResourceLoader.GetGraph("キリンアイコン.png");
        private int treebg = ResourceLoader.GetGraph("image_select/select_bg.png");
        private int stagename=ResourceLoader.GetGraph("image_select/select_UI.png");
        private int coron = ResourceLoader.GetGraph("image_result/rcolon.png");


        public Title(Game game) : base(game)
        {
            Dummy = new DummyPlayer(this);
        }

        public override void Draw()
        {
            if (stageCount == 0)
            {
                DX.DrawGraph(0, 0, titlebg);
                DX.DrawGraph(135, cursorFixedPos[0], select1);
                DX.DrawGraph(135, cursorFixedPos[1], select2);
                DX.DrawGraph(135, cursorFixedPos[2], option);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, head);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, horn);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, eye);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, ear);
            }
            if (stageCount >= 1)
            {
                
                DX.DrawGraph(treebgPos - Screen.Width, 0, treebg);
                DX.DrawGraph(UIpos - Screen.Width, 0, stagename);
               
                Dummy.Draw();
                if (stageCount == 2)
                {
                    if (treebgPos == 0)
                    {
                        int digit = 1000;
                        int leftCounter= 0;
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                if (j == bestscore1 / digit % 10 && bestscore1 / digit != 0)
                                {
                                    DX.DrawRotaGraph(frameX + fontInterval * leftCounter + 120, 470, fontScale1, 0, ResourceLoader.GetGraph("image_select/mozi_" + j + ".png"));
                                    leftCounter++;
                                }

                            }
                            digit /= 10;
                        }
                        digit = 10;
                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 10; j++)
                            {
                                if (j == bestTime1[0] / digit % 10 && (bestTime1[0] / digit != 0 || digit == 1))//ベストタイム,分
                                {
                                    DX.DrawRotaGraph(frameX + fontInterval * leftCounter, 534, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                                    leftCounter++;
                                }
                            }
                            for (int j = 0; j < 10; j++)
                            {
                                if (j == bestTime1[1] / digit % 10)
                                {
                                    DX.DrawRotaGraph(frameX + fontInterval * (2 + leftCounter), 534, fontScale1, 0, ResourceLoader.GetGraph("image_result/result_num_" + j + ".png"));
                                }
                            }
                            digit /= 10;
                        }
                        DX.DrawRotaGraph(200 + fontInterval * leftCounter, 534, 0.2, 0, coron);
                    }
                }
                
            }
        }
        
        public override void OnLoad()
        {
            cursorPos = cursorFixedPos[0];
            fadeCounter = 0;
            bestTime1 = Game.bestTime;
            bestscore1 = Game.bestScore;
        }

        public override void Update()
        {
            fadeCounter++;
            if (fadeCounter < fadeTime + 10)//BGMのフェード
            {
                if (Game.bgmManager.currentScene == "title"){ }//何もしない
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
                if (stageCount == 0)
                {
                    if (cursorPos != cursorFixedPos[0] && Input.UP.IsPush())//カーソルが一番上以外の時に↑が押されたら、カーソルを一つ上へ
                    {
                        for (int i = 0; i < cursorFixedPos.Length; i++)
                        {
                            if (cursorPos == cursorFixedPos[i])
                            {
                                Sound.Play("cursor_SE.mp3");
                                cursorPos = cursorFixedPos[i - 1];
                                break;
                            }
                        }
                    }
                    else if (cursorPos != cursorFixedPos[cursorFixedPos.Length - 1] && Input.DOWN.IsPush())//カーソルが一番下以外の時下を押されたら、カーソルが一つ下へ
                    {
                        for (int i = 0; i < cursorFixedPos.Length; i++)
                        {
                            if (cursorPos == cursorFixedPos[i])
                            {
                                Sound.Play("cursor_SE.mp3");
                                cursorPos = cursorFixedPos[i + 1];
                                break;
                            }
                        }
                    }
                    if (cursorPos == cursorFixedPos[0] && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        stageCount += 2;
                    }
                    else if (cursorPos == cursorFixedPos[1] && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.fadeAction = true;
                        Game.SetScene(new Tutolal(Game), new Fade(60, true, true));
                    }
                    else if (cursorPos == cursorFixedPos[2] && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.fadeAction = true;
                        Game.SetScene(new SceneOption(Game), new Fade(30, true, true));
                    }
                }
                if (stageCount >= 1)
                {
                    Dummy.Update();
                    Dummy.pos = new Vec2f(Screen.Width / 2, Screen.Height - 64);

                    if (Input.RIGHT.IsPush())
                    {
                        isRight = true;
                        isLeft = false;
                        Dummy.isDunnyRight = true;
                        if (stageCount > 6)
                        {
                            stageCount += 0;
                        }
                        else
                        {
                            stageCount += 1;
                        }
                    }
                    else if (Input.LEFT.IsPush())
                    {
                        isLeft = true;
                        isRight = false;
                        Dummy.isDunnyRight = false;
                        if (stageCount < 2)
                        {
                            stageCount -= 0;
                        }
                        else
                        {
                            stageCount -= 1;
                        }
                    }

                    if (Dummy.isDunnyRight == false)
                    {
                        Dummy.vel = Dummy.vel.SetX(MyMath.Lerp(Dummy.vel.X, -0.01f, 0.1f));
                    }
                    else if (Dummy.isDunnyRight == true)
                    {
                        Dummy.vel = Dummy.vel.SetX(MyMath.Lerp(Dummy.vel.X, 0.01f, 0.1f));
                    }

                    if (treebgPos != treeFixedPos[treeFixedPos.Length - 1] && Input.RIGHT.IsPush())//一番右側以外にいるとき、→が押されたら右へ
                    {
                        for (int i = 0; i < treeFixedPos.Length; i++)
                        {
                            if (treebgPos == treeFixedPos[i])
                            {
                                Sound.Loop("step_SE.mp3");
                                treebgPos -= treebgMoveWidth;
                                UIpos -= treebgMoveWidth;
                                break;
                            }
                        }
                    }
                    else if (treebgPos != treeFixedPos[0] && Input.LEFT.IsPush())//一番左側以外にいるとき、←が押されたら左へ
                    {
                        for (int i = 0; i < treeFixedPos.Length; i++)
                        {
                            if (treebgPos == treeFixedPos[i])
                            {
                                Sound.Loop("step_SE.mp3");
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
                        }
                        if (!isRight)
                        {
                            treebgPos += treebgMoveWidth;
                            UIpos += treebgMoveWidth;
                        }
                    }
                    if (!treeMove)
                    {
                        Sound.Stop("step_SE.mp3");
                    }

                    if (Input.BACK.IsPush())
                    {
                        stageCount = 0;
                        Sound.Play("cancel_SE.mp3");
                        stageWaitTime = 60;
                    }

                    stageWaitTime--;
                    if (Input.ACTION.IsPush() && stageWaitTime <= 0 && Tutolal.Tutorialcount == 0)
                    {
                        if (treebgPos == treeFixedPos[0])
                        {
                            Sound.Play("decision_SE.mp3");
                            Game.fadeAction = true;
                            Game.bgmManager.currentScene = "title";
                            Game.SetScene(new Tutolal(Game), new Fade(fadeTime, true, true));
                            Tutolal.Tutorialcount = 99;
                        }
                        else if (treebgPos == treeFixedPos[1])
                        {
                            Sound.Play("decision_SE.mp3");
                            Game.fadeAction = true;
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
        }
        
        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}