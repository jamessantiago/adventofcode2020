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
    public class Day_20 : BaseDay
    {
        Dictionary<int, List<string>> tiles;
        Dictionary<int, char[,]> fulltiles = new Dictionary<int, char[,]>();
        public enum side { top, bottom, left, right};

        public Day_20()
        {
            var images = File.ReadAllText(InputFilePath).Split("\n\n");
            tiles = new Dictionary<int, List<string>>();
            foreach (var image in images)
            {
                var imagelines = image.Split('\n');
                int id = int.Parse(Regex.Match(imagelines[0], @"Tile (\d+)").Groups[1].Value);
                List<string> edges = new List<string>();
                edges.Add(imagelines[1]);
                edges.Add(imagelines[imagelines.Length - 1]);
                string left = "", right = "";
                int linelength = imagelines[1].Length;
                for (int i = 1; i < imagelines.Length; i++)
                {
                    left += imagelines[i][0];
                    right += imagelines[i][linelength - 1];
                }
                edges.Add(left);
                edges.Add(right);
                tiles.Add(id, edges);

                char[,] imagearray = new char[linelength, linelength];
                for (int i = 0; i < linelength; i++)
                {
                    for (int j = 0; j < linelength; j++)
                    {
                        imagearray[j, i] = imagelines[i + 1][j];
                    }
                }
                fulltiles.Add(id, imagearray);
            }
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public bool EdgesMatch (int a, int b)
        {
            return tiles[a].Any(d => tiles[b].Any(p => d == p || d == Reverse(p)));
        }

        public int EdgeMatchCount(int tile)
        {
            return tiles[tile].Count(d => tiles.Any(p => p.Key != tile && p.Value.Any(k => k == d || k == Reverse(d))));
        }

        public bool HasMatch(int tile, string side)
        {
            return tiles.Any(d => d.Key != tile && d.Value.Any(p => p == side || p == Reverse(side)));
        }

        public override string Solve_1()
        {
            var corners = tiles.Where(d => EdgeMatchCount(d.Key) == 2);
            long product = 1;
            foreach (var c in corners) product *= c.Key;
            return product.ToString();
        }

        public char[,] Rotate(char[,] image2d)
        {
            int size = image2d.GetLength(0);
            char[,] newimage = new char[size, size];

            for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
            {
                newimage[i, j] = image2d[size - j - 1, i];
            }

            return newimage;
        }

        public char[,] FlipVert(char[,] image2d)
        {
            int size = image2d.GetLength(0);
            char[,] newimage = new char[size, size];            

            for (int i = 0; i < size; i++)
            {
                int nj = 0;
                for (int j = size - 1; j >= 0; j--)
                {
                    newimage[i, nj] = image2d[i, j];
                    nj++;
                }                
            }
            return newimage;
        }

        public char[,] FlipHoriz(char[,] image2d)
        {
            int size = image2d.GetLength(0);
            char[,] newimage = new char[size, size];

            int ni = 0;
            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = 0; j < size; j++)
                {
                    newimage[ni, j] = image2d[i, j];
                }
                ni++;
            }
            return newimage;
        }

        public string GetSide(char[,] image2d, side s)
        {
            string edge = "";
            int size = image2d.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                if (s == side.top)
                {
                    edge += image2d[i, 0];
                }
                else if (s == side.bottom)
                {
                    edge += image2d[i, size - 1];
                }
                else if (s == side.left) 
                {
                    edge += image2d[0, i];
                } 
                else if (s == side.right)
                {
                    edge += image2d[size - 1, i];
                }
            }

            return edge;
        }

        public void OrientCorner(int corner)
        {
            var top = GetSide(fulltiles[corner], side.top);
            var left = GetSide(fulltiles[corner], side.left);
            while (HasMatch(corner, top) || HasMatch(corner, left))
            {
                fulltiles[corner] = Rotate(fulltiles[corner]);
                top = GetSide(fulltiles[corner], side.top);
                left = GetSide(fulltiles[corner], side.left);
            }
        }

        public int FindTopMatchTo(int tile)
        {
            string tilebottom = GetSide(fulltiles[tile], side.bottom);
            var matchingtile = tiles.Where(d => d.Key != tile && d.Value.Any(e => e == tilebottom || e == Reverse(tilebottom)));
            if (matchingtile.Count() != 1) throw new Exception();
            var mkey = matchingtile.First().Key;
            string matchingtiletop = GetSide(fulltiles[mkey], side.top);
            while (matchingtiletop != tilebottom && matchingtiletop != Reverse(tilebottom))
            {
                fulltiles[mkey] = Rotate(fulltiles[mkey]);
                matchingtiletop = GetSide(fulltiles[mkey], side.top);
            }

            if (matchingtiletop != tilebottom)
            {
                fulltiles[mkey] = FlipHoriz(fulltiles[mkey]);
            }

            return mkey;
        }

        public int FindLeftMatchTo(int tile)
        {
            string tileright = GetSide(fulltiles[tile], side.right);
            var matchingtile = tiles.Where(d => d.Key != tile && d.Value.Any(e => e == tileright || e == Reverse(tileright)));
            if (matchingtile.Count() != 1) throw new Exception();
            var mkey = matchingtile.First().Key;
            string matchingtileleft = GetSide(fulltiles[mkey], side.left);
            while (matchingtileleft != tileright && matchingtileleft != Reverse(tileright))
            {
                fulltiles[mkey] = Rotate(fulltiles[mkey]);
                matchingtileleft = GetSide(fulltiles[mkey], side.left);
            }

            if (matchingtileleft != tileright)
            {
                fulltiles[mkey] = FlipVert(fulltiles[mkey]);
            }

            return mkey;
        }

        public int[,] StichImage(int corner, int size)
        {
            int[,] image = new int[size, size];
            image[0, 0] = corner;
            for (int i = 0; i < size; i ++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (i == 0)
                    {
                        image[i, j] = FindTopMatchTo(image[i, j - 1]);
                    } else
                    {
                        image[i, j] = FindLeftMatchTo(image[i - 1, j]);
                    }
                }
            }

            return image;
        }

        public char[,] RemoveSides(char[,] tile)
        {            
            int tilesize = tile.GetLength(0);
            char[,] newtile = new char[tilesize - 2, tilesize - 2];

            for (int x = 1; x < tilesize - 1; x++)
            {
                for (int y = 1; y < tilesize - 1; y++)
                {
                    newtile[x - 1, y - 1] = tile[x, y];
                }
            }
            return newtile;
        }

        public char[,] GenerateImage(int[,] image)
        {
            int tilesize = fulltiles.First().Value.GetLength(0) - 2;
            int imagesize = image.GetLength(0);

            int genimagesize = tilesize * imagesize;
            char[,] genimage = new char[genimagesize, genimagesize];

            for (int i = 0; i < imagesize; i++)
            {
                for (int j = 0; j < imagesize; j++)
                {
                    fulltiles[image[i, j]] = RemoveSides(fulltiles[image[i, j]]);

                    for (int x = 0;  x < tilesize; x++)
                    {
                        for (int y = 0; y < tilesize; y++)
                        {
                            genimage[x + (i * tilesize), y + (j * tilesize)] = fulltiles[image[i, j]][x, y];
                        }
                    }
                }
            }

            return genimage;
        }

        public void PrintImage(char[,] image)
        {
            int isize = image.GetLength(0);
            for (int y = 0; y < isize; y++)
            {
                for (int x = 0; x < isize; x++)
                {
                    Console.Write(image[x, y]);
                }
                Console.WriteLine();
            }
        }

        List<(int, int)> seamonster;
        int seamonheight = 0;
        int seamonwidth = 0;

        public void SetSeaMonster()
        {
            string[] monster = {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   " };
            seamonster = new List<(int,int)>();
            seamonwidth = monster[0].Length;
            seamonheight = monster.Length;
            for (int x = 0; x < monster[0].Length; x++)
            {
                for (int y = 0; y < monster.Length; y++)
                {
                    if (monster[y][x] == '#') seamonster.Add((x,y));
                }
            }
        }

        public int CountMonsters(char[,] image)
        {
            int monsters = 0;

            var imagelen = image.GetLength(0);
            for (int x = 0; x < imagelen - seamonwidth; x++)
            {
                for (int y = 0; y < imagelen - seamonheight; y++)
                {
                    bool ismonster = true;
                    foreach (var sm in seamonster)
                    {
                        if (image[x + sm.Item1, y + sm.Item2] != '#')
                        {
                            ismonster = false;
                            break;
                        }
                    }
                    if (ismonster) monsters++;
                }
            }
            return monsters;
        }

        public int CountRough(char[,] image)
        {
            int imagelen = image.GetLength(0);
            int count = 0;
            for (int i = 0; i < imagelen; i++)
            {
                for (int j = 0; j < imagelen; j++)
                {
                    if (image[i, j] == '#') count++;
                }
            }
            return count;
        }

        public override string Solve_2()
        {
            int l = (int)Math.Sqrt(tiles.Count);
            var corners = tiles.Where(d => EdgeMatchCount(d.Key) == 2);
            var firstcorner = corners.First().Key;
            OrientCorner(firstcorner);
            var image = StichImage(firstcorner, l);
            var genimage = GenerateImage(image);
            //PrintImage(genimage);
            SetSeaMonster();
            int smc = CountMonsters(genimage);
            for (int i = 0; i < 3; i++)
            { 
                if (smc != 0) break;
                genimage = Rotate(genimage);
                smc = CountMonsters(genimage);
                if (smc != 0) break;
                
                genimage = FlipHoriz(genimage);
                smc = CountMonsters(genimage);
                if (smc != 0) break;
                genimage = FlipHoriz(genimage);
            }

            int rough = CountRough(genimage);
            rough -= (smc * seamonster.Count);

            return rough.ToString();
        }
    }
}
