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
    public class Day_18 : BaseDay
    {
        string[] problems;

        public Day_18()
        {
            problems = File.ReadAllLines(InputFilePath);
        }

        public long Calculate(string problem)
        {
            // 2 * 2
            var probparts = problem.Replace("(", "").Replace(")", "").Split(' ');
            var result = long.Parse(probparts[0]);
            for (int i = 1; i < probparts.Length; i +=2)
            {
                if (probparts[i] == "+") result += long.Parse(probparts[i + 1]);
                else if (probparts[i] == "*") result *= long.Parse(probparts[i + 1]);
            }
            return result;
        }

        public long CalculateSequence(string problem)
        {
            MatchCollection match = Regex.Matches(problem, @"\([\d +\*]+\)");
            if (match.Count == 0) return Calculate(problem);
            do
            {
                for (int i = 0; i < match.Count; i++)
                {
                    var tmp = Calculate(match[i].Value);
                    problem = problem.Replace(match[i].Value, tmp.ToString());
                }

                match = Regex.Matches(problem, @"\([\d +\*]+\)");
            } while (match.Count != 0);

            return Calculate(problem);
        }

        public override string Solve_1()
        {
            List<long> results = new List<long>();
            foreach (var prob in problems)
            {
                results.Add(CalculateSequence(prob));
            }
            return results.Sum().ToString();
        }

        public long Calculate2(string problem)
        {
            problem = problem.Replace("(", "").Replace(")", "");
            Match adds = Regex.Match(problem, @"\d+ \+ \d+");

            while (adds.Success)
            {
                var addparts = adds.Value.Split(' ');
                var tmp = long.Parse(addparts[0]) + long.Parse(addparts[2]);
                var reg = new Regex(Regex.Escape(adds.Value));
                problem = reg.Replace(problem, tmp.ToString(), 1);
                adds = Regex.Match(problem, @"\d+ \+ \d+");
            }

            Match mults = Regex.Match(problem, @"\d+ \* \d+");

            while (mults.Success) { 
                var addparts = mults.Value.Split(' ');
                var tmp = long.Parse(addparts[0]) * long.Parse(addparts[2]);
                var reg = new Regex(Regex.Escape(mults.Value));
                problem = reg.Replace(problem, tmp.ToString(), 1);
                mults = Regex.Match(problem, @"\d+ \* \d+");
            }

            return long.Parse(problem);
        }

        public long CalculateSequence2(string problem)
        {
            MatchCollection match = Regex.Matches(problem, @"\([\d +\*]+\)");
            if (match.Count == 0) return Calculate2(problem);
            do
            {
                for (int i = 0; i < match.Count; i++)
                {
                    var tmp = Calculate2(match[i].Value);
                    problem = problem.Replace(match[i].Value, tmp.ToString());
                }

                match = Regex.Matches(problem, @"\([\d +\*]+\)");
            } while (match.Count != 0);

            return Calculate2(problem);
        }

        public override string Solve_2()
        {
            List<long> results = new List<long>();
            foreach (var prob in problems)
            {
                results.Add(CalculateSequence2(prob));
            }
            return results.Sum().ToString();
        }
    }
}
