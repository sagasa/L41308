using System;
using DxLibDLL;
using SAGASALib;
using Giraffe.Saves;

namespace Giraffe
{
    public class SceneOption : Scene
    {
        private const string SETTINGS = "settings";

        private int cursorPosX = 0;
        private int cursorPosY = 0;
        private readonly int[] fixedPosX = new int[] { 320, 520 };
        private readonly int[] fixedPosY = new int[] { 250, 350, 450, 550 };

        private int bgmPlay = 1;
        private int sePlay = 1;
        private const int on = 0;
        private const int off = 1;

        private const float fontScale = 0.8f;
        private int bg = ResourceLoader.GetGraph("title_bg.png");
        private int dark = ResourceLoader.GetGraph("option/dark25.png");
        private int cursor = ResourceLoader.GetGraph("option/cursor.png");

        public SceneOption(Game game) : base(game)
        { }

        public override void OnLoad()
        {
            cursorPosX = fixedPosX[0];
            cursorPosY = fixedPosY[0];
        }

        public override void Update()
        {
            //BGMの再生
            if (Game.bgmManager.playOn)
                Game.bgmManager.FadeIn(Game.bgmManager.currentScene, 30);

            if (!Game.fadeAction)
            {
                if (cursorPosY != fixedPosY[fixedPosY.Length - 1] &&
                    cursorPosX != fixedPosX[0] && Input.LEFT.IsPush())
                {
                    for (int i = 0; i < fixedPosX.Length; i++)
                    {
                        if (cursorPosX == fixedPosX[i])
                        {
                            Sound.Play("cursor_SE.mp3");
                            cursorPosX = fixedPosX[i - 1];
                            break;
                        }
                    }
                }
                else if (cursorPosY != fixedPosY[fixedPosY.Length - 1] &&
                         cursorPosX != fixedPosX[fixedPosX.Length - 1] && Input.RIGHT.IsPush())
                {
                    for (int i = 0; i < fixedPosX.Length; i++)
                    {
                        if (cursorPosX == fixedPosX[i])
                        {
                            Sound.Play("cursor_SE.mp3");
                            cursorPosX = fixedPosX[i + 1];
                            break;
                        }
                    }
                }
                else if (Input.UP.IsPush())
                {
                    if (cursorPosY == fixedPosY[fixedPosY.Length - 1])
                    {
                        Sound.Play("cursor_SE.mp3");
                        cursorPosX = fixedPosX[0];
                        cursorPosY = fixedPosY[fixedPosY.Length - 2];
                    }
                    else if (cursorPosY != fixedPosY[0])
                    {
                        for (int i = 0; i < fixedPosY.Length; i++)
                        {
                            if (cursorPosY == fixedPosY[i])
                            {
                                Sound.Play("cursor_SE.mp3");
                                cursorPosY = fixedPosY[i - 1];
                                break;
                            }
                        }
                    }
                }
                else if (Input.DOWN.IsPush())
                {
                    if (cursorPosY == fixedPosY[fixedPosY.Length - 2])
                    {
                        Sound.Play("cursor_SE.mp3");
                        cursorPosX = fixedPosX[0];
                        cursorPosY = fixedPosY[fixedPosY.Length - 1];
                    }
                    else if (cursorPosY != fixedPosY[fixedPosY.Length - 1])
                    {
                        for (int i = 0; i < fixedPosY.Length; i++)
                        {
                            if (cursorPosY == fixedPosY[i])
                            {
                                Sound.Play("cursor_SE.mp3");
                                cursorPosY = fixedPosY[i + 1];
                                break;
                            }
                        }
                    }
                }
                else if (Input.ACTION.IsPush())
                {
                    if (cursorPosY == fixedPosY[fixedPosY.Length - 1])
                    {
                        Sound.Play("decision_SE.mp3");
                        Game.fadeAction = true;
                        Game.SetScene(new Title(Game), new Fade(30, true, true));
                    }
                    else
                    {
                        if (cursorPosY == fixedPosY[0])//BGM
                        {
                            if (!Game.bgmManager.playOn && cursorPosX == fixedPosX[0])
                            {
                                Sound.Play("decision_SE.mp3");
                                bgmPlay = on;
                                Game.bgmManager.playOn = true;
                            }
                            else if (Game.bgmManager.playOn && cursorPosX == fixedPosX[1])
                            {
                                Sound.Play("decision_SE.mp3");
                                bgmPlay = off;
                                Game.bgmManager.playOn = false;
                                Game.bgmManager.AllRemove();
                            }
                        }
                        else if (cursorPosY == fixedPosY[1])//SE
                        {
                            if (!Sound.playOn && cursorPosX == fixedPosX[0])
                            {
                                Sound.DefinitelyPlay("decision_SE.mp3");
                                sePlay = on;
                                Sound.playOn = true;
                            }
                            else if (Sound.playOn && cursorPosX == fixedPosX[1])
                            {
                                sePlay = off;
                                Sound.playOn = false;
                            }
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, bg);
            DX.DrawGraph(0, 0, dark);

            DX.DrawRotaGraph(cursorPosX, cursorPosY, fontScale, 0, cursor);

            DX.DrawRotaGraph(120, fixedPosY[0], fontScale, 0, ResourceLoader.GetGraph("option/bgm_image.png"));
            DX.DrawRotaGraph(120, fixedPosY[1], fontScale, 0, ResourceLoader.GetGraph("option/se_image.png"));
            DX.DrawRotaGraph(120, fixedPosY[2], fontScale, 0, ResourceLoader.GetGraph("option/setup.png"));
            DX.DrawRotaGraph(fixedPosX[0], fixedPosY[fixedPosY.Length - 1], fontScale, 0, ResourceLoader.GetGraph("option/back.png"));

            DX.DrawRotaGraph(fixedPosX[0], fixedPosY[0], fontScale, 0, ResourceLoader.GetGraph("option/on" + bgmPlay + ".png"));
            DX.DrawRotaGraph(fixedPosX[1], fixedPosY[1], fontScale, 0, ResourceLoader.GetGraph("option/off" + sePlay + ".png"));
        }

        public override void OnExit()
        {
            Game.fadeAction = false;
            Game.settings.bgmPlayOn = Game.bgmManager.playOn;
            Game.settings.sePlayOn = Sound.playOn;
            SaveManager.Save(SETTINGS, Game.settings);
        }
    }
}