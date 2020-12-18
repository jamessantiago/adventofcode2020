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
    public class Day_17 : BaseDay
    {
        Dictionary<(int, int, int), bool> cubes = new Dictionary<(int, int, int), bool>();
        Dictionary<(int, int, int, int), bool> cubes2 = new Dictionary<(int, int, int, int), bool>();

        public Day_17()
        {
            var lines = File.ReadAllLines(InputFilePath);
            int dimen = lines[0].Length;

            for (int x = 0; x < dimen; x++)
            {
                for (int y = 0; y < dimen; y++)
                {
                    if (lines[y][x] == '#')
                    {
                        if (cubes.ContainsKey((x, y, 0))) cubes[(x, y, 0)] = true;
                        else cubes.Add((x, y, 0), true);

                        if (cubes2.ContainsKey((x, y, 0, 0))) cubes2[(x, y, 0, 0)] = true;
                        else cubes2.Add((x, y, 0, 0), true);
                    }
                    else
                    {
                        cubes.TryAdd((x, y, 0), false);
                        cubes2.TryAdd((x, y, 0, 0), false);
                    }
                    GetNeighbors(cubes, (x, y, 0), ref cubes);
                    GetNeighbors2(cubes2, (x, y, 0, 0), ref cubes2);
                }
            }
        }

        int maxradius = 2;

        public Dictionary<(int, int, int), bool> GetNeighbors(Dictionary<(int, int, int), bool> state, (int, int, int) target, ref Dictionary<(int, int, int), bool> newstate)
        {
            Dictionary<(int, int, int), bool> neighbors = new Dictionary<(int, int, int), bool>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0) continue;
                        (int, int, int) nei = (target.Item1 + x, target.Item2 + y, target.Item3 + z);
                        maxradius = Math.Max(maxradius, nei.Item1);
                        if (state.ContainsKey(nei)) neighbors.Add(nei, state[nei]);
                        else
                        {
                            neighbors.Add(nei, false);
                            newstate.TryAdd(nei, false);
                        }
                    }
                }
            }
            return neighbors;
        }

        public Dictionary<(int, int, int), bool> CalculateNextState(Dictionary<(int, int, int), bool> state)
        {
            var nextstate = new Dictionary<(int, int, int), bool>();
            foreach (var cube in state)
            {
                var newcube = (cube.Key.Item1, cube.Key.Item2, cube.Key.Item3);
                var neis = GetNeighbors(state, newcube, ref nextstate);
                var activeneicount = neis.Count(d => d.Value);

                if (cube.Value && !(activeneicount >= 2 && activeneicount <= 3)) nextstate.Add(newcube, false);
                else if (cube.Value) nextstate.Add(newcube, true);
                else if (cube.Value == false && activeneicount == 3) nextstate.Add(newcube, true);
                else nextstate.Add(newcube, false);
            }
            return nextstate;
        }

        void PrintCubes()
        {
            for (int z = -maxradius; z <= maxradius; z++)
            {
                Console.WriteLine($"z = {z}");
                for (int y = -maxradius; y <= maxradius; y++)
                {
                    for (int x = -maxradius; x <= maxradius; x++)
                    {
                        Console.Write(cubes.ContainsKey((x, y, z)) && cubes[(x, y, z)] ? "#" : ".");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        public override string Solve_1()
        {
            for (int i = 0; i < 6; i++)
            {
                //PrintCubes();
                cubes = CalculateNextState(cubes);
            }
            var activecount = cubes.Count(d => d.Value);
            return activecount.ToString();
        }

        public Dictionary<(int, int, int, int), bool> GetNeighbors2(Dictionary<(int, int, int, int), bool> state, 
            (int, int, int, int) target, ref Dictionary<(int, int, int, int), bool> newstate)
        {
            Dictionary<(int, int, int, int), bool> neighbors = new Dictionary<(int, int, int, int), bool>();
            for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            for (int z = -1; z <= 1; z++)
            for (int w = -1; w <= 1; w++)
            {
                if (x == 0 && y == 0 && z == 0 && w == 0) continue;
                (int, int, int, int) nei = (target.Item1 + x, target.Item2 + y, target.Item3 + z, target.Item4 + w);
                maxradius = Math.Max(maxradius, nei.Item1);
                if (state.ContainsKey(nei)) neighbors.Add(nei, state[nei]);
                else
                {
                    neighbors.Add(nei, false);
                    newstate.TryAdd(nei, false);
                }
            }
            return neighbors;
        }

        public Dictionary<(int, int, int, int), bool> CalculateNextState2(Dictionary<(int, int, int, int), bool> state)
        {
            var nextstate = new Dictionary<(int, int, int, int), bool>();
            foreach (var cube in state)
            {
                var newcube = (cube.Key.Item1, cube.Key.Item2, cube.Key.Item3, cube.Key.Item4);
                var neis = GetNeighbors2(state, newcube, ref nextstate);
                var activeneicount = neis.Count(d => d.Value);

                if (cube.Value && !(activeneicount >= 2 && activeneicount <= 3)) nextstate.Add(newcube, false);
                else if (cube.Value) nextstate.Add(newcube, true);
                else if (cube.Value == false && activeneicount == 3) nextstate.Add(newcube, true);
                else nextstate.Add(newcube, false);
            }
            return nextstate;
        }

        public override string Solve_2()
        {
            maxradius = 2;
            for (int i = 0; i < 6; i++)
            {
                cubes2 = CalculateNextState2(cubes2);
            }
            var activecount = cubes2.Count(d => d.Value);
            return activecount.ToString();
        }
    }
}
