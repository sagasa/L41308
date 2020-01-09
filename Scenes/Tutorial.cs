using System;
using DxLibDLL;
using SAGASALib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giraffe
{
    public class Tutolal : Scene
    {

        int count = 0;
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

        private int waku = ResourceLoader.GetGraph("81917.png");
        int y = 502;
        string[] text = new string[]
        {
          "画面説明です",
          "この生き物はキリン、このゲームの主人公。 ","プレーヤーはこのキャラを操作します",
          "これは木の枝ですプレイヤーは、","つかまって上に登っていくことができます",
          "スコアです。木の枝に噛みつく、ゴールまでに","かかった時間の速さ,などでスコアが増加します",
          "ミニマップです、ミニマップ上のキリンのアイコンは","マップ上のキリンの位置を表します",
          "タイマーです。スタートからゴールまでに","かかった時間がここに表示されます",
          "チュートリアル画面トップへ戻りますか？","戻る場合は決定ボタンを押してください"
        };

        string[] namae = new string[] { "～画面説明～","・キリン","・木の枝",
            "・スコア", "・ミニマップ", "・タイマー", "・画面説明を終了" };

        public Tutolal(Game game) : base(game)
        {
        }

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

            if (count >= 1 && count < 100)
            {
                DX.SetFontSize(25);
                DX.SetFontThickness(100);
                DX.ChangeFontType(DX.DX_FONTTYPE_ANTIALIASING_EDGE);
                DX.DrawGraph(0, 0, bg);
                DX.DrawGraph(-20, 30, setumei);
                DX.DrawGraph(-10, 610, window);
                DX.DrawGraph(390, 30, waku);
                DX.DrawString(420, 70, namae[0], DX.GetColor(255, 255, 255));
                DX.DrawString(400, 140, namae[1], DX.GetColor(255, 255, 255));
                DX.DrawString(400, 200, namae[2], DX.GetColor(255, 255, 255));
                DX.DrawString(400, 260, namae[3], DX.GetColor(255, 255, 255));
                DX.DrawString(400, 320, namae[4], DX.GetColor(255, 255, 255));
                DX.DrawString(400, 380, namae[5], DX.GetColor(255, 255, 255));
                DX.DrawString(400, 440, namae[6], DX.GetColor(255, 255, 255));
            }
            if (count == 1)
            {
                DX.DrawString(20, 640, text[0], DX.GetColor(255, 255, 255));

            }
            if (count == 2)
            {
                DX.DrawString(10, 640, text[1], DX.GetColor(255, 255, 255));
                DX.DrawString(10, 690, text[2], DX.GetColor(255, 255, 255));
                DX.DrawLine(410, 170, 520, 170, DX.GetColor(0, 0, 0));
                DX.DrawBox(180, 460, 250, 560, DX.GetColor(0, 0, 0), DX.FALSE);
            }
            if (count == 3)
            {
                DX.DrawString(10, 640, text[3], DX.GetColor(255, 255, 255));
                DX.DrawString(10, 690, text[4], DX.GetColor(255, 255, 255));
                DX.DrawLine(410, 230, 520, 230, DX.GetColor(0, 0, 0), DX.FALSE);
            }
            if (count == 4)
            {
                DX.DrawString(10, 640, text[5], DX.GetColor(255, 255, 255));
                DX.DrawString(10, 690, text[6], DX.GetColor(255, 255, 255));
                DX.DrawLine(410, 290, 520, 290, DX.GetColor(0, 0, 0), DX.FALSE);
            }
            if (count == 5)
            {
                DX.DrawString(10, 640, text[7], DX.GetColor(255, 255, 255));
                DX.DrawString(10, 690, text[8], DX.GetColor(255, 255, 255));
                DX.DrawLine(410, 350, 570, 350, DX.GetColor(0, 0, 0), DX.FALSE);
            }
            if (count == 6)
            {
                DX.DrawString(10, 640, text[9], DX.GetColor(255, 255, 255));
                DX.DrawString(10, 690, text[10], DX.GetColor(255, 255, 255));
                DX.DrawLine(410, 410, 540, 410, DX.GetColor(0, 0, 0), DX.FALSE);
            }
            if (count == 7)
            {
                DX.DrawString(10, 640, text[11], DX.GetColor(255, 255, 255));
                DX.DrawString(10, 690, text[12], DX.GetColor(255, 255, 255));
                DX.DrawLine(410, 470, 620, 470, DX.GetColor(0, 0, 0), DX.FALSE);
            }
            if (count >= 100)
            {

            }

        }

        public override void OnExit()
        {

        }

        public override void OnLoad()
        {

        }

        public override void Update()
        {
            if (y == 617 && Input.ACTION.IsPush())
            {
                count += 1;
                y = 0;
            }
            else if (y == 502 && Input.ACTION.IsPush())
            {
                count += 100;
                y = 0;
            }
            if (y == 0 && Input.DOWN.IsPush())
            {
                count += 1;
            }
            else if (y == 0 && Input.UP.IsPush())
            {
                count -= 1;
            }
            if (count >= 7 && count <= 99)
            {
                count = 7;
            }

            if (count == 7 && Input.ACTION.IsPush())
            {
                count = 0;
                y = 502;
            }
        }
    }
}
