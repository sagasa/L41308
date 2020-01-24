﻿using System;
using DxLibDLL;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
        public static int score = 0;
        int[] time = new int[] { 0, 0, 0 };//分,秒,フレーム
        
        int goalTimer = 300;
        int fadeTime = 180;
        //Map座標からScreen座標へ変換する
        public override Vec2f GetScreenPos(Vec2f mapPos)
        {
            return GetFixedPos(mapPos - MapPos) * PlayMap.CellSize;
        }

        public Vec2f GetFixedPos(Vec2f pos)
        {
            while (pos.X < 0)
                pos = pos.SetX(pos.X + Map.MapSize.X);
            while (Map.MapSize.X < pos.X)
                pos = pos.SetX(pos.X - Map.MapSize.X);
            return pos;
        }

        public bool IsInScreen(Vec2f pos)
        {
            return pos.IsInBetween(MapPos, MapPos + PlayMap.ScreenSize);
        }

        private Player player;
        public playerIcon playerIcon;

        private int Flag = ResourceLoader.GetGraph("ハタアイコン.png");
        private int bar = ResourceLoader.GetGraph("マップ.png");
        private int playbg = ResourceLoader.GetGraph("play_bg.png"); //背景描画
        private int scoreImage = ResourceLoader.GetGraph("image_play/score.png");
        private int stageName = ResourceLoader.GetGraph("image_play/stagename_" + 1 + ".png");
        private int watch = ResourceLoader.GetGraph("tokei.png");
        private int colon = ResourceLoader.GetGraph("image_play/colon.png");

        private int fontInterval = 25;
        private float fontScale = 1.0f;
        
        public PlayMap Map { get; private set; }
         
        //表示中の領域の左上のMap座標
        public Vec2f MapPos;

        

        public List<GameObject> gameObjects=new List<GameObject>();

        public ScenePlay(Game game) : base(game)
        {
            Map = new PlayMap(this, "map_1");
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);

            player = new Player(this);
            player.pos = MapPos+new Vec2f(2,2);
            playerIcon = new playerIcon(this);
        }


        public override void Draw()
        {
            //Vec2f pos = GetScreenPos(Vec2f.ZERO);
            DX.DrawGraph(0, 0, playbg);
            base.Draw();
            gameObjects.ForEach(obj => obj.Draw());
            player.Draw();
            
            DX.DrawGraph(550, 200, bar);
            DX.DrawGraph(555, 150, Flag);
            playerIcon.Draw();

            DX.DrawRotaGraph(100, 23 , 0.6, 0, stageName);
            DX.DrawRotaGraph(Screen.Width / 2 - 22, 23, 0.6, 0, scoreImage);
            DX.DrawRotaGraph(Screen.Width - 155, 25, 0.55, 0, watch);
            DX.DrawRotaGraph(Screen.Width - 75, 25, 0.7, 0, colon);

            //スコア
            int digit = 10000;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == score / digit % 10 && (score / digit != 0 || digit == 1))//無駄な0を表示しない
                    {
                        DX.DrawRotaGraph(Screen.Width / 2 - 10 + fontInterval * i, 25, fontScale, 0, ResourceLoader.GetGraph("image_effect/time_" + j + ".png"));
                        break;
                    }
                }
                digit /= 10;
            }
            //タイム
            digit = 10;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (j == time[0] / digit % 10 && (time[0] / digit != 0 || digit == 1))//分
                        DX.DrawRotaGraph(Screen.Width - 120 + fontInterval * i, 25, fontScale, 0, ResourceLoader.GetGraph("image_effect/time_" + j + ".png"));
                    if (j == time[1] / digit % 10)//秒
                        DX.DrawRotaGraph(Screen.Width - 55 + fontInterval * i, 25, fontScale, 0, ResourceLoader.GetGraph("image_effect/time_" + j + ".png"));
                    if (j >= time[0] / digit % 10 && j >= time[1] / digit % 10)
                        break;
                }
                digit /= 10;
            }
        }

        public override void OnExit()
        {
        }

        public override void OnLoad()
        {
            Game.isGoal = false;
            time[0] = 0;
            time[1] = 0;
            time[2] = 0;
            score = 0;
        }

        public override void Update()
        {
            if (!Game.isGoal)
            {
                Game.bgmManager.CrossFade("play", fadeTime);
                time[2]++;
                if (time[2] >= 60)
                {
                    time[1]++;
                    time[2] = 0;
                }
                if (time[1] >= 60)
                {
                    time[0]++;
                    time[1] = 0;
                }
            }

            gameObjects.ForEach(obj=> player.CalcInteract(obj));
            player.Update();
            playerIcon.Update();
            playerIcon.IconPos = (29 - player.Y) * 10.2f;
            gameObjects.ForEach(obj => obj.Update());
            gameObjects.RemoveAll(obj => obj.IsDead());

            base.Update();

            if (Game.isGoal)//ゴールにプレイヤーが触れたら
            {
                player.pos = new Vec2f(4.6f, 2.4f);
                player.velAngle = 0;
                player.angle = 0;
                if (goalTimer > 240)
                {
                    Game.bgmManager.FadeOut("play", 30);
                }

                if (goalTimer == 300)
                {
                    Sound.Play("goal_jingle.mp3");
                }
                goalTimer--;
                if (goalTimer == 0)
                {
                    Game.currentScore = score;
                    Game.currentTime = time;
                    Game.bgmManager.currentScene = "play";
                    Game.SetScene(new SceneResult(Game), new Fade(fadeTime, true, true));
                }
            }
        }
    }
}