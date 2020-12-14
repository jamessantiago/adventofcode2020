using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Z3;

namespace AdventOfCode
{
    public class Day_14 : BaseDay
    {
        string[] lines;
        ulong[] mem = new ulong[UInt16.MaxValue];
        Dictionary<long, long> newmem = new Dictionary<long, long>();
        long maxint = (long)Math.Pow(2, 36);

        public Day_14()
        {
            lines = File.ReadAllLines(InputFilePath);
        }

        public ulong ApplyMask(char[] mask, ulong val)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X') continue;
                if (mask[i] == '1')
                {
                    val |= (ulong)1 << i;
                } else {
                    val &= ~((ulong)1 << i);
                }                
                //Console.WriteLine(Convert.ToString((long)val, 2));
            }
            return val;
        }

        public long[] ApplyMask2(char[] mask, long val)
        {
            List<long> newvals = new List<long>() { val };
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == '0') continue;
                if (mask[i] == '1')
                {
                    for (int n = 0; n < newvals.Count; n++)
                    {
                        newvals[n] |= (long)1 << i;
                    }
                }
                else
                {
                    List<long> tmpvals = new List<long>();
                    for (int n = 0; n < newvals.Count; n++)
                    {
                        tmpvals.Add(newvals[n] | ((long)1 << i));
                        tmpvals.Add(newvals[n] & ~((long)1 << i));
                    }
                    newvals = tmpvals;   
                }                
            }
            //foreach (var v in newvals)
            //    Console.WriteLine(Convert.ToString((long)v, 2).ToString());
            return newvals.ToArray();
        }

        public override string Solve_1()
        {
            char[] curmask = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    var mask = Regex.Match(line, "mask = (.+)").Groups[1].Value;
                    curmask = mask.Reverse().ToArray();
                } else
                {
                    var match = Regex.Match(line, @"mem\[(\d+)\] = (\d+)");
                    var mempos = int.Parse(match.Groups[1].Value);
                    var val = ulong.Parse(match.Groups[2].Value);
                    mem[mempos] = ApplyMask(curmask, val);
                }
            }

            return mem.Select(d => (decimal)d).Sum().ToString();
        }

        public override string Solve_2()
        {
            char[] curmask = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    var mask = Regex.Match(line, "mask = (.+)").Groups[1].Value;
                    curmask = mask.Reverse().ToArray();
                }
                else
                {
                    var match = Regex.Match(line, @"mem\[(\d+)\] = (\d+)");
                    var mempos = int.Parse(match.Groups[1].Value);
                    var val = long.Parse(match.Groups[2].Value);
                    foreach (var memaddress in ApplyMask2(curmask, mempos))
                    {
                        if (newmem.ContainsKey(memaddress))
                            newmem[memaddress] = val;
                        else
                            newmem.Add(memaddress, val);
                    }
                }
            }
            return newmem.Select(d => d.Value).Sum().ToString();
        }
    }
}
