using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AdventOfCode
{
    public class Day_06 : BaseDay
    {
        

        public Day_06()
        {
            
        }

        public override string Solve_1()
        {
            List<HashSet<char>> groups = new List<HashSet<char>>();
            var lines = File.ReadLines(InputFilePath);

            var currentgroup = new HashSet<char>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    groups.Add(currentgroup);
                    currentgroup = new HashSet<char>();
                    continue;
                }

                foreach (var c in line.ToCharArray())
                {
                    currentgroup.Add(c);
                }
            }

            return groups.Select(d => d.Count).Sum().ToString();
        }

        public override string Solve_2()
        {
            var groups = new List<Dictionary<char, int>>();
            var lines = File.ReadLines(InputFilePath);

            var currentgroup = new Dictionary<char, int>();
            var groupcount = 0;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    foreach (var c in currentgroup)
                        if (c.Value != groupcount) currentgroup.Remove(c.Key);
                    groups.Add(currentgroup);
                    currentgroup = new Dictionary<char, int>();
                    groupcount = 0;
                    continue;
                }

                groupcount++;
                foreach (var c in line.ToCharArray())
                {
                    if (!currentgroup.ContainsKey(c) && groupcount == 1) currentgroup.Add(c, 1);
                    else if (currentgroup.ContainsKey(c) && currentgroup[c] == groupcount - 1) currentgroup[c]++;
                    else currentgroup.Remove(c);
                }
            }

            return groups.Select(d => d.Count).Sum().ToString();
        }
    }
}
