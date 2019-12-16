﻿using System;
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
        private bool wait = false;

        private int fadeTime = 180;

        private int head = ResourceLoader.GetGraph("player/player_head.png");
        private int horn = ResourceLoader.GetGraph("player/horn.png");
        private int eye = ResourceLoader.GetGraph("player/player_eye.png");
        private int ear = ResourceLoader.GetGraph("player/player_ear.png");
        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
        private int select1 = ResourceLoader.GetGraph("select_1.png");
        private int select2 = ResourceLoader.GetGraph("select_2.png");
        int y = 502;
        
        public Title(Game game) : base(game)
        {

        }

        public override void Draw()
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

        public override void OnExit()
        {
            
        }

        public override void OnLoad()
        {
            wait = false;
            Game.bgmManager.fadeInit = true;
        }

        public override void Update()
        {
            Game.isGoal = false;

            if(!wait)
            {
                Game.bgmManager.FadeIn("title", fadeTime);
            }

            if (Input.DOWN.IsPush() || Input.UP.IsPush())
            {
                Sound.Play("cursor_SE.mp3");
            }

            if (y == 617 && Input.ACTION.IsPush())
            {
                Sound.Play("cancel_SE.mp3");
            }
            else if (y == 502 && Input.ACTION.IsPush())
            {
                if (!wait)
                {
                    Sound.Play("decision_SE.mp3");
                    Game.SetScene(new ScenePlay(Game), new Fade(fadeTime, true, true));
                    wait = !wait;
                }
            }
        }
    }
}

