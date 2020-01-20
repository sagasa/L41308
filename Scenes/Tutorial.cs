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
        int Sounsa;

        

        private int head = ResourceLoader.GetGraph("player/player_head.png");
        private int horn = ResourceLoader.GetGraph("player/horn.png");
        private int eye = ResourceLoader.GetGraph("player/player_eye.png");
        private int ear = ResourceLoader.GetGraph("player/player_ear.png");
        private int titlebg = ResourceLoader.GetGraph("title_bg.png");
        private int select1 = ResourceLoader.GetGraph("select_3.png");
        private int select2 = ResourceLoader.GetGraph("select_4.png");
        private int setumei = ResourceLoader.GetGraph("プレイ画面スクショ.jpg");
        private int window = ResourceLoader.GetGraph("mes11_02.png");
        private int playbg = ResourceLoader.GetGraph("play_bg.png");
        private int waku = ResourceLoader.GetGraph("waku.png");
        private int mes = ResourceLoader.GetGraph("mes11_02_.png");
        private int stagename2 = ResourceLoader.GetGraph("image_play/stagename_0.png");

        int y = 502;//キリンの頭の座標

        string[] GamenText = new string[]//画面説明用コメント
        {
          "画面説明です",//画面説明,1
          "この生き物はキリン、このゲームの主人公。 ","プレーヤーはこのキャラを操作します",//キリン,2.3
          "これは木の枝です。プレイヤーは、","つかまって上に登っていくことができます",//木の枝,4,5
          "ステージ名です。ここに今遊んでいる","ステージの名前が表示されます",//ステージ名,6,7
          "スコアです。木の枝に噛みつく、ゴール","までにかかった時間の速さなどで","スコアが増加します",//8,8,10
          "ミニマップです、ミニマップ上のキリンの","アイコンは,マップ上のキリンの","位置を表します",//11,12,13
          "タイマーです。スタートからゴールまでに","かかった時間がここに表示されます",//14,15
          "チュートリアル画面トップへ戻りますか？","戻る場合は決定ボタンを押してください"//16,17
        };

        string[] Gamennamae = new string[] //画面説明項目用コメント
        {
            "～画面説明～","・キリン","・木の枝","・ステージ名",
            "・スコア", "・ミニマップ", "・タイマー", "・画面説明を終了"
        };

        string[] SousaText = new string[]//操作説明用コメント
        {
            "画面説明です",//1
            "十字キーの⇒を押すと、","プレイヤーは右の方向に移動します",//右,2,3
            "十字キーの⇐を押すと、","プレイヤーは左の方向に移動します",//左,4,5
            "地面に足がついている状態でスペースキーを押すと、","ジャンプをすることができます",//ジャンプ,6,7
            "スペースキーを押している状態で木の枝に触れると、","噛んでつかまることができます",//噛みつき,8,9
            "木の枝に","噛みついているとき、キリンは自動でぐるぐる回り始めます",//方向転換,10
            "回転の向きは、十字キーの左右で変えることができます!",//方向転換,11
            "十字キーの上下を押すとキリンの首が","長くなったり縮んだりします",//首伸び縮み,12,13
            "うまく首の長さを調節して、","うまく木の枝を飛び移りましょう",//まとめ,14
            "※これにて操作説明のチュートリアルは終了です",//画面終了,15
            "チュートリアル画面トップへ戻ります",//戻る,16
            "それではやってみましょう",//宣告,17
            "OK!",//OK!,18
            "操作ゲージ",//やってみよう,19
            "それでは右に移動してみましょう",//20
            "それでは左に移動してみましょう",//21
            "それでは2回ジャンプしてみましょう",//22
            "それでは木に噛みついてみましょう",//23
            "では首を伸ばしてリ縮めたりしてみましょう",//24
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
        uint green = DX.GetColor(0, 200, 0);

        public override void Draw()
        {
            Sounsa = SousaCount;
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
               
                DX.DrawGraph(0, 30, setumei);
                DX.DrawGraph(-150, 615, window);
                DX.DrawGraph(390, 30, waku);
                DX.DrawString(420, 75, Gamennamae[0], white);
                DX.DrawString(400, 145, Gamennamae[1], white);
                DX.DrawString(400, 210, Gamennamae[2], white);
                DX.DrawString(400, 277, Gamennamae[3], white);
                DX.DrawString(400, 342, Gamennamae[4], white);
                DX.DrawString(400, 410, Gamennamae[5], white);
                DX.DrawString(400, 475, Gamennamae[6], white);
                DX.DrawString(400, 540, Gamennamae[7], white);
                DX.DrawGraph(0, -20, stagename2);
            }
            if (count == 1)//画面説明(タイトル)
            {
                DX.DrawString(50, 638, GamenText[0], white);

            }
            if (count == 2)//画面説明(キリン)
            {
                DX.DrawString(50, 638, GamenText[1], white);
                DX.DrawString(50, 674, GamenText[2], white);
                DX.DrawBox(410, 172, 520, 174, black,DX.TRUE);
            }
            if (count == 3)//画面説明(木の枝)
            {
                DX.DrawString(50, 638, GamenText[3], white);
                DX.DrawString(50, 674, GamenText[4], white);
                DX.DrawBox(410, 237, 520, 239, black, DX.TRUE);
            }
            if (count == 4)//画面説明(ステージ名)
            {
                DX.DrawString(50, 638, GamenText[5], white);
                DX.DrawString(50, 674, GamenText[6], white);
                DX.DrawBox(410, 304, 570, 306, black, DX.TRUE);
            }
            if (count == 5)//画面説明(スコア)
            {
                DX.DrawString(50, 638, GamenText[7], white);
                DX.DrawString(50, 674, GamenText[8], white);
                DX.DrawString(50, 705, GamenText[9], white);
                DX.DrawBox(410, 370, 520, 372, black, DX.TRUE);
            }
            if (count == 6)//画面説明(ミニマップ)
            {
                DX.DrawString(50, 638, GamenText[10], white);
                DX.DrawString(50, 674, GamenText[11], white);
                DX.DrawString(50, 705, GamenText[12], white);
                DX.DrawBox(410, 437, 570, 439, black, DX.TRUE);
            }
            if (count == 7)//画面説明(タイマー)
            {
                DX.DrawString(50, 638, GamenText[13], white);
                DX.DrawString(50, 674, GamenText[14], white);
                DX.DrawBox(410, 502, 540, 504, black, DX.TRUE);
            }
            if(count==8)//画面説明(修了)
            {
                DX.DrawString(50, 638, GamenText[15], white);
                DX.DrawString(50, 674, GamenText[16], white);
                DX.DrawBox(410, 568, 620, 570, black, DX.TRUE);
            }
            if (count >= 99)//操作画面(タイトル)
            {
                DX.SetFontSize(15);
                DX.ChangeFontType(DX.DX_FONTTYPE_NORMAL);
                DX.DrawGraph(0, 0, playbg);
                Vec2f pos = GetScreenPos(Vec2f.ZERO);
                DX.DrawGraph((int)pos.X, (int)pos.Y, playbg);
                gameObjects.ForEach(obj => obj.Draw());
                player.Draw();
                DX.DrawGraph(-90, 0, mes);
                DX.DrawBox(100, 135, 100 + Sounsa * 5 / 2, 160, green, DX.TRUE);//進行ゲージのゲージ
                DX.DrawString(580, 120, SousaText[19], black);//OK!
                DX.DrawBox(98, 133, 602, 162, black, DX.FALSE);//進行ゲージ外枠
                DX.DrawString(110, 115, SousaText[20], black);
                DX.DrawGraph(0, 160, stagename2);

                if (count >= 99 && count <= 100)//右
                {
                    DX.DrawString(100, 30, SousaText[1], black);
                    if (CommentTime <= 60 || CommentTime == 120)
                    {
                        DX.DrawString(100, 60, SousaText[2], black);
                    }
                    if (CommentTime == 120)
                    {
                        DX.DrawString(100, 90, SousaText[21], black);
                    }

                }
                if (count >= 101 && count <= 102)//左
                {
                    DX.DrawString(100, 30, SousaText[3], black);
                    if (CommentTime <= 60 || CommentTime == 120)
                    {
                        DX.DrawString(100, 60, SousaText[4], black);
                    }
                    if ( CommentTime == 120)
                    {
                        DX.DrawString(100, 90, SousaText[22], black);
                    }
                }
                if (count >= 103 && count <= 104)//ジャンプ
                {
                    DX.DrawString(100, 30, SousaText[5], black);
                    if (CommentTime <= 60 || CommentTime == 120)
                    {
                        DX.DrawString(100, 60, SousaText[6], black);
                    }
                    if ( CommentTime == 120)
                    {
                        DX.DrawString(100, 90, SousaText[23], black);
                    }
                }
                if (count >= 105 && count <= 106)//噛みつき
                {
                    DX.DrawString(100, 30, SousaText[7], black);
                    if (CommentTime <= 60 || CommentTime == 120)
                    {
                        DX.DrawString(100, 60, SousaText[8], black);
                    }
                    if (CommentTime == 120)
                    {
                        DX.DrawString(100, 90, SousaText[24], black);
                    }
                }
                if (count >= 107 && count <= 108)//方向転換
                {
                     DX.DrawString(100, 30, SousaText[10], black);
                    if (CommentTime <= 80 || CommentTime == 120)
                    {
                        DX.DrawString(100, 60, SousaText[11], black);
                    }
                   
                    if (CommentTime == 120)
                    {
                        DX.DrawString(100, 90, SousaText[18], black);
                    }
                }
                if (count >= 109 && count <= 110)//首伸び縮み
                {
                    DX.DrawString(100, 30, SousaText[12], black);
                    if (CommentTime <= 60 || CommentTime == 120)
                    {
                        DX.DrawString(100, 60, SousaText[13], black);
                    }
                    if (CommentTime == 120)
                    {
                        DX.DrawString(100, 90, SousaText[25], black);
                    }
                }
                if (count == 111)
                {
                    DX.DrawBox(100, 135, 100 + Sounsa * 5 / 2, 160, green, DX.TRUE);
                    DX.DrawString(580, 120, SousaText[19], black);
                    DX.DrawString(100, 30, SousaText[15], black);
                    if (CommentTime <= 60 || CommentTime == 120)
                    {
                        DX.DrawString(100, 60, SousaText[16], black);
                    }
                    if ( CommentTime == 120)
                    {
                        DX.DrawString(100, 90, SousaText[18], black);
                    }
                }
            }
            DX.ChangeFontType(DX.DX_FONTTYPE_NORMAL);
            DX.DrawString(200, 300, "count:" + count, black);//カウント表示
            DX.DrawString(200, 350, "SounsaCount:" + SousaCount, black);//操作進行表示
            DX.DrawString(200, 400, "player.Y:" + player.Y, black);//プレイヤーの縦座標表示
            DX.DrawString(200, 450, "CommentTime:" + CommentTime, black);//コメント表示時間の表示
            DX.DrawString(200, 500, "velangle" + player.velAngle, black);
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
            if (count >= 8 && count <= 10)
            {
                count = 8;
            }
            else if (count <= 98 && count >= 11)
            {
                count = 99;
            }

            if (count == 8 && Input.ACTION.IsPush())
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

                if (CommentTime <= 0)
                {
                    count += 1;
                    CommentTime = 120;
                }
                if (Input.BACK.IsPush())//Xボタンで戻ります
                {
                    count = 0;
                    y = 502;
                }
                if (SousaCount == 200)
                {
                    count += 1;
                    SousaCount = 0;
                }
                if (count == 99)
                {
                    CommentTime--;
                }
                if (count == 100)//右移動(地面に足がついてるとき)
                {
                    if (Input.RIGHT.IsHold() && player.IsOnGround())
                    {
                        SousaCount += 20;
                    }
                }
                if (count == 101)
                {
                    CommentTime--;
                   
                }
                if (count == 102)//左移動(地面に足がついているとき)
                {
                    if (Input.LEFT.IsHold() && player.IsOnGround())
                    {
                        SousaCount +=20;
                    }
                }
                if (count == 103)
                {
                    CommentTime--;
                   
                }
                if (count == 104)//ジャンプ
                {
                    if (Input.ACTION.IsPush())
                    {
                        SousaCount += 100;
                    }
                }
                if (count == 105)
                {
                    CommentTime--;
                 
                }
                if (count == 106)//木につかまる
                {
                    if (player.velAngle>0||player.velAngle<0&& Input.ACTION.IsHold())
                    {
                        SousaCount += 2;
                    }
                }
                if (count == 107)
                {
                    CommentTime--;
                   
                }
                if (count == 108)//回転方向変更
                {
                    if (player.velAngle<0&& Input.LEFT.IsPush()&&Input.ACTION.IsHold())
                    {
                        SousaCount = +200;
                    }
                    else if(player.velAngle>0&&Input.RIGHT.IsPush()&&Input.ACTION.IsHold())
                    {
                        SousaCount = +200;
                    }
                }
                if (count == 109)
                {
                    CommentTime--;
                    
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

