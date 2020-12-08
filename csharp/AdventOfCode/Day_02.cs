using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day_02 : BaseDay
    {
        private readonly List<Tuple<int, int, string, string>> _input;

        public Day_02()
        {
            _input = File.ReadLines(InputFilePath)
                .Select(d => d.Split(": -".ToCharArray()))
                .Select(d => new Tuple<int, int, string, string>(int.Parse(d[0]), int.Parse(d[1]), d[2], d[4]))
                .ToList();
        }

        public override string Solve_1()
        {
            int valid = 0;
            foreach (var pass in _input)
            {
                int ccnt = pass.Item4.ToCharArray().Count(d => char.Parse(pass.Item3) == d);
                if (ccnt >= pass.Item1 && ccnt <= pass.Item2) valid++;
            }
            return valid.ToString();
        }

        public override string Solve_2()
        {
            int valid = 0;
            foreach (var pass in _input)
            {
                if (pass.Item4[pass.Item1 - 1] == char.Parse(pass.Item3) ^ pass.Item4[pass.Item2 - 1] == char.Parse(pass.Item3)) valid++;
            }
            return valid.ToString();
        }
    }
}
