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
    public class Day_08 : BaseDay
    {
        List<(string, int)> codes = new List<(string, int)>();
        HashSet<int> runCodes = new HashSet<int>();

        public Day_08()
        {
            foreach (var line in File.ReadLines(InputFilePath))
            {
                var vals = line.Split(' ');
                int val = 0;
                if (vals[1][0] == '+') val = int.Parse(vals[1].Substring(1));
                else val = -int.Parse(vals[1].Substring(1));
                codes.Add((vals[0], val));
            }
        }

        public override string Solve_1()
        {
            long accum = 0;
            int currindex = -1;
            do
            {
                runCodes.Add(currindex);
                currindex++;

                if (codes[currindex].Item1 == "acc")
                {
                    accum += codes[currindex].Item2;
                }
                else if (codes[currindex].Item1 == "jmp")
                {
                    currindex += codes[currindex].Item2 - 1;
                }
            } while (!runCodes.Contains(currindex));
            return accum.ToString();
        }

        public bool WillExitProperly(int indexcheck, out long accum)
        {
            int currindex = -1;
            accum = 0;
            int end = codes.Count;
            runCodes.Clear();
            do
            {
                runCodes.Add(currindex);
                currindex++;
                if (currindex >= end) return true;

                if (codes[currindex].Item1 == "acc")
                {
                    accum += codes[currindex].Item2;
                }
                else if ((currindex == indexcheck && codes[currindex].Item1 == "nop") || (currindex != indexcheck && codes[currindex].Item1 == "jmp"))
                {
                    currindex += codes[currindex].Item2 - 1;
                }
            } while (!runCodes.Contains(currindex));

            return false;
        }

        public override string Solve_2()
        {
            long accum = 0;
            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].Item1 == "nop" || codes[i].Item1 == "jmp")
                {
                    if (WillExitProperly(i, out accum)) return accum.ToString();
                }
            }

            return accum.ToString();
        }
    }
}
