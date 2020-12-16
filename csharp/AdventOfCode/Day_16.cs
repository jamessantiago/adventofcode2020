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
    public class Day_16 : BaseDay
    {
        Dictionary<string, List<(int, int)>> rules = new Dictionary<string, List<(int, int)>>();
        int[] myticket;
        List<int[]> nearticket = new List<int[]>();

        public Day_16()
        {
            var lines = File.ReadAllText(InputFilePath).Split("\n\n");
            foreach (var rule in lines[0].Split("\n"))
            {
                var ruleparts = Regex.Match(rule, @"([\w ]+): (\d+)-(\d+) or (\d+)-(\d+)");
                rules.Add(ruleparts.Groups[1].Value,
                    new List<(int, int)>
                    {
                        (int.Parse(ruleparts.Groups[2].Value), int.Parse(ruleparts.Groups[3].Value)),
                        (int.Parse(ruleparts.Groups[4].Value), int.Parse(ruleparts.Groups[5].Value)),
                    });
            }

            myticket = lines[1].Split("\n")[1].Split(',').Select(d => int.Parse(d)).ToArray();

            foreach (var ticket in lines[2].Split("\n"))
            {
                if (ticket.StartsWith("near")) continue;
                nearticket.Add(ticket.Split(',').Select(d => int.Parse(d)).ToArray());
            }
        }

        public bool IsValid(int v)
        {
            foreach (var rule in rules)
            {
                if (v >= rule.Value[0].Item1 && v <= rule.Value[0].Item2) return true;
                if (v >= rule.Value[1].Item1 && v <= rule.Value[1].Item2) return true;
            }
            return false;
        }

        public bool IsValidFor(int v, List<(int,int)> rule)
        {
            if (v >= rule[0].Item1 && v <= rule[0].Item2) return true;
            if (v >= rule[1].Item1 && v <= rule[1].Item2) return true;
            return false;
        }

        public override string Solve_1()
        {
            List<int> invalidfield = new List<int>();
            foreach (var ticket in nearticket)
            {
                invalidfield.AddRange(ticket.Where(d => !IsValid(d)));
            }
            return invalidfield.Sum().ToString();
        }

        public Dictionary<int, string> RecurseTestFields(Dictionary<int, string> knownfields, out bool valid)
        {
            int rulecount = rules.Count;
            while (knownfields.Count != rulecount)
            {
                bool fieldadded = false;
                int minpossiblefield = Int32.MaxValue;
                int minindex = -1;
                string[] minfield = null;
                for (int i = 0; i < rulecount; i++)
                {
                    if (knownfields.ContainsKey(i)) continue;
                    var ruletest = rules.Where(d => fields[i].All(k => IsValidFor(k, d.Value)));
                    if (ruletest.Count() == 1)
                    {
                        knownfields.Add(i, ruletest.First().Key);
                        rules.Remove(ruletest.First().Key);
                        fieldadded = true;
                    }
                    else if (minpossiblefield > ruletest.Count())
                    {
                        minpossiblefield = ruletest.Count();
                        minindex = i;
                        minfield = ruletest.Select(d => d.Key).ToArray();
                    }
                }

                if (!fieldadded && rulecount - knownfields.Count == minpossiblefield)
                {
                    valid = false;
                    return knownfields;
                }

                if (!fieldadded)
                {
                    foreach (var mf in minfield)
                    {
                        bool modelvalid = false;
                        var tmpfields = RecurseTestFields(knownfields, out modelvalid);
                        if (modelvalid)
                        {
                            knownfields = tmpfields;
                            break;
                        }
                    }
                }
            }
            valid = true;
            return knownfields;
        }

        List<int[]> validtickets;
        List<int[]> fields;

        public override string Solve_2()
        {
            validtickets = nearticket.Where(d => d.All(d => IsValid(d))).ToList();

            fields = new List<int[]>();

            for (int i = 0; i < rules.Count; i++)
            {
                fields.Add(validtickets.Select(d => d[i]).ToArray());
            }

            Queue<(int, string[], Dictionary<int, string>)> fieldstotest = new Queue<(int, string[], Dictionary<int, string>)>();
            
            Dictionary<int, string> knownfields = new Dictionary<int, string>();
            bool valid = false;
            knownfields = RecurseTestFields(knownfields, out valid);            

            long departval = 1;
            foreach (var departfield in knownfields.Where(d => d.Value.StartsWith("depart")))
            {
                departval *= myticket[departfield.Key];
            }

            return departval.ToString();
        }
    }
}
