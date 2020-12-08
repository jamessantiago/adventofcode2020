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
    public class Day_07 : BaseDay
    {
        Dictionary<string, Dictionary<string, int>> bags = new Dictionary<string, Dictionary<string, int>>();

        public Day_07()
        {

            foreach (var line in File.ReadLines(InputFilePath))
            {
                var matches = Regex.Match(line, @"(\w+ \w+) bags contain (.+)");
                var color = matches.Groups[1].Value;
                var contents = new Dictionary<string, int>();
                foreach (var content in matches.Groups[2].Value.Split(","))
                {
                    if (content.StartsWith("no")) continue;
                    var contentmatch = Regex.Match(content, @"(\d+) (\w+ \w+)");
                    contents.Add(contentmatch.Groups[2].Value, int.Parse(contentmatch.Groups[1].Value));
                }
                bags.Add(color, contents);
            }
        }

        public bool CanContainGold(string color)
        {
            var bag = bags[color];
            if (bag.Any(d => d.Key == "shiny gold")) return true;
            if (bag.Any(d => d.Key.StartsWith("no"))) return false;
            foreach (var content in bag)
            {
                if (CanContainGold(content.Key)) return true;
            }
            return false;
        }

        public long BagCount(string color)
        {
            var bag = bags[color];
            long runningCount = 0;
            if (bag.Any(d => d.Key.StartsWith("no"))) return runningCount;
            foreach (var content in bag)
            {
                runningCount += content.Value;
                for (int i = 0; i < content.Value; i++)
                    runningCount += BagCount(content.Key);
            }
            return runningCount;
        }

        public override string Solve_1()
        {
            var count = bags.Count(d => CanContainGold(d.Key));
            return count.ToString();
        }

        public override string Solve_2()
        {
            return BagCount("shiny gold").ToString();
        }
    }
}
