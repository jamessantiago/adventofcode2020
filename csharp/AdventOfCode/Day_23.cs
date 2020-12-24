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
using System.Collections.Immutable;

namespace AdventOfCode
{
    public class Day_23 : BaseDay
    {
        ImmutableList<int> cups;
        ImmutableList<int> origcups;

        public Day_23()
        {
            cups = File.ReadAllText(InputFilePath).ToCharArray().Select(d => int.Parse(d.ToString())).ToImmutableList();
            origcups = cups.ToImmutableList();
        }

        public override string Solve_1()
        {

            int cuplen = cups.Count();
            int curcup = cups.First();

            for (int i = 0; i < 100; i++)
            {
                //Console.WriteLine("Cups: {0}", String.Join(", ", cups));
                var cumpindx = cups.IndexOf(curcup);
                var cup1 = cups[(cumpindx + 1) % cuplen];
                var cup2 = cups[(cumpindx + 2) % cuplen];
                var cup3 = cups[(cumpindx + 3) % cuplen];
                cups = cups.Remove(cup1);
                cups = cups.Remove(cup2);
                cups = cups.Remove(cup3);
                //Console.WriteLine("Take: {0} {1} {2}", cup1, cup2, cup3);
                cumpindx = cups.IndexOf(curcup);
                curcup = cups[(cumpindx + 1) % (cuplen - 3)];

                var des = cups[cumpindx] - 1;
                var mincup = cups.Min();
                var maxcup = cups.Max();
                if (des < mincup) des = maxcup;
                while (des == cup1 || des == cup2 || des == cup3)
                {
                    des--;
                    if (des < mincup) des = maxcup;
                }
                //Console.WriteLine("Dest: {0}\n", des);
                var newindx = cups.IndexOf(des) + 1;
                if (newindx == cuplen - 3)
                {
                    cups = cups.Add(cup1);
                    cups = cups.Add(cup2);
                    cups = cups.Add(cup3);
                }
                else
                {
                    cups = cups.Insert(newindx, cup3);
                    cups = cups.Insert(newindx, cup2);
                    cups = cups.Insert(newindx, cup1);
                }
            }

            var result = "";
            for (int i = cups.IndexOf(1) + 1; i < cups.IndexOf(1) + cups.Count; i++)
            {
                result += cups[i % cups.Count].ToString();
            }

            return result;
        }

        public override string Solve_2()
        {
            //cups = origcups;
            //int mc = cups.Max();
            //int cc = cups.Count;
            //for (int j = 0; j < 1000000 - cc; j++) cups = cups.Add(j + mc + 1);
            //int cuplen = cups.Count();
            //int offset = 0;

            //int mincup = cups.Min();
            //int maxcup = cups.Max();

            //int i = 0;

            //for (i = 0; i < 10000000; i++)
            //{
            //    //Console.WriteLine("Cups: {0}", String.Join(", ", cups));
            //    var cup1 = cups[(i + offset + 1) % cuplen];
            //    var cup2 = cups[(i + offset + 2) % cuplen];
            //    var cup3 = cups[(i + offset + 3) % cuplen];

            //    var des = cups[(i + offset) % cuplen] - 1;

            //    if (des < mincup) des = maxcup;
            //    while (des == cup1 || des == cup2 || des == cup3)
            //    {
            //        des--;
            //        if (des < mincup) des = maxcup;
            //    }

            //    cups = cups.RemoveAt((i + offset + 1) % cuplen);
            //    cups = cups.RemoveAt((i + offset + 2) % cuplen);
            //    cups = cups.RemoveAt((i + offset + 3) % cuplen);
            //    //Console.WriteLine("Take: {0} {1} {2}", cup1, cup2, cup3);

            //    //Console.WriteLine("Dest: {0}\n", des);           
            //    var newindx = cups.IndexOf(des) + 1;

            //    if (newindx < (i + offset) % cuplen) offset++;
            //    if (newindx < (i + offset + 1) % cuplen) offset++;
            //    if (newindx < (i + offset + 2) % cuplen) offset++;
            //    if (newindx == cuplen - 3)
            //    {
            //        cups = cups.Add(cup1);
            //        cups = cups.Add(cup2);
            //        cups = cups.Add(cup3);
            //    }
            //    else
            //    {
            //        cups = cups.Insert(newindx, cup3);
            //        cups = cups.Insert(newindx, cup2);
            //        cups = cups.Insert(newindx, cup1);
            //    }
            //}



            //var ca = cups[cups.IndexOf(1) + 1];
            //var cb = cups[cups.IndexOf(1) + 2];

            cups = origcups;
            cups = cups.Add(10);

            int[] numbers = new int[1000001];
            for (int i = 0; i < cups.Count - 1; i++) numbers[cups[i]] = cups[i + 1];

            numbers[0] = 0;
            numbers[1000000] = cups.First();
            for (int i = 10; i < 1000000; ++i)
            {
                numbers[i] = i + 1;
            }

            int current = cups.First();

            for (int i = 0; i < 10000000; ++i)
            {
                int value = current;
                int next1 = numbers[current];
                int next2 = numbers[next1];
                int next3 = numbers[next2];

                do
                {
                    value--;
                    if (value == 0)
                        value = 1000000;
                }
                while (next1 == value || next2 == value || next3 == value);

                numbers[current] = numbers[next3];
                numbers[next3] = numbers[value];
                numbers[value] = next1;

                current = numbers[current];
            }

            long prod = numbers[1];
            prod *= numbers[numbers[1]];
            return prod.ToString();
        }
    }
}
