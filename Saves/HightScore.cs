using System;
using System.Collections.Generic;
using DxLibDLL;

namespace Giraffe.Saves
{
    public class HightScore
    {
        //後で削除
        //public Dictionary<string, int[]> bestTimes = new Dictionary<string, int[]>
        //{
        //    {"stage_1",new int[3]{1,00,0 } },{"stage_2",new int[3]{1,30,0 } },{"stage_3",new int[3]{2,00,0 } }
        //};
        //public Dictionary<string, int> bestScores = new Dictionary<string, int>
        //{
        //    {"stage_1",1000 },{"stage_2",1000 },{"stage_3",1000}
        //};

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
            for (int i = 0; i < 3; i++)
            {
                scoreRankings[i] = new List<Entry> { };
                timeRankings[i] = new List<Entry> { };
                for (int j = 0; j < 10; j++)
                {
                    scoreRankings[i].Add(new Entry());
                    timeRankings[i].Add(new Entry());
                    scoreRankings[i][j].name = "No_data";
                    scoreRankings[i][j].score = 0;
                    scoreRankings[i][j].timeBinary = DateTime.MaxValue.ToFileTime();
                    scoreRankings[i][j].dateBinary = DateTime.MaxValue.ToFileTime();
                    timeRankings[i][j].name = "No_data";
                    timeRankings[i][j].score = 0;
                    timeRankings[i][j].timeBinary = DateTime.MaxValue.ToFileTime();
                    timeRankings[i][j].dateBinary = DateTime.MaxValue.ToFileTime();
                }
            }
        }

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
                if (DateTime.FromBinary(timeRankings[stageNum][i].timeBinary) <= DateTime.FromBinary(scoreRankings[stageNum][10].timeBinary))
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
                if (DateTime.FromBinary(timeRankings[stageNum][i].timeBinary) <= DateTime.FromBinary(scoreRankings[stageNum][10].timeBinary))
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
    }
}