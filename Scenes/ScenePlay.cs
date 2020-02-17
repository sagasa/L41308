using System;
using DxLibDLL;
using System.Collections.Generic;
using SAGASALib;

namespace Giraffe
{
    public class ScenePlay : Scene
    {
        public int score = 0;
        public DateTime time;

        public bool IsGoal { get; private set; } = false;

        int goalTimer = 300;
        private const int fadeTime = 180;

        public readonly Navigator Navi;

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
                screenCenter.SetX(screenCenter.X - Map.MapSize.X / 2) :
                screenCenter.SetX(screenCenter.X + Map.MapSize.X / 2);

            //補完部分
            if (screenCenter.X < Map.MapSize.X / 2)
            {
                //スクリーンの中心がMap座標の中央より左側
                //補完対象なら補完
                if (inversionPos.X < mapPos.X)
                    mapPos = mapPos.SetX(mapPos.X - Map.MapSize.X);
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

        public bool IsInScreen(Vec2f pos, float ext = 0)
        {
            if (ext != 0)
            {
                Vec2f extVec = new Vec2f(ext, ext);
                return GetScreenPos(pos).IsInBetween(Vec2f.ZERO - extVec, Screen.Size + extVec);
            }
            return GetScreenPos(pos).IsInBetween(Vec2f.ZERO, Screen.Size);
        }

        private Player player;
        public playerIcon playerIcon;

        private static readonly int Flag = ResourceLoader.GetGraph("ハタアイコン.png");
        private static readonly int bar = ResourceLoader.GetGraph("マップ.png");
        private readonly int treeTop;
        private readonly int treeMiddle;
        private readonly int treeBottom;
        private readonly int stageName;
        private static readonly int treePattern = ResourceLoader.GetGraph("treePattern.png");

        private static readonly int scoreImage = ResourceLoader.GetGraph("image_play/score.png");
        private static readonly int watch = ResourceLoader.GetGraph("tokei.png");
        private static readonly int colon = ResourceLoader.GetGraph("image_play/colon.png");

        private int fontInterval = 25;
        private float fontScale = 1.0f;

        public PlayMap Map { get; private set; }

        //表示中の領域の左上のMap座標 常にMap座標の範囲内
        public Vec2f MapPos { get; private set; }



        public List<GameObject> gameObjects = new List<GameObject>();

        public readonly string ResourcesName;
        public readonly int StageNum;

        public ScenePlay(Game game, PlayMap map, string name, int num) : base(game)
        {
            ResourcesName = name;
            StageNum = num;
            Map = map;
            Map.SpawnObject(this);
            Navi = new Navigator(this);
            player = new Player(this);

            // TODO: プレイヤーとマップが恐らくnullで返されたせいでエラーが出ていたので追加
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);
            player.pos = MapPos + new Vec2f(PlayMap.ScreenSize.X / 2, PlayMap.ScreenSize.Y / 4 * 3);

            playerIcon = new playerIcon(this);

            treeTop = ResourceLoader.GetGraph("tree_top" + ResourcesName + ".png");
            treeMiddle = ResourceLoader.GetGraph("tree_middle" + ResourcesName + ".png");
            treeBottom = ResourceLoader.GetGraph("tree_bottom" + ResourcesName + ".png"); //背景描画treePattern
            stageName = ResourceLoader.GetGraph("image_play/stagename" + ResourcesName + ".png");
        }

        public void Init()
        {
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);
            player.pos = MapPos + new Vec2f(PlayMap.ScreenSize.X / 2, PlayMap.ScreenSize.Y / 4 * 3);
        }

        public override void Draw()
        {
            Vec2f pos = GetScreenPos(Vec2f.ZERO);
            DX.DrawGraph(0, 0, treeMiddle);
            DX.DrawExtendGraphF(BackGroundOffset.X * -1, BackGroundOffset.Y * -1, BackGroundOffset.X * -1 + Screen.Width * 2, BackGroundOffset.Y * -1 + Screen.Height * 2, treePattern);
            DX.DrawGraph(0, (int)pos.Y - 985, treeTop);
            pos = GetScreenPos(Map.MapSize);
            DX.DrawGraph(0, (int)pos.Y - 90, treeBottom);

            ParticleManagerBottom.Draw();
            gameObjects.ForEach(obj =>
            {
                if (IsInScreen(obj.pos, 100))
                    obj.Draw();
            });
            player.Draw();

            Navi.Draw();

            ParticleManagerTop.Draw();

            DX.DrawGraph(550, 200, bar);
            DX.DrawGraph(555, 150, Flag);
            playerIcon.Draw();

            DX.DrawRotaGraph(100, 23, 0.6, 0, stageName);
            DX.DrawRotaGraph(Screen.Width / 2 - 22, 23, 0.6, 0, scoreImage);
            DX.DrawRotaGraph(Screen.Width - 155, 25, 0.55, 0, watch);
            DX.DrawRotaGraph(Screen.Width - 75, 25, 0.7, 0, colon);

            int a = 0;
            //スコア
            NumberDraw.ScoreDraw(score, Screen.Width / 2 - 10, 25, fontInterval, fontScale, "image_effect/time_", false, false);
            //タイム
            NumberDraw.TimeDraw(time, Screen.Width - 150, 25, "image_effect/time_", fontInterval, fontScale, true);
        }

        public override void OnExit()
        {
            Game.fadeAction = false;
        }

        public override void OnLoad()
        {
            IsGoal = false;
            score = 0;
        }
        public void Goal(Vec2f pos)
        {
            player.Goal(pos / 0.705f);

            IsGoal = true;

        }

        public override void Update()
        {
            if (!IsGoal)
            {
                time.Add(TimeSpan.FromMilliseconds(16.666666666f));
            }

            Navi.Update();

            gameObjects.ForEach(obj => player.CalcInteract(obj));
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

                if (goalTimer == 300)
                {
                    Game.bgmManager.Set(30, "result", "play");
                    Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.FadeOut);
                    Sound.Play("goal_jingle.mp3");
                }
                goalTimer--;
                if (!Game.fadeAction && goalTimer <= 0)
                {
                    Game.fadeAction = true;
                    Game.bgmManager.Set(60);
                    Game.bgmManager.update = new BgmManager.Update(Game.bgmManager.FadeIn);
                    Game.SetScene(new SceneResult(Game, this, score, time), new Fade(fadeTime, true, true));
                }
            }

        }
    }
}