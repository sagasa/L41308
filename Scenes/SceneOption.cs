using System;
using DxLibDLL;
using SAGASALib;
using Giraffe.Saves;
namespace Giraffe
{
    public class SceneOption : Scene
    {
        private const string SETTINGS = "settings";

        private readonly int[] cursorFixedPosX = new int[] { 320, 520 };
        private readonly int[] cursorFixedPosY = new int[] { 250, 350, 450, 575 };
        private int cursorPosX = 0;
        private int cursorPosY = 0;
        private int bgmPlay = 0;//描画用
        private int sePlay = 0;
        private const int on = 0;
        private const int off = 1;
        private readonly int[] messageFixedPosX = new int[] { Screen.Width / 2 - 125, Screen.Width / 2 + 125, Screen.Width / 2 };
        private int messageCursorPosX = 0;
        private const int messageCursorPosY = 450;
        private readonly string[] messages = new string[] { "save", "save_done", "reset", "reset_done" };
        private string displayMessage;
        private bool messageIndicate = false;
        private const float fontScale = 0.8f;
        private int shadow = ResourceLoader.GetGraph("bg/shadow25.png");
        private int cursor = ResourceLoader.GetGraph("option/cursor.png");
        public SceneOption(Game game) : base(game)
        { }
        public override void OnLoad()
        {
            Game.bgmManager.Set(60);
            cursorPosX = cursorFixedPosX[0];
            cursorPosY = cursorFixedPosY[0];
            if (Game.bgmManager.playOn)
                bgmPlay = on;
            else
                bgmPlay = off;
            if (Sound.playOn)
                sePlay = on;
            else
                sePlay = off;
        }
        public override void Update()
        {
            if (!Game.fadeAction && !messageIndicate)
            {
                if (cursorPosX != cursorFixedPosX[0] && Input.LEFT.IsPush())
                {
                    for (int i = 0; i < cursorFixedPosX.Length; i++)
                    {
                        if (cursorPosX == cursorFixedPosX[i])
                        {
                            Sound.Play("cursor_SE.mp3");
                            cursorPosX = cursorFixedPosX[i - 1];
                            break;
                        }
                    }
                }
                else if (cursorPosY != cursorFixedPosY[cursorFixedPosY.Length - 1] &&
                         cursorPosX != cursorFixedPosX[cursorFixedPosX.Length - 1] && Input.RIGHT.IsPush())
                {
                    for (int i = 0; i < cursorFixedPosX.Length; i++)
                    {
                        if (cursorPosX == cursorFixedPosX[i])
                        {
                            Sound.Play("cursor_SE.mp3");
                            cursorPosX = cursorFixedPosX[i + 1];
                            break;
                        }
                    }
                }
                else if (cursorPosY != cursorFixedPosY[0] && Input.UP.IsPush())
                {
                    for (int i = 0; i < cursorFixedPosY.Length; i++)
                    {
                        if (cursorPosY == cursorFixedPosY[i])
                        {
                            Sound.Play("cursor_SE.mp3");
                            cursorPosY = cursorFixedPosY[i - 1];
                            break;
                        }
                    }
                }
                else if (cursorPosY != cursorFixedPosY[cursorFixedPosY.Length - 1] && Input.DOWN.IsPush())
                {
                    if (cursorPosY == cursorFixedPosY[cursorFixedPosY.Length - 2])
                    {
                        Sound.Play("cursor_SE.mp3");
                        cursorPosX = cursorFixedPosX[0];
                        cursorPosY = cursorFixedPosY[cursorFixedPosY.Length - 1];
                    }
                    else
                    {
                        for (int i = 0; i < cursorFixedPosY.Length; i++)
                        {
                            if (cursorPosY == cursorFixedPosY[i])
                            {
                                Sound.Play("cursor_SE.mp3");
                                cursorPosY = cursorFixedPosY[i + 1];
                                break;
                            }
                        }
                    }
                }
                else if (Input.ACTION.IsPush())
                {
                    if (cursorPosY == cursorFixedPosY[cursorFixedPosY.Length - 1])
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.fadeAction = true;
                        Game.SetScene(new Title(Game,0), new Fade(30, true, true));
                    }
                    else
                    {
                        if (cursorPosY == cursorFixedPosY[0])//BGM
                        {
                            if (!Game.bgmManager.playOn && cursorPosX == cursorFixedPosX[0])
                            {
                                Sound.Play("decision_SE.mp3");
                                bgmPlay = on;
                                Game.bgmManager.playOn = true;
                                Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.FadeIn);
                            }
                            else if (Game.bgmManager.playOn && cursorPosX == cursorFixedPosX[1])
                            {
                                Sound.Play("decision_SE.mp3");
                                bgmPlay = off;
                                Game.bgmManager.playOn = false;
                                Game.bgmManager.AllRemove();
                            }
                        }
                        else if (cursorPosY == cursorFixedPosY[1])//SE
                        {
                            if (!Sound.playOn && cursorPosX == cursorFixedPosX[0])
                            {
                                Sound.DefinitelyPlay("decision_SE.mp3");
                                sePlay = on;
                                Sound.playOn = true;
                            }
                            else if (Sound.playOn && cursorPosX == cursorFixedPosX[1])
                            {
                                sePlay = off;
                                Sound.playOn = false;
                            }
                        }
                        else if (cursorPosY == cursorFixedPosY[2])//設定
                        {
                            if (cursorPosX == cursorFixedPosX[0])//セーブ
                            {
                                messageIndicate = true;
                                displayMessage = messages[0];
                                messageCursorPosX = messageFixedPosX[0];
                            }
                            else if (cursorPosX == cursorFixedPosX[1])//リセット
                            {
                                messageIndicate = true;
                                displayMessage = messages[2];
                                messageCursorPosX = messageFixedPosX[1];
                            }
                        }
                    }
                }
            }
            else if (!Game.fadeAction && messageIndicate)
            {
                if (messageCursorPosX == messageFixedPosX[0] && Input.RIGHT.IsPush())
                {
                    Sound.Play("cursor_SE.mp3");
                    messageCursorPosX = messageFixedPosX[1];
                }
                else if (messageCursorPosX == messageFixedPosX[1] && Input.LEFT.IsPush())
                {
                    Sound.Play("cursor_SE.mp3");
                    messageCursorPosX = messageFixedPosX[0];
                }
                else if (Input.ACTION.IsPush())
                {
                    if (messageCursorPosX == messageFixedPosX[0])
                    {
                        Sound.Play("decision_SE.mp3");
                        if (displayMessage == messages[0])//セーブ
                        {
                            Game.settings.bgmPlayOn = Game.bgmManager.playOn;
                            Game.settings.sePlayOn = Sound.playOn;
                            SaveManager.Save(SETTINGS, Game.settings);
                            displayMessage = messages[1];
                            messageCursorPosX = messageFixedPosX[2];
                        }
                        else if (displayMessage == messages[2])//リセット
                        {
                            Game.bgmManager.playOn = true;
                            Sound.playOn = true;
                            Game.settings.bgmPlayOn = true;
                            Game.settings.sePlayOn = true;
                            bgmPlay = on;
                            sePlay = on;
                            displayMessage = messages[3];
                            messageCursorPosX = messageFixedPosX[2];
                        }
                    }
                    else if (messageCursorPosX == messageFixedPosX[1])
                    {
                        Sound.Play("decision_SE.mp3");
                        messageIndicate = false;
                    }
                    else if (messageCursorPosX == messageFixedPosX[2])
                    {
                        if (displayMessage == messages[1] || displayMessage == messages[3])
                        {
                            Sound.Play("decision_SE.mp3");
                            messageIndicate = false;
                        }
                    }
                }
                else if (Input.BACK.IsPush())
                {
                    Sound.Play("cancel_SE.mp3");
                    messageIndicate = false;
                }
            }
        }
        public override void Draw()
        {
            DX.DrawGraph(0, 0, ResourceLoader.GetGraph("title_bg.png"));
            DX.DrawGraph(0, 0, shadow);
            if (!messageIndicate)
                DX.DrawRotaGraph(cursorPosX, cursorPosY, fontScale, 0, cursor);
            DX.DrawRotaGraph(120, cursorFixedPosY[0], fontScale, 0, ResourceLoader.GetGraph("option/bgm_image.png"));
            DX.DrawRotaGraph(120, cursorFixedPosY[1], fontScale, 0, ResourceLoader.GetGraph("option/se_image.png"));
            DX.DrawRotaGraph(120, cursorFixedPosY[2], fontScale, 0, ResourceLoader.GetGraph("option/setup.png"));
            DX.DrawRotaGraph(cursorFixedPosX[0], cursorFixedPosY[0], fontScale, 0, ResourceLoader.GetGraph("option/on" + bgmPlay + ".png"));
            DX.DrawRotaGraph(cursorFixedPosX[1], cursorFixedPosY[0], fontScale, 0, ResourceLoader.GetGraph("option/off" + bgmPlay + ".png"));
            DX.DrawRotaGraph(cursorFixedPosX[0], cursorFixedPosY[1], fontScale, 0, ResourceLoader.GetGraph("option/on" + sePlay + ".png"));
            DX.DrawRotaGraph(cursorFixedPosX[1], cursorFixedPosY[1], fontScale, 0, ResourceLoader.GetGraph("option/off" + sePlay + ".png"));
            DX.DrawRotaGraph(cursorFixedPosX[0], cursorFixedPosY[2], 0.6f, 0, ResourceLoader.GetGraph("option/save.png"));
            DX.DrawRotaGraph(cursorFixedPosX[1], cursorFixedPosY[2], 0.6f, 0, ResourceLoader.GetGraph("option/reset.png"));
            DX.DrawRotaGraph(cursorFixedPosX[0], cursorFixedPosY[cursorFixedPosY.Length - 1], fontScale, 0, ResourceLoader.GetGraph("option/back.png"));
            if (messageIndicate)
            {
                DX.DrawGraph(0, 0, shadow);
                DX.DrawGraph(0, 0, ResourceLoader.GetGraph("option/" + displayMessage + "_message.png"));
                DX.DrawRotaGraph(messageCursorPosX, messageCursorPosY, 0.9, 0, cursor);
                if (displayMessage == messages[0] || displayMessage == messages[2])
                {
                    DX.DrawRotaGraph(messageFixedPosX[0], messageCursorPosY, 0.9, 0, ResourceLoader.GetGraph("option/yes.png"));
                    DX.DrawRotaGraph(messageFixedPosX[1], messageCursorPosY, 0.9, 0, ResourceLoader.GetGraph("option/no.png"));
                }
                else if (displayMessage == messages[1] || displayMessage == messages[3])
                {
                    DX.DrawRotaGraph(messageFixedPosX[2], messageCursorPosY, 0.9, 0, ResourceLoader.GetGraph("option/yes.png"));
                }
            }
        }
        public override void OnExit()
        {
            Game.fadeAction = false;
        }
    }
}