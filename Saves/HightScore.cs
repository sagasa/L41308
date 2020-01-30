using System;
using System.Collections.Generic;
namespace Giraffe.Saves
{
    public class HightScore
    {
        public Dictionary<string, int> bestScores = new Dictionary<string, int>{
            { "stage_1", 500},{"stage_2",500 },{"stage_3",500 } };
        public int bestScore = 500;
        public int[] bestTime = { 1, 30, 0 };
    }
}