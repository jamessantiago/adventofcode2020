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
    public class Day_19 : BaseDay
    {
        string pattern = "";
        string pattern31 = "";
        string pattern42 = "";
        string[] messages;
        Dictionary<int, string> rules;
        string aindx = "";
        string bindx = "";
        int depthlimit = 25;

        public string CompilePattern(string rule, int depth)
        {
            depth++;
            if (depth > depthlimit) return "";
            if (rule.Contains('|'))
            {
                var ruleparts = rule.Split('|');
                var rulea = ruleparts[0].Split(' ');
                var ruleb = ruleparts[1].Split(' ');

                var patternA = "";
                for (int i = 1; i < rulea.Length - 1; i++)
                {
                    if (int.Parse(rulea[i]) == 42)
                    {
                        pattern42 = CompilePattern(rules[int.Parse(rulea[i])], depth);
                        patternA += pattern42;
                    }
                    else if (int.Parse(rulea[i]) == 31)
                    {
                        pattern31 = CompilePattern(rules[int.Parse(rulea[i])], depth);
                        patternA += pattern31;
                    }
                    else if (rulea[i] == "+") patternA += "+";
                    else if (rulea[i] == aindx) patternA += "a";
                    else if (rulea[i] == bindx) patternA += "b";
                    else patternA += CompilePattern(rules[int.Parse(rulea[i])], depth);
                }

                var patternB = "";
                for (int i = 1; i < ruleb.Length; i++)
                {
                    if (ruleb[i] == "+") patternB += "+";
                    else if (ruleb[i] == aindx) patternB += "a";
                    else if (ruleb[i] == bindx) patternB += "b";
                    else patternB += CompilePattern(rules[int.Parse(ruleb[i])], depth);
                }

                return $"({patternA}|{patternB})";
            } else
            {
                var rulea = rule.Split(' ');

                var patternA = "";
                for (int i = 1; i < rulea.Length; i++)
                {
                    if (rulea[i] == "+") patternA += "+";
                    else if (rulea[i] == aindx) patternA += "a";
                    else if (rulea[i] == bindx) patternA += "b";
                    else patternA += CompilePattern(rules[int.Parse(rulea[i])], depth);
                }

                return $"({patternA})";
            }
        }

        public Day_19()
        {
            var input = File.ReadAllText(InputFilePath).Split("\n\n");
            rules = input[0].Split('\n').ToDictionary(d => int.Parse(d.Split(':')[0]), d => d);
            messages = input[1].Split('\n');

            aindx = rules.First(d => d.Value.Contains("a")).Value.Split(' ')[0].Replace(":", "");
            bindx = rules.First(d => d.Value.Contains("b")).Value.Split(' ')[0].Replace(":", "");


        }

        public override string Solve_1()
        {
            pattern = "^" + CompilePattern(rules[0], 0) + "$";
            var m = messages.Where(d => Regex.IsMatch(d, pattern));
            return m.Count().ToString();
        }

        public override string Solve_2()
        {
            rules[8] = "8: 42 +";
            rules[11] = "11: 42 31 | 42 11 31";
            pattern = "^" + CompilePattern(rules[0], 0) + "$";
            var m = messages.Where(d => Regex.IsMatch(d, pattern));
            return m.Count().ToString();
        }
    }
}
