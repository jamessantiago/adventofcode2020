using AoCHelper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Z3;

namespace AdventOfCode
{
    public class Day_15 : BaseDay
    {
        List<int> input;

        public Day_15()
        {
            input = File.ReadAllText(InputFilePath).Split(',').Select(d => int.Parse(d)).ToList();
        }

        public int GetLastNum(int n)
        {
            Dictionary<int, int> lastindexes = new Dictionary<int, int>();
            for (int i = 0; i < input.Count - 1; i++) lastindexes.Add(input[i], i);
            int lastnum = input.Last();

            for (int i = input.Count; i < n; i++)
            {
                if (!lastindexes.ContainsKey(lastnum))
                {
                    lastindexes.Add(lastnum, i - 1);
                    lastnum = 0;
                }
                else
                {
                    int newval = i - lastindexes[lastnum] - 1;
                    lastindexes[lastnum] = i - 1;
                    lastnum = newval;
                }
            }
            return lastnum;
        }

        public override string Solve_1()
        {
            return GetLastNum(2020).ToString();
        }

        public override string Solve_2()
        {
            return GetLastNum(30000000).ToString();
        }
    }
}
