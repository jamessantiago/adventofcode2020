using AoCHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        private readonly List<long> _input;

        public Day_01()
        {
            _input = File.ReadLines(InputFilePath).Select(d => long.Parse(d)).ToList();
            _input.Sort();
        }

        public override string Solve_1()
        {
            foreach (var num in _input)
            {
                long expected = 2020 - num;
                if (_input.BinarySearch(expected) >= 0)
                {
                    return (num * expected).ToString();
                }
            }
            return "";
        }

        public override string Solve_2()
        {
            foreach (var num1 in _input)
            {
                foreach (var num2 in _input)
                {
                    long bothnums = num1 + num2;
                    if (bothnums > 2020) break;
                    long expected = 2020 - bothnums;
                    if (_input.BinarySearch(expected) >= 0)
                    {
                        return (num1 * num2 * expected).ToString();
                    }
                }
            }
            return "";
        }
    }
}
