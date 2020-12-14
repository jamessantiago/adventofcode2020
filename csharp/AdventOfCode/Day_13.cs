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
    public static class BetterEnumerable
    {
        public static IEnumerable<int> SteppedRange(int fromInclusive, int toExclusive, int step)
        {
            for (var i = fromInclusive; i < toExclusive; i += step)
            {
                yield return i;
            }
        }
    }

    public class Day_13 : BaseDay
    {
        int departure = 0;
        List<int> buses;
        int[] buses2;

        public Day_13()
        {
            var lines = File.ReadAllLines(InputFilePath);
            departure = int.Parse(lines[0]);
            buses = lines[1].Split(',').Select(d => d != "x" ? int.Parse(d) : 0).Where(d => d != 0).ToList();            
            buses.Sort();

            buses2 = lines[1].Split(',').Select(d => d != "x" ? int.Parse(d) : 0).ToArray();
        }

        public override string Solve_1()
        {
            int departbus = 0;
            int waittime = Int32.MaxValue;
            Dictionary<int, int> selectedbuses = new Dictionary<int, int>();
            foreach (var bus in buses)
            {
                int time = 0;
                while (time < departure)
                {
                    time += bus;
                }
                selectedbuses.Add(bus, time - departure);

            }

            foreach (var bus in selectedbuses)
            {
                if (bus.Value < waittime)
                {
                    waittime = bus.Value;
                    departbus = bus.Key;
                }
            }

            return (waittime * departbus).ToString();
        }

        

        public override string Solve_2()
        {
            //Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } });
            //List<IntExpr> X = new List<IntExpr>();
            //List<BoolExpr> val_c = new List<BoolExpr>();
            //List<BoolExpr> div_rules_c = new List<BoolExpr>();
            //List<BoolExpr> cons_rules_c = new List<BoolExpr>();
            //int indx = 0;
            //for (int i = 0; i < buses2.Length; i++)
            //{
            //    if (buses2[i] == 0) continue;
                
            //    X.Add((IntExpr)ctx.MkConst(ctx.MkSymbol("x_" + (indx + 1)), ctx.IntSort));
            //    val_c.Add(ctx.MkLe(ctx.MkInt(100000000000000), X[indx]));
            //    //val_c.Add(ctx.MkLe(ctx.MkInt(0), X[indx]));
            //    div_rules_c.Add(ctx.MkEq(ctx.MkMod(X[indx], ctx.MkInt(buses2[i])), ctx.MkInt(0)));
            //    if (i != 0)
            //        cons_rules_c.Add(ctx.MkEq(X[indx], ctx.MkAdd(X[0], ctx.MkInt(i))));
            //    indx++;
            //}

            //BoolExpr buses_c = ctx.MkTrue();
            //buses_c = ctx.MkAnd(ctx.MkAnd(val_c), buses_c);
            //buses_c = ctx.MkAnd(ctx.MkAnd(div_rules_c), buses_c);
            //buses_c = ctx.MkAnd(ctx.MkAnd(cons_rules_c), buses_c);

            //Microsoft.Z3.Solver s = ctx.MkSolver();
            //s.Assert(buses_c);

            //if (s.Check() == Status.SATISFIABLE)
            //{
            //    Model m = s.Model;
            //    var first = m.Evaluate(X[0]);
            //    return first.ToString();
            //} else
            //{
            //    return "no";
            //}

            long foundval = 0;
            //Dictionary<int, int> xmemo = new Dictionary<int, int>();
            //Dictionary<int, long> buscount = new Dictionary<int, long>();
            int[] xmemo = new int[buses2.Length];
            long[] buscount = new long[buses2.Length];
            int busleng = buses2.Length;
            List<int> nonxbuses = new List<int>();

            for (int i = 0; i < busleng; i++)
            {
                if (buses2[i] == 0) continue;
                int numxes = 0;
                for (int j = i + 1; j < busleng && buses2[j] == 0; j++) if (buses2[j] == 0) numxes++;
                xmemo[i] = numxes;
                nonxbuses.Add(i);
            }

            //for (long i = 100000000000000; i < Int64.MaxValue; i += buses2[0])
            //{
            //    long last = i;
            //    bool found = true;
            //    for (int b = 1; b < busleng; b++)
            //    {
            //        if (buses2[b] == 0) continue;
            //        while (buscount[b] < last) buscount[b] += buses2[b];
            //        if ((i + b + xmemo[b]) != buscount[b])
            //        {
            //            found = false;
            //            break;
            //        }
            //    }
            //    if (found)
            //    {
            //        foundval = i;
            //        break;
            //    }
            //}



            //for (long i = 100000000000004; i < Int64.MaxValue; i += buses2[0])
            //{
            //    int numxes = xmemo[0];
            //    if ((i + numxes + 1) % buses2[1 + numxes] != 0) continue;

            //    bool found = false;
            //    for (int b = 1; b < nonxbuses.Count - 1; b++)
            //    {
            //        numxes = xmemo[nonxbuses[b]];
            //        if ((i + nonxbuses[b] + numxes + 1) % buses2[nonxbuses[b] + numxes + 1] == 0) found = true;
            //        else found = false;

            //        if (!found) break;
            //    }
            //    if (found)
            //    {
            //        foundval = i;
            //        break;
            //    }
            //}

            long bus0 = (long)buses2[0];
            for (int b = 1; b < buses2.Length; b++)
            {
                if (buses2[b] == 0) continue;
                var tmp = buses2[b];
                while(true)
                {
                    foundval += bus0;
                    if ((foundval + b) % tmp == 0)
                    {
                        bus0 *= tmp;
                        break;
                    }
                }
            }


            return foundval.ToString();
        }
    }
}
