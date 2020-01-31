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
        //表示するスコア, 1文字目のx座標, 1文字目のy座標, 文字の間隔, 文字の拡縮, 使う画像の数字部分より前までのファイル名, ゼロ埋めするか(省略可能), 左詰めするか(省略可能)
        public static void ScoreDraw(int score, int x, int y, int interval, float fontScale, string name, bool zeroPadding = false, bool leftPadding = true)
        {
            //桁数を調べる
            int digit = 0;//桁数
            for (int i = 0, s = score; i < s; digit++)
            {
                s = s / 10;
            }
            //描画
            for (int i = 0, leftCounter = 0, unit = (int)System.Math.Pow(10, digit - 1); i < digit; i++, unit /= 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めするとき
                    {
                        DX.DrawRotaGraph(x + interval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    if (!leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めしないとき
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
            //描画
            int leftCounter = 0;//左詰めに使用
            for (int i = 0, unit = (int)System.Math.Pow(10, digit - 1); i < digit; i++, unit /= 10)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めするとき
                    {
                        DX.DrawRotaGraph(x + xInterval * leftCounter, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        leftCounter++;
                        break;
                    }
                    if (!leftPadding && j == score / unit % 10 && (zeroPadding || score / unit != 0))//左詰めしないとき
                    {
                        DX.DrawRotaGraph(x + xInterval * i, y, fontScale, 0, ResourceLoader.GetGraph(name + j + ".png"));
                        break;
                    }
                }
            }
            leftCount = leftCounter;//レフトカウンターを返す
        }


    }
}
