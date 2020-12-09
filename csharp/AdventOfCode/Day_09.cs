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
    public class Day_09 : BaseDay
    {
        List<long> nums;
        long elcount;
        int preamble = 25;

        public Day_09()
        {
            nums = File.ReadLines(InputFilePath).Select(d => long.Parse(d)).ToList();
            elcount = nums.Count;
        }

        public bool ContainsSums(int idx)
        {
            for (int i = idx - preamble; i < elcount; i++)
            {
                for (int j = idx - preamble; j < elcount; j++)
                {
                    if (i == j) continue;
                    if (nums[i] + nums[j] == nums[idx]) return true;
                }
            }
            return false;
        }

        public override string Solve_1()
        {
            for (int i = preamble; i < elcount; i++)
            {
                if (!ContainsSums(i)) return nums[i].ToString();
            }
            return "";
        }

        public override string Solve_2()
        {
            long invalid = 0;
            for (int i = preamble; i < elcount; i++)
            {
                if (!ContainsSums(i)) invalid = nums[i];
            }

            int found = -1;
            for (int i = 0; i < elcount; i++)
            {
                long workingsum = 0;
                for (int j = i; j < elcount; j++)
                {
                    workingsum += nums[j];
                    if (workingsum > invalid) break;
                    if (workingsum == invalid)
                    {
                        found = j;
                        break;
                    }
                }

                if (found > -1)
                {
                    var contig = nums.GetRange(i, found - i + 1);
                    return (contig.Min() + contig.Max()).ToString();
                }
            }

            

            return "";
        }
    }
}
