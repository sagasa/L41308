using System;
using DxLibDLL;
using SAGASALib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    public class Tutolal : ScenePlay
    {

        public override Vec2f GetScreenPos(Vec2f mapPos)
        {
            return (mapPos - MapPos) * PlayMap.CellSize;
        }
        public new bool IsInScreen(Vec2f pos)
        {
            return pos.IsInBetween(MapPos, MapPos + PlayMap.ScreenSize);
        }
        public new PlayMap Map { get; private set; }
        public new Vec2f MapPos;
        public List<GameObject> GameObjects = new List<GameObject>();

        private Player player;
        int count = 0;
        int SousaCount = 0;
        int CommentTime = 120;



        private int head = ResourceLoader.GetGraph("player/player_head.png");
        private int horn = ResourceLoader.GetGraph("player/horn.png");
        private int eye = ResourceLoader.GetGraph("player/player_eye.png");
        private int ear = ResourceLoader.GetGraph("player/player_ear.png");
        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
        private int select1 = ResourceLoader.GetGraph("select_3.png");
        private int select2 = ResourceLoader.GetGraph("select_4.png");
        private int setumei = ResourceLoader.GetGraph("プレイ画面スクショ.jpg");
        private int bg = ResourceLoader.GetGraph("t.bg.png");
        private int window = ResourceLoader.GetGraph("ant1.png");
        private int playbg = ResourceLoader.GetGraph("play_bg.png");
        private int waku = ResourceLoader.GetGraph("81917.png");
        private int mes = ResourceLoader.GetGraph("mes11_02_.png");

        int y = 502;//キリンの頭の座標

        string[] GamenText = new string[]//画面説明用コメント
        {
          "画面説明です",
          "この生き物はキリン、このゲームの主人公。 ","プレーヤーはこのキャラを操作します",
          "これは木の枝です。プレイヤーは、","つかまって上に登っていくことができます",
          "スコアです。木の枝に噛みつく、ゴールまでに","かかった時間の速さ,などでスコアが増加します",
          "ミニマップです、ミニマップ上のキリンのアイコンは","マップ上のキリンの位置を表します",
          "タイマーです。スタートからゴールまでに","かかった時間がここに表示されます",
          "チュートリアル画面トップへ戻りますか？","戻る場合は決定ボタンを押してください"
        };

        string[] Gamennamae = new string[] //画面説明項目用コメント
        {
            "～画面説明～","・キリン","・木の枝",
            "・スコア", "・ミニマップ", "・タイマー", "・画面説明を終了"
        };

        string[] SousaText = new string[]//操作説明用コメント
        {
            "画面説明です",//1
            "十字キーの⇒を押すと、","プレイヤーは右の方向に移動します",//右,2,3
            "十字キーの⇐を押すと、","プレイヤーは左の方向に移動します",//左,4,5
            "地面に足がついている状態でスペース","キーを押すと、ジャンプをすることができます",//ジャンプ,6,7
            "スペースキーを押している状態で木の枝に触れ","ると、噛んでつかまることができます",//噛みつき,8,9
            "木の枝に噛みついているとき、","キリンは自動でぐるぐる回り始めます",//方向転換,10
            "回転の向きは、十字キーの左右で変えることができます",//方向転換,11
            "十字キーの上下を押すとキリンの首","が長くなったり縮んだりします",//首伸び縮み,12,13
            "うまく首の長さを調節して、","うまく木の枝を飛び移りましょう",//まとめ,14
            "※これにて操作説明のチュートリアルは終了です",//画面終了,15
            "チュートリアル画面トップへ戻ります",//16
        };


        public Tutolal(Game game) : base(game)
        {
            Map = new PlayMap(this, "map1_leaf");
            MapPos = new Vec2f(0, Map.MapSize.Y - PlayMap.ScreenSize.Y);

            player = new Player(this);
            player.pos = MapPos + new Vec2f(2, 2);
        }

        uint white = DX.GetColor(255, 255, 255);//白
        uint black = DX.GetColor(0, 0, 0);//黒

        public override void Draw()
        {

            if (count == 0)
            {
                DX.DrawGraph(0, 0, titlebg);
                DX.DrawGraph(105, 515, select1);
                DX.DrawGraph(105, 630, select2);
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

            if (count >= 1 && count < 99)
            {
                DX.SetFontSize(25);
                DX.SetFontThickness(100);
                DX.ChangeFontType(DX.DX_FONTTYPE_ANTIALIASING_EDGE);
                DX.DrawGraph(0, 0, bg);
                DX.DrawGraph(-20, 30, setumei);
                DX.DrawGraph(-10, 610, window);
                DX.DrawGraph(390, 30, waku);
                DX.DrawString(420, 70, Gamennamae[0], white);
                DX.DrawString(400, 140, Gamennamae[1], white);
                DX.DrawString(400, 200, Gamennamae[2], white);
                DX.DrawString(400, 260, Gamennamae[3], white);
                DX.DrawString(400, 320, Gamennamae[4], white);
                DX.DrawString(400, 380, Gamennamae[5], white);
                DX.DrawString(400, 440, Gamennamae[6], white);
            }
            if (count == 1)//画面説明(タイトル)
            {
                DX.DrawString(20, 640, GamenText[0], white);

            }
            if (count == 2)//画面説明(キリン)
            {
                DX.DrawString(20, 640, GamenText[1], white);
                DX.DrawString(20, 690, GamenText[2], white);
                DX.DrawLine(410, 170, 520, 170, black);
                DX.DrawBox(180, 460, 250, 560, black, DX.FALSE);
            }
            if (count == 3)//画面説明(木の枝)
            {
                DX.DrawString(10, 640, GamenText[3], white);
                DX.DrawString(10, 690, GamenText[4], white);
                DX.DrawLine(410, 230, 520, 230, black, DX.FALSE);
            }
            if (count == 4)//画面説明(スコア)
            {
                DX.DrawString(10, 640, GamenText[5], white);
                DX.DrawString(10, 690, GamenText[6], white);
                DX.DrawLine(410, 290, 520, 290, black, DX.FALSE);
            }
            if (count == 5)//画面説明(ミニマップ)
            {
                DX.DrawString(10, 640, GamenText[7], white);
                DX.DrawString(10, 690, GamenText[8], white);
                DX.DrawLine(410, 350, 570, 350, black, DX.FALSE);
            }
            if (count == 6)//画面説明(タイマー)
            {
                DX.DrawString(10, 640, GamenText[9], white);
                DX.DrawString(10, 690, GamenText[10], white);
                DX.DrawLine(410, 410, 540, 410, black, DX.FALSE);
            }
            if (count == 7)//画面説明(修了)
            {
                DX.DrawString(10, 640, GamenText[11], white);
                DX.DrawString(10, 690, GamenText[12], white);
                DX.DrawLine(410, 470, 620, 470, black, DX.FALSE);
            }
            if (count >= 99)//操作画面(タイトル)
            {
                DX.DrawGraph(0, 0, playbg);
                Vec2f pos = GetScreenPos(Vec2f.ZERO);
                DX.DrawGraph((int)pos.X, (int)pos.Y, playbg);
                gameObjects.ForEach(obj => obj.Draw());
                player.Draw();
                DX.DrawGraph(-90, 0, mes);

                if (count >= 99 && count <= 100)//右
                {
                    DX.DrawString(100, 30, SousaText[1], black);
                    DX.DrawString(100, 60, SousaText[2], black);
                }
                if (count >= 101 && count <= 102)//左
                {
                    DX.DrawString(100, 30, SousaText[3], black);
                    DX.DrawString(100, 60, SousaText[4], black);
                }
                if (count >= 103 && count <= 104)//ジャンプ
                {
                    DX.DrawString(100, 30, SousaText[5], black);
                    DX.DrawString(100, 60, SousaText[6], black);
                }
                if (count >= 105 && count <= 106)//噛みつき
                {
                    DX.DrawString(100, 30, SousaText[7], black);
                    DX.DrawString(100, 60, SousaText[8], black);
                }
                if (count >= 107 && count <= 108)//方向転換
                {
                    DX.DrawString(100, 30, SousaText[9], black);
                    DX.DrawString(100, 60, SousaText[10], black);
                    DX.DrawString(100, 90, SousaText[11], black);

                }
                if (count >= 109 && count <= 110)//首伸び縮み
                {
                    DX.DrawString(100, 30, SousaText[12], black);
                    DX.DrawString(100, 60, SousaText[13], black);
                }
                if (count == 111)
                {
                    DX.DrawString(100, 30, SousaText[15], black);
                    DX.DrawString(100, 60, SousaText[16], black);
                }
            }

            DX.DrawString(200, 300, "" + count, black);//カウント表示
            DX.DrawString(200, 350, "" + SousaCount, black);//操作進行表示
            DX.DrawString(200, 400, "" + player.Y, black);//プレイヤーの縦座標表示
            DX.DrawString(200, 450, "" + CommentTime, black);//コメント表示時間の表示
        }

        public override void OnExit()
        {

        }

        public override void OnLoad()
        {

        }

        public override void Update()
        {

            if (count == 0 && Input.BACK.IsPush())
            {
                Game.SetScene(new Title(Game));
            }
            if (y == 617 && Input.ACTION.IsPush())
            {
                count += 1;
                y = 0;
            }
            else if (y == 502 && Input.ACTION.IsPush())
            {
                count += 99;
                y = 0;
            }
            if (y == 0 && Input.DOWN.IsPush())
            {
                if (count >= 99)
                {

                }
                else
                {
                    count += 1;
                }

            }
            else if (y == 0 && Input.UP.IsPush())
            {
                if (count <= 2)
                {
                    count += 0;
                }
                else if (count >= 99)
                {
                    count += 0;
                }
                else
                {
                    count -= 1;
                }
            }
            if (count >= 7 && count <= 10)
            {
                count = 7;
            }
            else if (count <= 98 && count >= 11)
            {
                count = 99;
            }

            if (count == 7 && Input.ACTION.IsPush())
            {
                count = 0;
                y = 502;
            }

            if (count >= 99)//操作画面トップ
            {
                gameObjects.ForEach(obj => player.CalcInteract(obj));
                player.Update();
                gameObjects.ForEach(obj => obj.Update());
                gameObjects.RemoveAll(obj => obj.IsDead());


                if (Input.BACK.IsPush())//Xボタンで戻ります
                {
                    count = 0;
                    y = 502;
                }
                if (SousaCount == 100)
                {
                    count += 1;
                    SousaCount = 0;
                }
                if (count == 99)
                {
                    CommentTime--;
                    if (CommentTime <= 0)
                    {
                        count += 1;
                        CommentTime = 120;
                    }
                }
                if (count == 100)//右移動(地面に足がついてるとき)
                {
                    if (Input.RIGHT.IsHold() && player.IsOnGround())
                    {
                        SousaCount += 1;
                    }
                }
                if (count == 101)
                {
                    CommentTime--;
                    if (CommentTime <= 0)
                    {
                        count += 1;
                        CommentTime = 120;
                    }
                }
                if (count == 102)//左移動(地面に足がついているとき)
                {
                    if (Input.LEFT.IsHold() && player.IsOnGround())
                    {
                        SousaCount += 1;
                    }
                }
                if (count == 103)
                {
                    CommentTime--;
                    if (CommentTime <= 0)
                    {
                        count += 1;
                        CommentTime = 120;
                    }
                }
                if (count == 104)//ジャンプ
                {
                    if (Input.ACTION.IsPush())
                    {
                        SousaCount += 50;
                    }
                }
                if (count == 105)
                {
                    CommentTime--;
                    if (CommentTime <= 0)
                    {
                        count += 1;
                        CommentTime = 120;
                    }
                }
                if (count == 106)//木につかまる
                {
                    if (player.Y <= 21 && player.Y >= 18 && Input.ACTION.IsHold())
                    {
                        SousaCount += 1;
                    }
                }
                if (count == 107)
                {
                    CommentTime--;
                    if (CommentTime <= 0)
                    {
                        count += 1;
                        CommentTime = 120;
                    }
                }
                if (count == 108)//回転方向変更
                {
                    if (Input.ACTION.IsHold() && Input.RIGHT.IsPush() || Input.LEFT.IsPush())
                    {
                        SousaCount = +100;
                    }
                }
                if (count == 109)
                {
                    CommentTime--;
                    if (CommentTime <= 0)
                    {
                        count += 1;
                        CommentTime = 120;
                    }
                }
                if (count == 110)//首伸ばし
                {
                    if (Input.UP.IsHold() || Input.DOWN.IsHold())
                    {
                        SousaCount += 1;
                    }

                }
                if (count == 111)//説明終了
                {
                    CommentTime--;
                    if (CommentTime <= 0)
                    {
                        count = 0;
                        y = 502;
                    }
                }


            }
        }
    }
}

