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
    public class Day_24 : BaseDay
    {
        List<List<string>> rules = new List<List<string>>();

        public Day_24()
        {            
            var lines = File.ReadAllLines(InputFilePath);
            foreach (var line in lines)
            {
                var matches = Regex.Matches(line, @"e|se|sw|w|nw|ne");
                var rule = new List<string>();
                for (int i = 0; i < matches.Count; i++)
                {
                    rule.Add(matches[i].Value);
                }
                rules.Add(rule);
            }
        }

        public (int,int,int) MoveTile((int, int, int) tile, string direction)
        {
            switch (direction)
            {
                case "e":
                    tile.Item3++;
                    break;
                case "se":
                    tile.Item2 += tile.Item1;
                    tile.Item3 += tile.Item1;
                    tile.Item1 = 1 - tile.Item1;
                    break;
                case "sw":
                    tile.Item2 += tile.Item1;
                    tile.Item3 -= (1 - tile.Item1);
                    tile.Item1 = 1 - tile.Item1;
                    break;
                case "w":
                    tile.Item3--;
                    break;
                case "nw":
                    tile.Item2 -= (1 - tile.Item1);
                    tile.Item3 -= (1 - tile.Item1);
                    tile.Item1 = 1 - tile.Item1;
                    break;
                case "ne":
                    tile.Item2 -= (1 - tile.Item1);
                    tile.Item3 += tile.Item1;
                    tile.Item1 = 1 - tile.Item1;
                    break;
                default:
                    break;
            }
            return tile;
        }

        public override string Solve_1()
        {
            Dictionary<(int, int, int), bool> tiles = new Dictionary<(int, int, int), bool>();
            foreach (var rule in rules)
            {
                (int, int, int) tile = (0, 0, 0);
                foreach (var direction in rule)
                {
                    tile = MoveTile(tile, direction);
                }

                if (tiles.ContainsKey(tile))
                    tiles[tile] = !tiles[tile];
                else
                    tiles.Add(tile, true);
            }

            return tiles.Count(d => d.Value).ToString();
        }

        List<string> directions = new List<string> { "e", "se", "sw", "w", "nw", "ne" };

        public List<(int, int, int)> GetNeighbors((int, int, int) tile)
        {
            List<(int, int, int)> neighbors = new List<(int, int, int)>();
            foreach (var dir in directions)
            {
                neighbors.Add(MoveTile(tile, dir));
            }
            return neighbors;
        }

        public override string Solve_2()
        {
            Dictionary<(int, int, int), bool> tiles = new Dictionary<(int, int, int), bool>();
            foreach (var rule in rules)
            {
                (int, int, int) tile = (0, 0, 0);
                foreach (var direction in rule)
                {
                    tile = MoveTile(tile, direction);
                }

                if (tiles.ContainsKey(tile))
                    tiles[tile] = !tiles[tile];
                else
                    tiles.Add(tile, true);
            }

            for (int i = 0; i < 100; i++)
            {
                var changetowhite = tiles.Where(d => d.Value).Where(d =>
                {
                    var neis = GetNeighbors(d.Key);
                    var btcount = tiles.Count(t => neis.Contains(t.Key) && t.Value);
                    return btcount == 0 || btcount > 2;
                }).ToList();

                var whitetiles = tiles.Where(d => d.Value).SelectMany(d => GetNeighbors(d.Key)).ToHashSet()
                    .Where(d => !tiles.ContainsKey(d) || !tiles[d]);
                
                var changetoblack = whitetiles.Where(d =>
                    {
                        var neis = GetNeighbors(d);
                        var btcount = tiles.Count(t => neis.Contains(t.Key) && t.Value);
                        return btcount == 2;
                    }).ToList();

                foreach (var ctw in changetowhite)
                {
                    tiles[ctw.Key] = false;
                }

                foreach (var ctb in changetoblack)
                {
                    if (tiles.ContainsKey(ctb))
                        tiles[ctb] = true;
                    else
                        tiles.Add(ctb, true);
                }

                //Console.WriteLine("Day {0}: {1}", i + 1, tiles.Count(d => d.Value));
            }

            return tiles.Count(d => d.Value).ToString();
        }
    }
}
