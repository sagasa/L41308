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
                    if (leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めするとき
                    {
                        DX.DrawRotaGraph(x + interval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    else if (!leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めしないとき
                    {
                        DX.DrawRotaGraph(x + interval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
            }
        }
        
        //レフトカウントにレフトカウンターの値を返す　new等の描画に使うため
        public static void ScoreDraw(int score, int x, int y, int xInterval, float fontScale, string name, ref int leftCount, bool zeroPadding = false, bool leftPadding = true)
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
            int leftCounter = 0;//左詰めに使用
            for (int i = 0, unit = (int)Math.Pow(10, digit - 1); i < digit; i++, unit /= 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めするとき
                    {
                        DX.DrawRotaGraph(x + xInterval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    else if (!leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めしないとき
                    {
                        DX.DrawRotaGraph(x + xInterval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
            }
            leftCount = leftCounter;//レフトカウンターを返す
        }

        //表示するタイム, 1文字目のx座標, y座標, 文字の間隔, 文字の拡縮, 使う画像の数字部分より前までのファイル名, レフトカウンターの値を返す(コロン用), フレーム数を表示するか(省略可), ゼロ埋めするか(省略可), 左詰めするか(省略可)
        public static void TimeDraw(int[] time, int x, int y, int interval, float fontScale, string name, ref int leftCount, bool fpsDraw = false, bool zeroPadding = false, bool leftPadding = true)
        {
            int leftCounter = 0;
            for (int i = 0, digit = 2, unit = (int)Math.Pow(10, digit - 1); i < digit; i++, unit /= 10)
            {
                for (int j = 0; j < 10; j++)//分
                {
                    if (leftPadding && j == time[0] / unit % 10 && (zeroPadding || unit == 1 || time[0] / unit != 0))
                    {
                        DX.DrawRotaGraph(x + interval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    else if (!leftPadding && j == time[0] / unit % 10 && (zeroPadding || unit == 1 || time[0] / unit != 0))
                    {
                        DX.DrawRotaGraph(x + interval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
                for (int j = 0; j < 10; j++)//秒
                {
                    if (leftPadding && j == time[1] / digit % 10)
                    {
                        DX.DrawRotaGraph(x + interval * (2 + leftCounter), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                    else if (!leftPadding && j == time[1] / digit % 10)
                    {
                        DX.DrawRotaGraph(x + interval * (2 + i), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
                if(fpsDraw)//フレーム数
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (leftPadding && j == time[2] / digit % 10)
                        {
                            DX.DrawRotaGraph(x + interval * (5 + leftCounter), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                            break;
                        }
                        else if (!leftPadding && j == time[2] / digit % 10)
                        {
                            DX.DrawRotaGraph(x + interval * (5 + i), y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                            break;
                        }
                    }
                }
            }
            leftCount = leftCounter;
        }
        
    }
}
