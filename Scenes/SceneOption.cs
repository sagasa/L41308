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
        private int[] fixedPosX = new int[] { 100, 400 };
        private int[] fixedPosY = new int[] { 100, 200, 300 };
        private bool[] playOn = new bool[] { true, true };

        private int bg = ResourceLoader.GetGraph("title_bg.png");
        private int dark = ResourceLoader.GetGraph("option/dark25.png");
        private int bgmImage = ResourceLoader.GetGraph("option/bgm_image.png");
        private int seImage = ResourceLoader.GetGraph("option/se_image.png");
        private int back = ResourceLoader.GetGraph("option/back.png");
        private int cursor = ResourceLoader.GetGraph("option/cursor.png");

        public SceneOption(Game game) : base(game)
        { }

        public override void OnLoad()
        { }

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
                else
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
                else
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
                                Sound.DefinitelyPlay("decision_SE.mp3");
                                playOn[i] = true;
                            }
                            if (cursorPosX == fixedPosX[1])
                            {
                                playOn[i] = false;
                            }
                            break;
                        }
                    }
                }
            }

            if (Game.bgmManager.playOn != playOn[0])
                Game.bgmManager.playOn = playOn[0];
            if (Sound.playOn != playOn[1])
                Sound.playOn = playOn[1];
        }

        public override void Draw()
        {
            DX.DrawGraph(0, 0, bg);
            DX.DrawGraph(0, 0, dark);
            DX.DrawRotaGraph(0, fixedPosY[0], 1, 0, bgmImage);
            //BGMの文字
            //SEの文字
            //戻るの文字
            //カーソル
            //オン オフの文字を明るいのと暗いのを2種類

            //DX.DrawRotaGraph(cursorPosX, cursorPosY, 1, 0,cursor);
            //if (Sound.playOn)
            //{
            //    DX.DrawRotaGraph(fixedPosX[0], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/on" + 1 + ".png"));
            //    DX.DrawRotaGraph(fixedPosX[1], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/off" + 2 + ".png"));
            //}
            //else if (!Sound.playOn)
            //{
            //    DX.DrawRotaGraph(fixedPosX[0], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/on" + 2 + ".png"));
            //    DX.DrawRotaGraph(fixedPosX[1], fixedPosY[0], 1, 0, ResourceLoader.GetGraph("option/off" + 1 + ".png"));
            //}
            //if (Game.bgmManager.playOn)
            //{
            //    DX.DrawRotaGraph(fixedPosX[0], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/on" + 1 + ".png"));
            //    DX.DrawRotaGraph(fixedPosX[1], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/off" + 2 + ".png"));
            //}
            //else if (!Game.bgmManager.playOn)
            //{
            //    DX.DrawRotaGraph(fixedPosX[0], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/on" + 2 + ".png"));
            //    DX.DrawRotaGraph(fixedPosX[1], fixedPosY[1], 1, 0, ResourceLoader.GetGraph("option/off" + 1 + ".png"));
            //}
        }

        public override void OnExit()
        {
        }
    }
}