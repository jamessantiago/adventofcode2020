using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day_11 : BaseDay
    {
        char[,] seats;
        int xlen = 0;
        int ylen = 0;

        public Day_11()
        {
            var lines = File.ReadLines(InputFilePath).ToList();
            xlen = lines[0].Length;
            ylen = lines.Count;
            seats = new char[xlen,ylen];
            for (int x = 0; x < xlen; x++)
            {
                for (int y = 0; y < ylen; y++)
                {
                    seats[x,y] = lines[y][x];
                }                
            }
        }

        public char[,] CalculateNextSeats(char[,] curseats, ref bool statechanged)
        {
            char[,] newseats = new char[xlen, ylen];
            statechanged = false;
            for (int x = 0; x < xlen; x++)
            {
                for (int y = 0; y < ylen; y++)
                {
                    if (curseats[x,y] == '.')
                    {
                        newseats[x,y] = '.';
                        continue;
                    }

                    int numoccupied = 0;
                    bool canleft = x > 0;
                    bool canright = x < xlen - 1;
                    bool canup = y > 0;
                    bool candown = y < ylen - 1;

                    if (canleft) numoccupied += curseats[x - 1, y] == '#' ? 1 : 0; //left
                    if (canright) numoccupied += curseats[x + 1, y] == '#' ? 1 : 0; //right


                    if (canup) numoccupied += curseats[x, y - 1] == '#' ? 1 : 0; //up
                    if (candown) numoccupied += curseats[x, y + 1] == '#' ? 1 : 0; //down

                    if (canleft && canup) numoccupied += curseats[x - 1, y - 1] == '#' ? 1 : 0;
                    if (canleft && candown) numoccupied += curseats[x - 1, y + 1] == '#' ? 1 : 0;
                    if (canright && canup) numoccupied += curseats[x + 1, y - 1] == '#' ? 1 : 0;
                    if (canright && candown) numoccupied += curseats[x + 1, y + 1] == '#' ? 1 : 0;

                    if (numoccupied == 0 && curseats[x,y] == 'L')
                    {
                        newseats[x,y] = '#';
                        statechanged = true;
                    }
                    else if (numoccupied >= 4 && curseats[x, y] == '#')
                    {
                        newseats[x,y] = 'L';
                        statechanged = true;
                    }
                    else newseats[x, y] = curseats[x, y];
                }
            }

            return newseats;
        }

        public void PrintSeats(char[,] curseats)
        {
            Console.WriteLine();
            for (int y = 0; y < ylen; y++)
            {
                for (int x = 0; x < xlen; x++)
                {
                    Console.Write("{0} ", curseats[x, y]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public override string Solve_1()
        {
            bool statechanged = true;
            var seats1 = new char[xlen,ylen];
            for (int x = 0; x < xlen; x++)
            {
                for (int y = 0; y < ylen; y++)
                {
                    seats1[x, y] = seats[x,y];
                }
            }

            while (statechanged == true)
            {
                //PrintSeats(seats);
                seats1 = CalculateNextSeats(seats1, ref statechanged);
            }

            int totalocc = 0;
            for (int x = 0; x < xlen; x++)
            {
                for (int y = 0; y < ylen; y++)
                {
                    if (seats1[x, y] == '#') totalocc++;
                }
            }
            return totalocc.ToString();
        }



        public int CountFirstSeen(char[,] curseats, int x, int y)
        {
            int occupied = 0;
            int tmpx = x;
            int tmpy = y;
            while (tmpx - 1 >= 0)
            {
                tmpx--;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            tmpx = x;
            while (tmpx + 1 < xlen)
            {
                tmpx++;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            tmpx = x;
            tmpy = y;
            while (tmpy + 1 < ylen)
            {
                tmpy++;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            tmpx = x;
            tmpy = y;
            while (tmpy - 1 >= 0)
            {
                tmpy--;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            tmpx = x;
            tmpy = y;
            while (tmpx - 1 >= 0 && tmpy - 1 >= 0)
            {
                tmpx--;
                tmpy--;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            tmpx = x;
            tmpy = y;
            while (tmpx - 1 >= 0 && tmpy + 1 < ylen)
            {
                tmpx--;
                tmpy++;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            tmpx = x;
            tmpy = y;
            while (tmpx + 1 < xlen && tmpy - 1 >= 0)
            {
                tmpx++;
                tmpy--;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            tmpx = x;
            tmpy = y;
            while (tmpx + 1 < xlen && tmpy + 1 < ylen)
            {
                tmpx++;
                tmpy++;
                if (curseats[tmpx, tmpy] == 'L') break;
                else if (curseats[tmpx, tmpy] == '#')
                {
                    occupied++;
                    break;
                }
            }

            return occupied;
        }

        public int CountAdjacent(char[,] curseats, int x, int y)
        {
            int numoccupied = 0;
            bool canleft = x > 0;
            bool canright = x < xlen - 1;
            bool canup = y > 0;
            bool candown = y < ylen - 1;

            if (canleft) numoccupied += curseats[x - 1, y] == '#' ? 1 : 0; //left
            if (canright) numoccupied += curseats[x + 1, y] == '#' ? 1 : 0; //right


            if (canup) numoccupied += curseats[x, y - 1] == '#' ? 1 : 0; //up
            if (candown) numoccupied += curseats[x, y + 1] == '#' ? 1 : 0; //down

            if (canleft && canup) numoccupied += curseats[x - 1, y - 1] == '#' ? 1 : 0;
            if (canleft && candown) numoccupied += curseats[x - 1, y + 1] == '#' ? 1 : 0;
            if (canright && canup) numoccupied += curseats[x + 1, y - 1] == '#' ? 1 : 0;
            if (canright && candown) numoccupied += curseats[x + 1, y + 1] == '#' ? 1 : 0;

            return numoccupied;
        }

        public char[,] CalculateNextSeats2(char[,] curseats, ref bool statechanged)
        {
            char[,] newseats = new char[xlen, ylen];
            statechanged = false;
            for (int x = 0; x < xlen; x++)
            {
                for (int y = 0; y < ylen; y++)
                {
                    if (curseats[x, y] == '.')
                    {
                        newseats[x, y] = '.';
                    } else if (curseats[x, y] == 'L' && CountFirstSeen(curseats, x, y) == 0)
                    {
                        newseats[x, y] = '#';
                        statechanged = true;
                    }
                    else if (curseats[x, y] == '#' && CountFirstSeen(curseats, x, y) >= 5)
                    {
                        newseats[x, y] = 'L';
                        statechanged = true;
                    }
                    else newseats[x, y] = curseats[x, y];
                }
            }

            return newseats;
        }



        public override string Solve_2()
        {
            bool statechanged = true;
            while (statechanged == true)
            {
                //PrintSeats(seats);
                seats = CalculateNextSeats2(seats, ref statechanged);
            }

            int totalocc = 0;
            for (int x = 0; x < xlen; x++)
            {
                for (int y = 0; y < ylen; y++)
                {
                    if (seats[x, y] == '#') totalocc++;
                }
            }
            return totalocc.ToString();
        }
    }
}
