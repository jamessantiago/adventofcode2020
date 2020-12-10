using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day_10 : BaseDay
    {
        List<int> adapters;

        public Day_10()
        {
            adapters = File.ReadLines(InputFilePath).Select(d => int.Parse(d)).ToList();
            adapters.Add(0);
            adapters.Add(adapters.Max() + 3);
            adapters.Sort();
        }

        public override string Solve_1()
        {
            long onecount = 0;
            long threecount = 0;
            for (int i = 1; i < adapters.Count; i++)
            {
                int diff = adapters[i] - adapters[i - 1];
                if (diff == 1) onecount++;
                else threecount++;
            }
            return (onecount * threecount).ToString();
        }

        public override string Solve_2()
        {
            memo.Add(0, 1);
            return PathCount(adapters.Last()).ToString();
        }

        Dictionary<int, long> memo = new Dictionary<int, long>();

        public long PathCount(int index)
        {
            if (memo.ContainsKey(index)) return memo[index];

            long count = 0;
            for (int i = 1; i <= 3; i++)
            {
                if (adapters.Contains(index - i))
                {
                    var tmp = PathCount(index - i);
                    if (!memo.ContainsKey(index - i))
                        memo.Add(index - i, tmp);
                    count += tmp;
                }
            }

            return count;
        }

    }
}
