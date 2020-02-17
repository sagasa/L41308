using System;
using System.Collections.Generic;
using DxLibDLL;
using SAGASALib;

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
            for (int i = 1; i < 3; i++)
            {
                scoreRankings[i] = new List<Entry> { };
                timeRankings[i] = new List<Entry> { };
                for (int j = 0; j < 10; j++)
                {
                    scoreRankings[i].Add(new Entry());
                    timeRankings[i].Add(new Entry());
                    scoreRankings[i][j].name = "No_data";
                    scoreRankings[i][j].score = 0;
                    scoreRankings[i][j].timeBinary = DateTime.MinValue.ToBinary();
                    scoreRankings[i][j].dateBinary = DateTime.MinValue.ToBinary();
                    timeRankings[i][j].name = "No_data";
                    timeRankings[i][j].score = 0;
                    timeRankings[i][j].timeBinary = DateTime.MinValue.ToBinary();
                    timeRankings[i][j].dateBinary = DateTime.MinValue.ToBinary();
                }
            }
        }
        //ソート
        public void RankingSort(Entry entry, int stageNum)
        {
            //10番を追加、データ代入
            scoreRankings[stageNum].Add(new Entry());
            timeRankings[stageNum].Add(new Entry());
            scoreRankings[stageNum][10] = entry;
            timeRankings[stageNum][10] = entry;
            //ソート
            for (int i = 0; i < 10; i++)
            {
                //スコアのソート
                if (scoreRankings[stageNum][i].score <= scoreRankings[stageNum][10].score)
                {
                    Entry w = scoreRankings[stageNum][i];
                    scoreRankings[stageNum][i] = scoreRankings[stageNum][10];
                    scoreRankings[stageNum][10] = w;
                }
                //タイム
                if (DateTime.FromBinary(timeRankings[stageNum][i].timeBinary) >= DateTime.FromBinary(scoreRankings[stageNum][10].timeBinary))
                {
                    Entry w = timeRankings[stageNum][i];
                    timeRankings[stageNum][i] = scoreRankings[stageNum][10];
                    scoreRankings[stageNum][10] = w;
                }
            }
            //10番を削除
            scoreRankings[stageNum].RemoveAt(10);
            timeRankings[stageNum].RemoveAt(10);
        }
        //順位を返す
        public void RankingSort(Entry entry, int stageNum, ref int scoreRank, ref int timeRank)
        {
            //10番を追加、データ代入
            scoreRankings[stageNum].Add(new Entry());
            timeRankings[stageNum].Add(new Entry());
            scoreRankings[stageNum][10] = entry;
            timeRankings[stageNum][10] = entry;
            scoreRank = 10;
            timeRank = 10;
            //ソート
            for (int i = 0; i < 10; i++)
            {
                //スコアのソート
                if (scoreRankings[stageNum][i].score <= scoreRankings[stageNum][10].score)
                {
                    if (scoreRank == 10)
                        scoreRank = i;
                    Entry w = scoreRankings[stageNum][i];
                    scoreRankings[stageNum][i] = scoreRankings[stageNum][10];
                    scoreRankings[stageNum][10] = w;
                }
                //タイム
                if (DateTime.FromBinary(timeRankings[stageNum][i].timeBinary) >= DateTime.FromBinary(scoreRankings[stageNum][10].timeBinary))
                {
                    if (timeRank == 10)
                        timeRank = i;
                    Entry w = timeRankings[stageNum][i];
                    timeRankings[stageNum][i] = scoreRankings[stageNum][10];
                    scoreRankings[stageNum][10] = w;
                }
            }
            //10番を削除
            scoreRankings[stageNum].RemoveAt(10);
            timeRankings[stageNum].RemoveAt(10);
        }

        //記録更新を確認
        public bool BreakRecord(Entry entry, int stageNum)
        {
            if (entry.score >= scoreRankings[stageNum][9].score)
                return true;
            if (DateTime.FromBinary(entry.timeBinary) <= DateTime.FromBinary(timeRankings[stageNum][9].timeBinary))
                return true;
            return false;
        }

        public void RankingDraw()
        {
            int flameX = 50;
            float fontScale = 0.18f;
            int heightInterval = 50;
            int widthInterval = 10;
            for (int i = 1; i <= 10; i++)
            {
                if (i == 10)
                {
                    DX.DrawRotaGraph(flameX - widthInterval, 100 + heightInterval * (i - 1), fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + 1 + ".png"));
                    DX.DrawRotaGraph(flameX, 100 + heightInterval * (i - 1), fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + 0 + ".png"));
                }
                else
                {
                    DX.DrawRotaGraph(flameX, 100 + heightInterval * (i - 1), fontScale, 0, ResourceLoader.GetGraph("image_result/result_num_" + i + ".png"));
                }
            }
        }
    }
}