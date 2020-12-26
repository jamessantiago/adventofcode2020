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
using System.Numerics;

namespace AdventOfCode
{
    public class Day_25 : BaseDay
    {
        long cardpub = 0;
        long doorpub = 0;

        public Day_25()
        {
            var lines = File.ReadAllLines(InputFilePath);
            cardpub = long.Parse(lines[0]);
            doorpub = long.Parse(lines[1]);
        }          
        
        public override string Solve_1()
        {
            int cardloop = 0;
            do
            {
                cardloop++;
            } while (cardpub != BigInteger.ModPow(7, cardloop, 20201227));

            return BigInteger.ModPow(doorpub, cardloop, 20201227).ToString();
        }

        public override string Solve_2()
        {
            return "";
        }
    }
}
