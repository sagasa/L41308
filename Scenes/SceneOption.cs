using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public class SceneOption : Scene
    {
        private int cursorPosX = 0;
        private int cursorPosY = 0;
        private int[] fixedPosX = new int[] { 320, 520 };
        private int[] fixedPosY = new int[] { 250, 450, 650 };
        private bool[] playOn = new bool[] { true, true };
        private bool bgmReset = false;

        private int bg = ResourceLoader.GetGraph("title_bg.png");
        private int dark = ResourceLoader.GetGraph("option/dark25.png");
        private int bgmImage = ResourceLoader.GetGraph("option/bgm_image.png");
        private int seImage = ResourceLoader.GetGraph("option/se_image.png");
        private int back = ResourceLoader.GetGraph("option/back.png");
        private int cursor = ResourceLoader.GetGraph("image_result/cursor.png");

        public SceneOption(Game game) : base(game)
        { }

        public override void OnLoad()
        {
            playOn[0] = Game.bgmManager.playOn;
            playOn[1] = Sound.playOn;
            cursorPosX = fixedPosX[0];
            cursorPosY = fixedPosY[0];
        }

        public override void Update()
        {
            if (cursorPosY != fixedPosY[fixedPosY.Length - 1] && Input.LEFT.IsPush())
            {
                for (int i = 0; i < fixedPosX.Length; i++)
                {
                    if (cursorPosX == fixedPosX[0])
                        break;
                    if (cursorPosX == fixedPosX[i])
                    {
                        Sound.Play("cursor_SE.mp3");
                        cursorPosX = fixedPosX[i - 1];
                        break;
                    }
                }
            }
            else if (cursorPosY != fixedPosY[fixedPosY.Length - 1] && Input.RIGHT.IsPush())
            {
                for (int i = 0; i < fixedPosX.Length; i++)
                {
                    if (cursorPosX == fixedPosX[fixedPosX.Length - 1])
                        break;
                    if (cursorPosX == fixedPosX[i])
                    {
                        Sound.Play("cursor_SE.mp3");
                        cursorPosX = fixedPosX[i + 1];
                        break;
                    }
                }
            }
            if (Input.UP.IsPush())
            {
                if (cursorPosY == fixedPosY[fixedPosY.Length - 1])
                {
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
            if (Input.DOWN.IsPush())
            {
                if (cursorPosY == fixedPosY[fixedPosY.Length - 2])
                {
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
            if (Input.ACTION.IsPush())
            {
                if (cursorPosY == fixedPosY[fixedPosY.Length - 1])
                    Game.SetScene(new Title(Game), new Fade(120, true, true));
                else
                {
                    for (int i = 0; i < playOn.Length; i++)
                    {
                        if (cursorPosY == fixedPosY[i])
                        {
                            if (cursorPosX == fixedPosX[0])
                            {
                                playOn[i] = true;
                                if (cursorPosY == fixedPosY[1])
                                    Sound.DefinitelyPlay("decision_SE.mp3");
                                else
                                    Sound.Play("decision_SE.mp3");
                            }
                            if (cursorPosX == fixedPosX[1])
                            {
                                playOn[i] = false;
                                if (cursorPosY != fixedPosY[1])
                                    Sound.Play("decision_SE.mp3");
                            }
                            break;
                        }
                    }
                }
            }

            if (Game.bgmManager.playOn != playOn[0])
            {
                bgmReset = false;
                Game.bgmManager.playOn = playOn[0];
            }
            if (Sound.playOn != playOn[1])
            {
                Sound.playOn = playOn[1];
            }
            if (!Game.bgmManager.playOn && !bgmReset)
            {
                bgmReset = true;
                Game.bgmManager.AllRemove();
            }
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, bg);
            DX.DrawGraph(0, 0, dark);
            DX.DrawRotaGraph(120, fixedPosY[0], 1, 0, bgmImage);
            DX.DrawRotaGraph(120, fixedPosY[1], 1, 0, seImage);
            DX.DrawRotaGraph(cursorPosX, cursorPosY, 1, 0, cursor);
            DX.DrawRotaGraph(fixedPosX[0], fixedPosY[fixedPosY.Length - 1], 1, 0, back);
            
            if (playOn[0])
            {
                DX.DrawRotaGraph(fixedPosX[0], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/on" + 1 + ".png"));
                DX.DrawRotaGraph(fixedPosX[1], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/off" + 2 + ".png"));
            }
            else if (!playOn[0])
            {
                DX.DrawRotaGraph(fixedPosX[0], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/on" + 2 + ".png"));
                DX.DrawRotaGraph(fixedPosX[1], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/off" + 1 + ".png"));
            }
            if (playOn[1])
            {
                DX.DrawRotaGraph(fixedPosX[0], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/on" + 1 + ".png"));
                DX.DrawRotaGraph(fixedPosX[1], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/off" + 2 + ".png"));
            }
            else if (!playOn[1])
            {
                DX.DrawRotaGraph(fixedPosX[0], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/on" + 2 + ".png"));
                DX.DrawRotaGraph(fixedPosX[1], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/off" + 1 + ".png"));
            }
        }

        public override void OnExit()
        {
        }
    }
}