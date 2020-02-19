using System;
using System.Collections.Generic;
using DxLibDLL;
using SAGASALib;
using KUMALib;

namespace Giraffe.Saves
{
    public class HightScore
    {
        public class Entry
        {
            public string name;
            public int score;
            public long timeBinary;
            public long dateBinary;
        }

        public Dictionary<int, List<Entry>> scoreRankings = new Dictionary<int, List<Entry>> { };
        public Dictionary<int, List<Entry>> timeRankings = new Dictionary<int, List<Entry>> { };

        public void RankingInit()
        {
            for (int i = 1; i <= 3; i++)
            {
                scoreRankings[i] = new List<Entry> { };
                timeRankings[i] = new List<Entry> { };
                for (int j = 0; j < 5; j++)
                {
                    scoreRankings[i].Add(new Entry());
                    timeRankings[i].Add(new Entry());
                    scoreRankings[i][j].name = "No_data";
                    scoreRankings[i][j].score = 0;
                    scoreRankings[i][j].timeBinary = DateTime.MinValue.ToBinary();
                    scoreRankings[i][j].dateBinary = DateTime.MinValue.ToBinary();
                    timeRankings[i][j].name = "No_data";
                    timeRankings[i][j].score = 0;
                    timeRankings[i][j].timeBinary = DateTime.MaxValue.ToBinary();
                    timeRankings[i][j].dateBinary = DateTime.MinValue.ToBinary();
                }
            }
        }
        //ソート
        public void RankingSort(Entry entry, int stageNum)
        {
            //5番を追加、データ代入
            scoreRankings[stageNum].Add(new Entry());
            timeRankings[stageNum].Add(new Entry());
            scoreRankings[stageNum][5] = entry;
            timeRankings[stageNum][5] = entry;
            //ソート
            for (int i = 0; i < 5; i++)
            {
                //スコアのソート
                if (scoreRankings[stageNum][i].score <= scoreRankings[stageNum][5].score)
                {
                    Entry w = scoreRankings[stageNum][i];
                    scoreRankings[stageNum][i] = scoreRankings[stageNum][5];
                    scoreRankings[stageNum][5] = w;
                }
                //タイム
                if (DateTime.FromBinary(timeRankings[stageNum][i].timeBinary) >= DateTime.FromBinary(scoreRankings[stageNum][5].timeBinary))
                {
                    Entry w = timeRankings[stageNum][i];
                    timeRankings[stageNum][i] = scoreRankings[stageNum][5];
                    scoreRankings[stageNum][5] = w;
                }
            }
            //5番を削除
            scoreRankings[stageNum].RemoveAt(5);
            timeRankings[stageNum].RemoveAt(5);
        }
        //順位を返す
        public void RankingSort(Entry entry, int stageNum, ref int scoreRank, ref int timeRank)
        {
            //5番を追加、データ代入
            scoreRankings[stageNum].Add(new Entry());
            timeRankings[stageNum].Add(new Entry());
            scoreRankings[stageNum][5] = entry;
            timeRankings[stageNum][5] = entry;
            scoreRank = 10;
            timeRank = 10;
            //ソート
            for (int i = 0; i < 5; i++)
            {
                //スコアのソート
                if (scoreRankings[stageNum][i].score <= scoreRankings[stageNum][5].score)
                {
                    if (scoreRank == 10)
                        scoreRank = i;
                    Entry w = scoreRankings[stageNum][i];
                    scoreRankings[stageNum][i] = scoreRankings[stageNum][5];
                    scoreRankings[stageNum][5] = w;
                }
                //タイム
                if (DateTime.FromBinary(timeRankings[stageNum][i].timeBinary) >= DateTime.FromBinary(scoreRankings[stageNum][5].timeBinary))
                {
                    if (timeRank == 10)
                        timeRank = i;
                    Entry w = timeRankings[stageNum][i];
                    timeRankings[stageNum][i] = scoreRankings[stageNum][5];
                    scoreRankings[stageNum][5] = w;
                }
            }
            //5番を削除
            scoreRankings[stageNum].RemoveAt(5);
            timeRankings[stageNum].RemoveAt(5);
        }

        //記録更新を確認
        public bool BreakRecord(Entry entry, int stageNum)
        {
            if (entry.score >= scoreRankings[stageNum][4].score)
                return true;
            if (DateTime.FromBinary(entry.timeBinary) <= DateTime.FromBinary(timeRankings[stageNum][4].timeBinary))
                return true;
            return false;
        }

        public void ScoreRankingDraw(int stageNum, int rank, int x = 0)
        {
            const string image = "image_result/num_";
            const int rankX = 80;
            const int nameX = rankX + 50;
            const int dateX = nameX + 10;
            const int scoreX = nameX + 300;
            const int timeX = scoreX;
            const int line1 = 130;
            const int line2 = line1 + 50;
            const int nameY = line1 - 15;
            const int fontScale = 30;
            const float imageScale = 0.18f;
            const int heightInterval = 110;
            const int widthInterval = 25;
            DX.SetFontSize(fontScale);
            for (int i = 0; i < 5; i++)
            {
                DX.DrawRotaGraph(x+rankX, line1 + heightInterval * i, imageScale, 0, ResourceLoader.GetGraph(image + (i + 1) + ".png"));
                if (i == rank)//プレイヤーの色だけ違う色に
                    DX.DrawString(x+nameX, nameY + heightInterval * i, scoreRankings[stageNum][i].name, DX.GetColor(255, 131, 0));
                else
                    DX.DrawString(x+nameX, nameY + heightInterval * i, scoreRankings[stageNum][i].name, DX.GetColor(63, 42, 11));
                NumberDraw.ScoreDraw(scoreRankings[stageNum][i].score, x+scoreX, line1 + heightInterval * i, widthInterval, imageScale, image);
                NumberDraw.TimeDraw(DateTime.FromBinary(scoreRankings[stageNum][i].timeBinary), x+timeX, line2 + heightInterval * i, widthInterval, imageScale, image);
                NumberDraw.DateDraw(DateTime.FromBinary(scoreRankings[stageNum][i].dateBinary), x+dateX, line2 + heightInterval * i, widthInterval, imageScale, image);
            }
        }

        public void TimeRankingDraw(int stageNum, int rank, int x = 0)
        {
            const string image = "image_result/num_";
            const int rankX = 80;
            const int nameX = rankX + 50;
            const int dateX = nameX + 10;
            const int scoreX = nameX + 300;
            const int timeX = scoreX;
            const int line1 = 130;
            const int line2 = line1 + 50;
            const int nameY = line1 - 15;
            const int fontScale = 30;
            const float imageScale = 0.18f;
            const int heightInterval = 110;
            const int widthInterval = 25;
            DX.SetFontSize(fontScale);
            for (int i = 0; i < 5; i++)
            {
                DX.DrawRotaGraph(x + rankX, line1 + heightInterval * i, imageScale, 0, ResourceLoader.GetGraph(image + (i + 1) + ".png"));
                if (i == rank)//プレイヤーの色だけ違う色に
                    DX.DrawString(x + nameX, nameY + heightInterval * i, timeRankings[stageNum][i].name, DX.GetColor(255, 131, 0));
                else
                    DX.DrawString(x + nameX, nameY + heightInterval * i, timeRankings[stageNum][i].name, DX.GetColor(63, 42, 11));
                NumberDraw.ScoreDraw(timeRankings[stageNum][i].score, x + scoreX, line1 + heightInterval * i, widthInterval, imageScale, image);
                NumberDraw.TimeDraw(DateTime.FromBinary(timeRankings[stageNum][i].timeBinary), x + timeX, line2 + heightInterval * i, widthInterval, imageScale, image);
                NumberDraw.DateDraw(DateTime.FromBinary(timeRankings[stageNum][i].dateBinary), x + dateX, line2 + heightInterval * i, widthInterval, imageScale, image);
            }
        }
    }
}