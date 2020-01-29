using System;
using DxLibDLL;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
        public static int score = 0;
        int[] time = new int[] { 0, 0, 0 };//分,秒,フレーム

        public bool IsGoal { get; private set; } = false;

        int goalTimer = 300;
        int fadeTime = 180;

        //ビューポートの座標を移動
        public void Scroll(Vec2f vec)
        {
            MapPos += vec;
            MapPos = GetFixedPos(MapPos);

            BackGroundOffset += vec * 30;
            if (BackGroundOffset.X < 0)
                BackGroundOffset = BackGroundOffset.SetX(Screen.Width);
            if (BackGroundOffset.Y < 0)
                BackGroundOffset = BackGroundOffset.SetY(Screen.Height);
            if (Screen.Width < BackGroundOffset.X)
                BackGroundOffset = BackGroundOffset.SetX(0);
            if (Screen.Height < BackGroundOffset.Y)
                BackGroundOffset = BackGroundOffset.SetY(0);
        }

        private Vec2f BackGroundOffset = Vec2f.ZERO;

        //Map座標からScreen座標へ変換する
        public override Vec2f GetScreenPos(Vec2f mapPos)
        {
            //範囲内に
            mapPos = GetFixedPos(mapPos);

            //スクリーンの中心
            Vec2f screenCenter = MapPos + PlayMap.ScreenSize / 2;
            //スクリーンの中心の反対側のMap座標
            Vec2f inversionPos = Map.MapSize.X / 2 < screenCenter.X ? 
                screenCenter.SetX(screenCenter.X - Map.MapSize.X / 2):
                screenCenter.SetX(screenCenter.X + Map.MapSize.X / 2);

            //補完部分
            if (screenCenter.X < Map.MapSize.X / 2)
            {
                //スクリーンの中心がMap座標の中央より左側
                //補完対象なら補完
                if (inversionPos.X < mapPos.X)
                    mapPos = mapPos.SetX(mapPos.X-Map.MapSize.X);
            }
            else
            {
                //スクリーンの中心がMap座標の中央より右側
                //補完対象なら補完
                if (mapPos.X < inversionPos.X)
                    mapPos = mapPos.SetX(mapPos.X + Map.MapSize.X);
            }

            return (mapPos - MapPos) * PlayMap.CellSize;
        }

        public Vec2f GetFixedPos(Vec2f pos)
        {
            while (pos.X < 0)
                pos = pos.SetX(pos.X + Map.MapSize.X);
            while (Map.MapSize.X < pos.X)
                pos = pos.SetX(pos.X - Map.MapSize.X);
            return pos;
        }

        public bool IsInScreen(Vec2f pos,float ext = 0)
        {
            if (ext != 0)
            {
                Vec2f extVec = new Vec2f(ext,ext);
                return GetScreenPos(pos).IsInBetween(Vec2f.ZERO - extVec, Screen.Size + extVec);
            }
            return GetScreenPos(pos).IsInBetween(Vec2f.ZERO, Screen.Size);
        }

        private Player player;
        public playerIcon playerIcon;

        private static readonly int Flag = ResourceLoader.GetGraph("ハタアイコン.png");
        private static readonly int bar = ResourceLoader.GetGraph("マップ.png");
        private static readonly int treeTop = ResourceLoader.GetGraph("tree_top.png");
        private static readonly int treeMiddle = ResourceLoader.GetGraph("tree_middle.png");
        private static readonly int treePattern = ResourceLoader.GetGraph("treePattern.png");
        private static readonly int treeBottom = ResourceLoader.GetGraph("tree_bottom.png"); //背景描画treePattern
        private static readonly int scoreImage = ResourceLoader.GetGraph("image_play/score.png");
        private static readonly int stageName = ResourceLoader.GetGraph("image_play/stagename_" + 1 + ".png");
        private static readonly int watch = ResourceLoader.GetGraph("tokei.png");
        private static readonly int colon = ResourceLoader.GetGraph("image_play/colon.png");

        private int fontInterval = 25;
        private float fontScale = 1.0f;
        
        public PlayMap Map { get; private set; }
         
        //表示中の領域の左上のMap座標 常にMap座標の範囲内
        public Vec2f MapPos { get; private set; }



        public List<GameObject> gameObjects=new List<GameObject>();

        public ScenePlay(Game game) : base(game)
        {
            Map = new PlayMap(this, "map_1");
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);
            player = new Player(this);
            player.pos = MapPos+new Vec2f(PlayMap.ScreenSize.X/2, PlayMap.ScreenSize.Y / 4*3);
            playerIcon = new playerIcon(this);
        }
        
        public override void Draw()
        {
            Vec2f pos = GetScreenPos(Vec2f.ZERO);
            DX.DrawGraph(0, 0, treeMiddle);
            DX.DrawExtendGraphF(BackGroundOffset.X * -1, BackGroundOffset.Y * -1, BackGroundOffset.X * -1 + Screen.Width * 2, BackGroundOffset.Y * -1 + Screen.Height * 2, treePattern);
            DX.DrawGraph(0, (int) pos.Y - 985, treeTop);
            pos = GetScreenPos(Map.MapSize);
            DX.DrawGraph(0, (int)pos.Y - 90, treeBottom);

            ParticleManagerBottom.Draw();
            gameObjects.ForEach(obj =>
            {
                if(IsInScreen(obj.pos,100))
                    obj.Draw();
            });
            player.Draw();
            
            ParticleManagerTop.Draw();

            DX.DrawGraph(550, 200, bar);
            DX.DrawGraph(555, 150, Flag);
            playerIcon.Draw();

            DX.DrawRotaGraph(100, 23, 0.6, 0, stageName);
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
            Game.fadeAction = false;
        }

        public override void OnLoad()
        {
            IsGoal = false;
            time[0] = 0;
            time[1] = 0;
            time[2] = 0;
            score = 0;
        }
        public void Goal(Vec2f pos)
        {
            player.Goal(pos/0.705f);

            IsGoal = true;

        }

        public override void Update()
        {
            if (!IsGoal)
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
            playerIcon.IconPos = (29 - player.Y) * 10.5f;
            gameObjects.ForEach(obj => obj.Update());
            gameObjects.RemoveAll(obj => obj.IsDead());

            base.Update();

            if (IsGoal)//ゴールにプレイヤーが触れたら
            {
                //player.pos = player.oldPos;
                //player.velAngle = 0;
                //player.angle -= 20;
               
                if (goalTimer > 240)
                {
                    Game.bgmManager.FadeOut("play", 30);
                }

                if (goalTimer == 300)
                {
                    Sound.Play("goal_jingle.mp3");
                }
                goalTimer--;
                if (!Game.fadeAction && goalTimer <= 0)
                {
                    Game.currentScore = score;
                    Game.currentTime = time;
                    Game.bgmManager.currentScene = "play";
                    Game.fadeAction = true;
                    Game.SetScene(new SceneResult(Game), new Fade(fadeTime, true, true));
                }
            }
            
        }
    }
}