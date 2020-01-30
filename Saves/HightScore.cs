using System;
using System.Collections.Generic;
namespace Giraffe.Saves
{
    public class HightScore
    {
        public Dictionary<string, int> bestScores = new Dictionary<string, int>
        {
            { "stage_1", 500},{"stage_2",500 },{"stage_3",500 }
        };
        public Dictionary<string, int[]> bestTimes = new Dictionary<string, int[]>
        {
            {"stage_1",new int[]{1,30,0 } },{"stage_2",new int[]{1,30,0 } },{"stage_3",new int[]{1,30,0 } }
        };
    }
}