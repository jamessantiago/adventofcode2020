using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;

namespace AdventOfCode
{
    public class Day_03 : BaseDay
    {
        private readonly List<char[]> _input;

        public Day_03()
        {
            _input = File.ReadLines(InputFilePath).Select(d => d.ToCharArray()).ToList();
        }

        public override string Solve_1()
        {
            int xlength = _input.First().Length;
            int ylength = _input.Count;
            int x = 0;
            int trees = 0;

            for (int y = 1; y < ylength; y++)
            {
                x = (x + 3) % xlength;
                if (_input[y][x] == '#') trees++;
            }

            return trees.ToString();
        }

        public override string Solve_2()
        {
            int xlength = _input.First().Length;
            int ylength = _input.Count;
            int x = 0;
            List<int> trees = new List<int>();
            int[] slopes = { 1, 3, 5, 7 };

            foreach (var slope in slopes)
            {
                var slopetrees = 0;
                for (int y = 1; y < ylength; y++)
                {
                    x = (x + slope) % xlength;
                    if (_input[y][x] == '#') slopetrees++;
                }
                x = 0;
                trees.Add(slopetrees);
            }

            var treecount = 0;
            for (int y = 2; y < ylength; y += 2)
            {
                x = (x + 1) % xlength;
                if (_input[y][x] == '#') treecount++;
            }
            trees.Add(treecount);

            long answer = 1;
            foreach (var t in trees) answer *= t;

            return answer.ToString();
        }
    }
}
