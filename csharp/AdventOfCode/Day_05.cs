using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoCHelper;
using System.IO;

namespace AdventOfCode
{
    public class Day_05 : BaseDay
    {
        List<char[]> _input;

        public Day_05()
        {
            _input = File.ReadLines(InputFilePath).Select(d => d.ToCharArray()).ToList();
        }

        public override string Solve_1()
        {
            long maxid = 0;
            foreach (var seat in _input)
            {
                int lbrow = 0;
                int ubrow = 127;
                for (int i = 0; i < 6; i++)
                {
                    if (seat[i] == 'F') ubrow -= (int)Math.Ceiling((double)(ubrow - lbrow) / (double)2);
                    else lbrow += (int)Math.Ceiling((double)(ubrow - lbrow) / (double)2);
                }
                int row = seat[6] == 'F' ? lbrow : ubrow;

                int lbcol = 0;
                int ubcol = 7;
                for (int i = 7; i < 9; i++)
                {
                    if (seat[i] == 'L') ubcol -= (int)Math.Ceiling((double)(ubcol - lbcol) / (double)2);
                    else lbcol += (int)Math.Ceiling((double)(ubcol - lbcol) / (double)2);
                }
                int col = seat[9] == 'L' ? lbcol : ubcol;

                long seatid = row * 8 + col;
                maxid = Math.Max(seatid, maxid);
            }

            return maxid.ToString();
        }

        public override string Solve_2()
        {
            List<long> seats = new List<long>();
            long maxid = 0;
            foreach (var seat in _input)
            {
                int lbrow = 0;
                int ubrow = 127;
                for (int i = 0; i < 6; i++)
                {
                    if (seat[i] == 'F') ubrow -= (int)Math.Ceiling((double)(ubrow - lbrow) / (double)2);
                    else lbrow += (int)Math.Ceiling((double)(ubrow - lbrow) / (double)2);
                }
                int row = seat[6] == 'F' ? lbrow : ubrow;

                int lbcol = 0;
                int ubcol = 7;
                for (int i = 7; i < 9; i++)
                {
                    if (seat[i] == 'L') ubcol -= (int)Math.Ceiling((double)(ubcol - lbcol) / (double)2);
                    else lbcol += (int)Math.Ceiling((double)(ubcol - lbcol) / (double)2);
                }
                int col = seat[9] == 'L' ? lbcol : ubcol;

                long seatid = row * 8 + col;
                seats.Add(seatid);
                maxid = Math.Max(seatid, maxid);
            }

            seats.Sort();

            int seatcount = seats.Count;
            long foundseat = 0;
            for (int i = 1; i < seatcount; i++)
            {
                if (seats[i] - 1 != seats[i - 1]) foundseat = seats[i] - 1;
            }

            return foundseat.ToString();
        }
    }
}
