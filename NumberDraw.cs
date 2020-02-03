using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;
using SAGASALib;

namespace Giraffe
{
    public static class NumberDraw
    {
        //表示するスコア, 1文字目のx座標, y座標, 文字の間隔, 文字の拡縮, 使う画像の数字部分より前までのファイル名, ゼロ埋めするか(省略可), 左詰めするか(省略可)
        public static void ScoreDraw(int score, int x, int y, int interval, float fontScale, string name, bool zeroPadding = false, bool leftPadding = true)
        {
            //桁数を調べる
            int digit = 0;//桁数
            for (int i = 0, s = score; i < s; digit++)
            {
                s = s / 10;
            }
            if (zeroPadding && digit < 4)//ゼロ埋め
            {
                digit = 4;
            }
            //描画
            for (int i = 0, leftCounter = 0, unit = (int)Math.Pow(10, digit - 1); i < digit; i++, unit /= 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (leftPadding && j == score / unit % 10 && (zeroPadding || score >= unit))//左詰めするとき
                    {
                        DX.DrawRotaGraph(x + interval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    else if (!leftPadding && j == score / unit % 10 && (zeroPadding || score >= unit))//左詰めしないとき
                    {
                        DX.DrawRotaGraph(x + interval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
            }
        }
        
        //レフトカウンターの値を返す　new等の描画に使うため
        public static void ScoreDraw(int score, int x, int y, int xInterval, float fontScale, string name, ref int leftCounter, bool zeroPadding = false, bool leftPadding = true)
        {
            //桁数を調べる
            int digit = 0;//桁数
            for (int i = 0, s = score; i < s; digit++)
            {
                s = s / 10;
            }
            if (zeroPadding && digit < 4)//ゼロ埋め
            {
                digit = 4;
            }
            //描画
            leftCounter = 0;//左詰めに使用
            for (int i = 0, unit = (int)Math.Pow(10, digit - 1); i < digit; i++, unit /= 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (leftPadding && j == score / unit % 10 && (zeroPadding || score >= unit))//左詰めするとき
                    {
                        DX.DrawRotaGraph(x + xInterval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    else if (!leftPadding && j == score / unit % 10 && (zeroPadding || score >= unit))//左詰めしないとき
                    {
                        DX.DrawRotaGraph(x + xInterval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
            }
        }

        //表示するタイム, 1文字目のx座標, y座標, 文字の間隔, 文字の拡縮, 使う画像の数字部分より前までのファイル名, レフトカウンターの値を返す(コロン用), フレーム数を表示するか(省略可), ゼロ埋めするか(省略可), 左詰めするか(省略可)
        public static void TimeDraw(int[] time, int x, int y, int interval, float fontScale, string name, ref int leftCounter, bool fpsDraw = false, bool zeroPadding = false, bool leftPadding = true)
        {
            leftCounter = 0;
            for (int i = 0, digit = 2, unit = (int)Math.Pow(10, digit - 1); i < digit; i++, unit /= 10)
            {
                for (int j = 0; j < 10; j++)//分
                {
                    if (leftPadding && j == time[0] / unit % 10 && (zeroPadding || unit == 1 || time[0] >= unit))
                    {
                        DX.DrawRotaGraph(x + interval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    if (!leftPadding && j == time[0] / unit % 10 && (zeroPadding || unit == 1 || time[0] >= unit))
                    {
                        DX.DrawRotaGraph(x + interval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
                for (int j = 0; j < 10; j++)//秒
                {
                    if (leftPadding && j == time[1] / unit % 10)
                    {
                        DX.DrawRotaGraph(x + interval * (2 + leftCounter), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                    else if (!leftPadding && j == time[1] / unit % 10)
                    {
                        DX.DrawRotaGraph(x + interval * (3 + i), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
                if (fpsDraw)//フレーム数
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (leftPadding && j == time[2] / unit % 10)
                        {
                            DX.DrawRotaGraph(x + interval * (5 + leftCounter), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                            break;
                        }
                        else if (!leftPadding && j == time[2] / unit % 10)
                        {
                            DX.DrawRotaGraph(x + interval * (6 + i), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                            break;
                        }
                    }
                }
            }
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
    }
}
