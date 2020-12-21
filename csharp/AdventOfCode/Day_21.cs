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
    public class Day_21 : BaseDay
    {
        HashSet<string> ingrediants = new HashSet<string>();
        HashSet<string> allergens = new HashSet<string>();
        List<string> unknowns;
        HashSet<(HashSet<string>, HashSet<string>)> foods = new HashSet<(HashSet<string>, HashSet<string>)>();

        public Day_21()
        {
            foreach (var food in File.ReadAllLines(InputFilePath))
            {
                var ingred = Regex.Match(food, @"([\w ]+) \(").Groups[1].Value.Split(' ');
                var allerg = Regex.Match(food, @"\(contains ([\w\, ]+)\)").Groups[1].Value.Split(", ");
                foods.Add((ingred.ToHashSet(), allerg.ToHashSet()));
                foreach (var i in ingred) ingrediants.Add(i);
                foreach (var i in allerg) allergens.Add(i);
            }            
        }

        public override string Solve_1()
        {
            unknowns = new List<string>();
            foreach (var ing in ingrediants)
            {
                var possal = foods.Where(d => d.Item1.Contains(ing)).SelectMany(d => d.Item2);
                if (possal.All(pa => foods.Any(f => !f.Item1.Contains(ing) && f.Item2.Contains(pa))))
                {
                    unknowns.Add(ing);                    
                }
            }
            int totalcount = 0;
            foreach (var unk in unknowns)
            {
                totalcount += foods.Count(d => d.Item1.Contains(unk));
            }
            return totalcount.ToString();
        }

        public override string Solve_2()
        {
            ingrediants = ingrediants.Where(d => !unknowns.Contains(d)).ToHashSet();

            HashSet<(HashSet<string>, HashSet<string>)> newfoods = new HashSet<(HashSet<string>, HashSet<string>)>();
            foreach (var food in foods)
            {
                var ingreds = food.Item1.Where(d => !unknowns.Contains(d)).ToHashSet();
                newfoods.Add((ingreds, food.Item2));
            }

            Dictionary<string, string> foundallergens = new Dictionary<string, string>();

            while (allergens.Count > 0)
            {
                allergens = allergens.Where(d => !foundallergens.Keys.Contains(d)).ToHashSet();
                ingrediants = ingrediants.Where(d => !foundallergens.Values.Contains(d)).ToHashSet();
                foreach (var al in allergens)
                {
                    var possfoods = foods.Where(d => d.Item2.Contains(al));
                    var ingreds = ingrediants.Where(d => possfoods.All(f => f.Item1.Contains(d)));
                    if (ingreds.Count() == 1)
                    {
                        foundallergens.Add(al, ingreds.First());
                    }
                }
            }

            var res = String.Join(",", foundallergens.Keys.OrderBy(d => d).Select(d => foundallergens[d]));

            return res;
        }
    }
}
