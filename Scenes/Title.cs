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
        private const int fadeTime = 90;
        private const int shortFadeTime = 60;
        private int fadeCounter = 0;
        private int stageWaitTime = 60;
        private bool isRight = false;
        private int treebgPos = 0;
        private float neckSize = 6.2f;
        private int neckPos = 600;
        private int[] treeFixedPos = new int[] { Screen.Width, 0, -Screen.Width, -Screen.Width * 2 };
        private const int treebgMoveWidth = Screen.Width / 80;
        private bool treeMove = false;
        private int UIpos = 0;
        private int cursorPos;
        private int[] cursorFixedPosY = new int[] { 450, 550, 650 };
        int[]  bestTime1 =new int[] {0,0,0};
        int bestscore1 = 0;

        public static bool stageSelect = false;

        private int head = ResourceLoader.GetGraph("player/player_head.png");
        private int horn = ResourceLoader.GetGraph("player/horn.png");
        private int eye = ResourceLoader.GetGraph("player/player_eye.png");
        private int ear = ResourceLoader.GetGraph("player/player_ear.png");
        private int body = ResourceLoader.GetGraph("player/body.png");
        private int leg = ResourceLoader.GetGraph("player/player_leg.png");
        private int tail = ResourceLoader.GetGraph("player/player_tail.png");
        private int neck = ResourceLoader.GetGraph("player/neck.png");
        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
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
            if (!stageSelect)//タイトル画面
            {
                DX.DrawGraph(0, 0, titlebg);
                //for (int i = 0; i < cursorFixedPosY.Length; i++)//選択肢
                //{
                //    DX.DrawGraph(135, cursorFixedPosY[i], ResourceLoader.GetGraph("select_" + i + ".png"));
                //}
                DX.DrawGraph(135, cursorFixedPosY[0], ResourceLoader.GetGraph("select_" + 0 + ".png"));
                DX.DrawGraph(90, cursorFixedPosY[1], ResourceLoader.GetGraph("select_" + 1 + ".png"));
                DX.DrawGraph(135, cursorFixedPosY[2], ResourceLoader.GetGraph("select_" + 2 + ".png"));
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, head);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, horn);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, eye);
                DX.DrawRectGraphF(7, cursorPos, 0, 0, 128, 128, ear);
                DX.DrawRectGraphF(7, 668, 0, 0, 128, 128, leg);
                DX.DrawGraph(7, 668, body);
                DX.DrawRectGraphF(7, 668, 0, 0, 128, 128, tail);
                DX.DrawRotaGraph3(7, neckPos, 0, 64, 1, neckSize, 0, neck, DX.TRUE, DX.FALSE);

            }
            if (stageSelect)//ステージセレクト
            {
                DX.DrawGraph(treebgPos - Screen.Width, 0, treebg);
                DX.DrawGraph(UIpos - Screen.Width, 0, stagename);
                Dummy.Draw();

                if (treebgPos == treeFixedPos[1])
                {
                    int digit = 1000;
                    int leftCounter = 0;
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
        
        public override void OnLoad()
        {
            cursorPos = cursorFixedPosY[0];
            Dummy.pos = new Vec2f(Screen.Width / 2, Screen.Height - 64);
            fadeCounter = 0;

            bestTime1 = Game.bestTime;
            bestscore1 = Game.bestScore;
        }

        public override void Update()
        {
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
                if (!stageSelect)//タイトル画面
                {
                    if (cursorPos != cursorFixedPosY[0] && Input.UP.IsPush())//カーソルが一番上以外の時に↑が押されたら、カーソルを一つ上へ
                    {
                        for (int i = 0; i < cursorFixedPosY.Length; i++)
                        {
                            if (cursorPos == cursorFixedPosY[i])
                            {
                                neckSize += 2.5f;
                                neckPos -= 61;
                                Sound.Play("cursor_SE.mp3");
                                cursorPos = cursorFixedPosY[i - 1];
                                break;
                            }
                        }
                    }
                    else if (cursorPos != cursorFixedPosY[cursorFixedPosY.Length - 1] && Input.DOWN.IsPush())//カーソルが一番下以外の時下を押されたら、カーソルが一つ下へ
                    {
                        for (int i = 0; i < cursorFixedPosY.Length; i++)
                        {
                            if (cursorPos == cursorFixedPosY[i])
                            {
                                neckSize -= 2.5f;
                                neckPos +=61;
                                Sound.Play("cursor_SE.mp3");
                                cursorPos = cursorFixedPosY[i + 1];
                                break;
                            }
                        }
                    }
                    if (cursorPos == cursorFixedPosY[0] && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        stageSelect = true;
                    }
                    else if (cursorPos == cursorFixedPosY[1] && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.fadeAction = true;
                        Game.SetScene(new Tutolal(Game), new Fade(shortFadeTime, true, true));
                        Tutolal.Tutorialcount += 1;
                    }
                    else if (cursorPos == cursorFixedPosY[2] && Input.ACTION.IsPush())
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.bgmManager.currentScene = "title";
                        Game.fadeAction = true;
                        Game.SetScene(new SceneOption(Game), new Fade(shortFadeTime, true, true));
                    }
                }
                else if (stageSelect)//ステージセレクト
                {
                    stageWaitTime--;
                    Dummy.Update();

                    if (Dummy.isDunnyRight == false)
                    {
                        Dummy.vel = Dummy.vel.SetX(MyMath.Lerp(Dummy.vel.X, -0.01f, 0.1f));
                    }
                    else if (Dummy.isDunnyRight == true)
                    {
                        Dummy.vel = Dummy.vel.SetX(MyMath.Lerp(Dummy.vel.X, 0.01f, 0.1f));
                    }

                    if (Input.RIGHT.IsPush())
                    {
                        isRight = true;
                        Dummy.isDunnyRight = true;
                    }
                    else if (Input.LEFT.IsPush())
                    {
                        isRight = false;
                        Dummy.isDunnyRight = false;
                    }

                    if (treebgPos != treeFixedPos[treeFixedPos.Length - 1] && Input.RIGHT.IsPush())//一番右側以外にいるとき、→が押されたら右へ
                    {
                        for (int i = 0; i < treeFixedPos.Length; i++)
                        {
                            if (treebgPos == treeFixedPos[i])
                            {
                                if (!Sound.CheckPlaySound("step_SE.mp3"))
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
                                if (!Sound.CheckPlaySound("step_SE.mp3"))
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
                    if (!treeMove || !stageSelect)
                        Sound.Stop("step_SE.mp3");
                    
                    if (Input.ACTION.IsPush() && stageWaitTime <= 0 && Tutolal.Tutorialcount == 0)
                    {
                        //共通のもの
                        for (int i = 0; i < treeFixedPos.Length; i++)
                        {
                            if (treebgPos == treeFixedPos[i])
                            {
                                Sound.Play("decision_SE.mp3");
                                Game.fadeAction = true;
                                Game.bgmManager.currentScene = "title";
                                break;
                            }
                        }
                        //固有のもの
                        if (treebgPos == treeFixedPos[0])//チュートリアル
                        {
                            Tutolal.Tutorialcount += 99;
                            Game.SetScene(new Tutolal(Game), new Fade(fadeTime, true, true));
                        }
                        else if (treebgPos == treeFixedPos[1])//ステージ1
                        {
                            Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                        }
                        else if (treebgPos == treeFixedPos[2]) { }
                        else if (treebgPos == treeFixedPos[3]) { }
                    }
                    if (Input.BACK.IsPush())
                    {
                        stageSelect = false;
                        Sound.Play("cancel_SE.mp3");
                        stageWaitTime = 60;
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