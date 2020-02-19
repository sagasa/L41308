using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using SAGASALib;

namespace KUMALib
{
    public static class NumberDraw
    {
        //表示するスコア, 1文字目のx座標, y座標, 文字の間隔, 文字の拡縮, 使う画像の数字部分より前までのファイル名, ゼロ埋めするか(省略可), 左詰めするか(省略可)
        public static void ScoreDraw(int score, int x, int y, int interval, float imageScale, string fileName, bool zeroPadding = false, bool leftPadding = true)
        {
            int digit;
            if (leftPadding)//桁数を調べる
                digit = (score == 0) ? 1 : ((int)Math.Log10(score) + 1);
            else
                digit = 4;
            for (int i = 0; i < digit; i++)
            {
                if (zeroPadding || score / (int)Math.Pow(10, digit - 1 - i) > 0 || i == digit - 1)
                {
                    DX.DrawRotaGraph(x + interval * i, y, imageScale, 0, ResourceLoader.GetGraph(fileName + (score / (int)Math.Pow(10, digit - 1 - i) % 10) + ".png"));
                }
            }
        }

        //レフトカウンターの値を返す　new等の描画に使うため
        public static void ScoreDraw(int score, int x, int y, int interval, float imageScale, string fileName, ref int leftCounter, bool zeroPadding = false, bool leftPadding = true)
        {
            //桁数を調べる
            int digit;
            if (leftPadding)//桁数を調べる
                digit = (score == 0) ? 1 : ((int)Math.Log10(score) + 1);
            else
                digit = 4;
            for (leftCounter = 0; leftCounter < digit; leftCounter++)
            {
                if (zeroPadding || score / (int)Math.Pow(10, digit - 1 - leftCounter) > 0 || leftCounter == digit - 1)
                {
                    DX.DrawRotaGraph(x + interval * leftCounter, y, imageScale, 0, ResourceLoader.GetGraph(fileName + (score / (int)Math.Pow(10, digit - 1 - leftCounter) % 10) + ".png"));
                }
            }
        }

        //表示するタイム, 1文字目のx座標, y座標, 文字の間隔, 文字の拡縮, 使う画像の数字部分より前までのファイル名, ミリ秒を表示するか(省略可), ゼロ埋めするか(省略可), 左詰めするか(省略可)
        public static void TimeDraw(DateTime time, int x, int y, int interval, float imageScale, string fileName, bool milliDraw = false, bool zeroPadding = false, bool leftPadding = true)
        {
            int pointer = 0;

            void Draw(int time_, int digit_, bool zeroPadding_ = true)
            {
                for (int i = 0; i < digit_; i++, pointer++)
                {
                    if (zeroPadding_ || time_ / (int)Math.Pow(10, digit_ - 1 - i) > 0 || i == digit_ - 1)
                    {
                        DX.DrawRotaGraph(x + interval * pointer, y, imageScale, 0, ResourceLoader.GetGraph(fileName + (time_ / (int)Math.Pow(10, digit_ - 1 - i) % 10) + ".png"));
                    }
                }
            }

            void SymbolDraw(string symbol)
            {
                DX.DrawRotaGraph(x + interval * pointer, y, imageScale, 0, ResourceLoader.GetGraph(fileName + symbol + ".png"));
                pointer++;
            }

            int digit;
            if (leftPadding)//桁数を調べる
                digit = (time.Minute == 0) ? 1 : ((int)Math.Log10(time.Minute) + 1);
            else
                digit = 2;
            Draw(time.Minute, digit, zeroPadding);
            SymbolDraw("colon");
            Draw(time.Second, 2);
            if (milliDraw)
            {
                SymbolDraw("dot");
                Draw(time.Millisecond, 2);
            }
        }

        //レフトカウンターの値を返す　new等の描画に使うため
        public static void TimeDraw(DateTime time, int x, int y, int interval, float imageScale, string fileName, ref int leftCouner, bool milliDraw = false, bool zeroPadding = false, bool leftPadding = true)
        {
            int pointer = 0;

            void Draw(int time_, int digit_, bool zeroPadding_ = true)
            {
                for (int i = 0; i < digit_; i++, pointer++)
                {
                    if (zeroPadding_ || time_ / (int)Math.Pow(10, digit_ - 1 - i) > 0 || i == digit_ - 1)
                    {
                        DX.DrawRotaGraph(x + interval * pointer, y, imageScale, 0, ResourceLoader.GetGraph(fileName + (time_ / (int)Math.Pow(10, digit_ - 1 - i) % 10) + ".png"));
                    }
                }
            }

            void SymbolDraw(string symbol)
            {
                DX.DrawRotaGraph(x + interval * pointer, y, imageScale, 0, ResourceLoader.GetGraph(fileName + symbol + ".png"));
                pointer++;
            }

            int digit;
            if (leftPadding)//桁数を調べる
                digit = (time.Minute == 0) ? 1 : ((int)Math.Log10(time.Minute) + 1);
            else
                digit = 2;
            Draw(time.Minute, digit, zeroPadding);
            SymbolDraw("colon");
            Draw(time.Second, 2);
            if (milliDraw)
            {
                SymbolDraw("dot");
                Draw(time.Millisecond, 4);
            }
            leftCouner = pointer;
        }
        
        public static void TimeDraw(DateTime time, int x, int y,string name, int interval, float fontScale, bool zeroPadding = true)
        {
            int pointer = 0;

            void Draw(int num,int digit = -1)
            {
                //桁数算出
                int d = (num == 0) ? 1 : ((int) Math.Log10(num) + 1);
                //デフォ値orゼロ埋め無しなら上書き
                digit = digit == -1 || (!zeroPadding&&d<=digit) ? d : digit;

                pointer += interval * digit;
                //桁数が多いなら上から表示
                for (int i = 0; i < digit; i++)
                {
                    //0パディング
                    int n = d < i ? 0:num % 10;
                    num /= 10;

                    DX.DrawRotaGraph(x + pointer - interval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + n + ".png"));
                }
            }
            Draw(time.Minute,2);
            Draw(time.Second, 2);
            Draw(time.Millisecond, 4);
        }

        public static void DateDraw(DateTime time, int x, int y, int interval, float imageScale, string fileName, bool zeroPadding = false, bool leftPadding = true)
        {
            int pointer = 0;

            void Draw(int time_, int digit_, bool zeroPadding_ = true)
            {
                for (int i = 0; i < digit_; i++, pointer++)
                {
                    DX.DrawRotaGraph(x + interval * pointer, y, imageScale, 0, ResourceLoader.GetGraph(fileName + (time_ / (int)Math.Pow(10, digit_ - 1 - i) % 10) + ".png"));
                }
            }
            
            void SlashDraw()
            {
                DX.DrawRotaGraph(x + interval * pointer, y, imageScale, 0, ResourceLoader.GetGraph(fileName + "slash" + ".png"));
                pointer++;
            }
            
            Draw(time.Year % 100, 2);
            SlashDraw();
            Draw(time.Month, 2);
            SlashDraw();
            Draw(time.Day, 2);
        }
    }
}
