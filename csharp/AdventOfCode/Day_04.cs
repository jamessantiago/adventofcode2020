using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AoCHelper;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day_04 : BaseDay
    {
        private readonly List<Dictionary<string, string>> _input;

        public Day_04()
        {
            var lines =File.ReadLines(InputFilePath).ToList();
            var passports = new List<Dictionary<string, string>>();
            var curpass = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    passports.Add(curpass);
                    curpass = new Dictionary<string, string>();
                    continue;
                }
                
                foreach (var record in line.Split(' '))
                {
                    var keyval = record.Split(':');
                    curpass.Add(keyval[0], keyval[1]);
                }                
            }
            _input = passports;
        }


        public override string Solve_1()
        {
            var validpassports = _input.Count(d => d.Count == 8 || (d.Count == 7 && !d.ContainsKey("cid")));
            return validpassports.ToString();
        }

        public bool isvalid(Dictionary<string, string> pass)
        {
            try
            {
                if (!(int.Parse(pass["byr"]) >= 1920 && int.Parse(pass["byr"]) <= 2002)) return false;
                if (!(int.Parse(pass["iyr"]) >= 2010 && int.Parse(pass["iyr"]) <= 2020)) return false;
                if (!(int.Parse(pass["eyr"]) >= 2020 && int.Parse(pass["eyr"]) <= 2030)) return false;
                bool iscm = pass["hgt"].Substring(pass["hgt"].Length - 2) == "cm";
                int height = int.Parse(pass["hgt"].Substring(0, pass["hgt"].Length - 2));
                if (iscm && !(height >= 150 && height <= 193)) return false;
                if (!iscm && !(height >= 59 && height <= 76)) return false;
                if (!Regex.IsMatch(pass["hcl"], @"#[0-9a-f]{6}")) return false;
                if (!Regex.IsMatch(pass["ecl"], @"amb|blu|brn|gry|grn|hzl|oth")) return false;
                if (!(int.TryParse(pass["pid"], out height) && pass["pid"].Length == 9)) return false;
            } catch
            {
                return false;
            }

            return true;
        }

        public override string Solve_2()
        {
            var validpassports = _input.Where(d => d.Count == 8 || (d.Count == 7 && !d.ContainsKey("cid"))).Count(d => isvalid(d));
            return validpassports.ToString();
        }
    }
}
