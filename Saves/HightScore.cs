using System;
using System.Collections.Generic;
using DxLibDLL;

namespace Giraffe.Saves
{
    public class HightScore
    {
        public Dictionary<string, int[]> bestTimes = new Dictionary<string, int[]>
        {
            {"stage_1",new int[3]{1,00,0 } },{"stage_2",new int[3]{1,30,0 } },{"stage_3",new int[3]{2,00,0 } }
        };
        public Dictionary<string, int> bestScores = new Dictionary<string, int>
        {
            {"stage_1",1000 },{"stage_2",1000 },{"stage_3",1000}
        };

        private class Entry
        {
            string name;
            int score;
            long time;
        }

        //Dictionary<int ,List<Tuple<string,int>>>

        public Dictionary<string, int[]> scoreRankingScores = new Dictionary<string, int[]>
        {
            {"stage_1",new int[10]{1000, 900, 800, 700, 600, 500, 400, 300, 200, 100 } },
            {"stage_2",new int[10]{1000, 900, 800, 700, 600, 500, 400, 300, 200, 100 } },
            {"stage_3",new int[10]{1000, 900, 800, 700, 600, 500, 400, 300, 200, 100 } }
        };
        public Dictionary<string, string[]> scoreRankingNames = new Dictionary<string, string[]>
        {
            {"stage_1",new string[10]{"No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data" } },
            {"stage_2",new string[10]{"No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data" } },
            {"stage_3",new string[10]{"No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data" } }
        };
        public Dictionary<string, DateTime[]> scoreRankingDates = new Dictionary<string, DateTime[]>
        {
            {"stage_1",new DateTime[10]{DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now } },
            {"stage_2",new DateTime[10]{DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now } },
            {"stage_3",new DateTime[10]{DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now } }
        };
        public Dictionary<string, int[]> timeRankingTimes = new Dictionary<string, int[]>
        {
            {"stage_1_1",new int[]{1,00,0 } },{"stage_1_2",new int[]{1,10,0 } },{"stage_1_3",new int[]{1,20,0 } },{"stage_1_4",new int[]{1,30,0 } },{"stage_1_5",new int[]{1,40,0 } },
            {"stage_1_6",new int[]{1,00,0 } },{"stage_1_7",new int[]{1,00,0 } },{"stage_1_8",new int[]{1,10,0 } },{"stage_1_9",new int[]{2,20,0 } },{"stage_1_10",new int[]{2,30,0 } },

            
        };
        public Dictionary<string, string[]> timeRankingNames = new Dictionary<string, string[]>
        {
            {"stage_1",new string[10]{"No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data" } },
            {"stage_2",new string[10]{"No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data" } },
            {"stage_3",new string[10]{"No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data", "No_data" } }
        };
        public Dictionary<string, DateTime[]> timeRankingDates = new Dictionary<string, DateTime[]>
        {
            {"stage_1",new DateTime[10]{DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now } },
            {"stage_2",new DateTime[10]{DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now } },
            {"stage_3",new DateTime[10]{DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now } }
        };
    }
}