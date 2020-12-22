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
    public class Day_22 : BaseDay
    {

        List<int> deck1 = new List<int>();
        List<int> deck2 = new List<int>();
        Queue<int> deck1a = new Queue<int>();
        Queue<int> deck2a = new Queue<int>();

        public Day_22()
        {
            var decks = File.ReadAllText(InputFilePath).Split("\n\n");
            deck1 = decks[0].Split('\n').Skip(1).Select(d => int.Parse(d)).ToList();
            deck2 = decks[1].Split('\n').Skip(1).Select(d => int.Parse(d)).ToList();
            foreach (var c in deck1) deck1a.Enqueue(c);
            foreach (var c in deck2) deck2a.Enqueue(c);
        }

        public override string Solve_1()
        {
            while (deck1.Count != 0 && deck2.Count != 0)
            {
                if (deck1[0] > deck2[0])
                {
                    var losingcard = deck2[0];
                    var winningcard = deck1[0];
                    deck1.RemoveAt(0);
                    deck2.RemoveAt(0);
                    deck1.Add(winningcard);
                    deck1.Add(losingcard);
                } else
                {
                    var losingcard = deck1[0];
                    var winningcard = deck2[0];
                    deck1.RemoveAt(0);
                    deck2.RemoveAt(0);
                    deck2.Add(winningcard);
                    deck2.Add(losingcard);
                }
            }

            var winner = deck1.Count != 0 ? deck1 : deck2;
            winner.Reverse();
            int count = 0;
            long result = 0;
            foreach (var c in winner)
            {
                count++;
                result += c * count;
            }
            return result.ToString();
        }

        public void Print(Queue<int> d1, Queue<int> d2)
        {
            Console.WriteLine("\n\nPlayer 1: {0}\nPlayer 2: {1}", String.Join(", ", d1), String.Join(", ", d2));
        }

        Dictionary<int[], bool> memo = new Dictionary<int[], bool>();

        public (bool, Queue<int>) SubCombat(Queue<int> subdeck1, Queue<int> subdeck2, bool firstgame)
        {
            HashSet<int[]> previousgames = new HashSet<int[]>();

            //Console.WriteLine("\nStarting Subgame\n");

            //if (firstgame) Print(subdeck1, subdeck2);
            bool autowin = false;
            while (subdeck1.Count != 0 && subdeck2.Count != 0)
            {
                var deck1card = subdeck1.Dequeue();
                var deck2card = subdeck2.Dequeue();

                if (deck1card <= subdeck1.Count && deck2card <= subdeck2.Count)
                {
                    if (SubCombat(new Queue<int>(subdeck1.Take(deck1card)), new Queue<int>(subdeck2.Take(deck2card)), false).Item1)
                    {
                        subdeck1.Enqueue(deck1card);
                        subdeck1.Enqueue(deck2card);
                    } else
                    {
                        subdeck2.Enqueue(deck2card);
                        subdeck2.Enqueue(deck1card);
                    }
                }
                else if (deck1card > deck2card)
                {
                    subdeck1.Enqueue(deck1card);
                    subdeck1.Enqueue(deck2card);
                }
                else
                {
                    subdeck2.Enqueue(deck2card);
                    subdeck2.Enqueue(deck1card);
                }

                var gamedata = subdeck1.Union(new int[] {0}).Union(subdeck2).ToArray();
                if (previousgames.Any(p => p.SequenceEqual(gamedata)))
                {
                    autowin = true;
                    break;
                }                
                else previousgames.Add(gamedata);
                
                //if (firstgame) Print(subdeck1, subdeck2);
            }

            var winner = autowin || subdeck1.Count != 0 ? true : false;
            Queue<int> winningdeck = null;
            if (firstgame) winningdeck = autowin || subdeck1.Count != 0 ? subdeck1 : subdeck2;

            return (winner, winningdeck);
        }

        public override string Solve_2()
        {
            var deck = SubCombat(deck1a, deck2a, true);
            var winner = deck.Item2.ToList();
            winner.Reverse();
            int count = 0;
            long result = 0;
            foreach (var c in winner)
            {
                count++;
                result += c * count;
            }
            return result.ToString();
        }
    }
}
