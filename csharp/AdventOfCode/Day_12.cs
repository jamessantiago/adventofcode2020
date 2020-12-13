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
    public class Day_12 : BaseDay
    {
        List<(char, long)> commands = new List<(char, long)>();

        public Day_12()
        {
            commands = File.ReadLines(InputFilePath).Select(d => (d[0], long.Parse(d.Substring(1)))).ToList();
        }

        public (long, long) Move(long x, long y, char direction, long dist)
        {
            switch (direction)
            {
                case 'N':
                    y -= dist;
                    break;
                case 'S':
                    y += dist;
                    break;
                case 'E':
                    x += dist;
                    break;
                case 'W':
                    x -= dist;         
                    break;
                default:
                    break;
            }
            return (x, y);
        }

        public char GetDirection(long heading)
        {
            if (heading == 0) return 'E';
            if (heading == 90) return 'S';
            if (heading == 180) return 'W';
            if (heading == 270) return 'N';
            throw new Exception();
        }

        public override string Solve_1()
        {
            long heading = 0;
            long x = 0, y = 0;

            foreach (var command in commands)
            {
               if (command.Item1 == 'R')
                {
                    heading = (heading + command.Item2) % 360;
                } else if (command.Item1 == 'L')
                {
                    heading += 360;
                    heading = (heading - command.Item2) % 360;
                } else if (command.Item1 == 'F')
                {
                    (x, y) = Move(x, y, GetDirection(heading), command.Item2);
                } else
                {
                    (x, y) = Move(x, y,command.Item1, command.Item2);
                }
            }

            return (Math.Abs(x) + Math.Abs(y)).ToString();

        }

        public long GetDistance(long x1, long y1, long x2, long y2)
        {
            return (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
        }

        public double GetDistanceExact(long x1, long y1, long x2, long y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public (long, long) MoveWaypolongAround(long x, long y, double radius, double heading)
        {
            long x2 = (long)(x + radius * Math.Cos(DegreeToRadian(heading)));
            long y2 = (long)(y + radius * Math.Sin(DegreeToRadian(heading)));
            return (x2, y2);
        }

        public char GetWaypolongDirection(long shipx, long shipy, long wpx, long wpy)
        {
            if (shipx < wpx) return 'E';
            if (shipx > wpx) return 'W';
            if (shipy > wpy) return 'S';
            if (shipy < wpy) return 'N';
            throw new Exception();
        }

        public (long, long) GetOffsets(long shipx, long shipy, long wpx, long wpy)
        {
            return ((wpx - shipx), (wpy - shipy));
        }

        public double GetHeading(long shipx, long shipy, long wpx, long wpy)
        {
            long xdiff = wpx - shipx;
            long ydiff = wpy - shipy;
            return (Math.Atan2(ydiff, xdiff) * 180.0 / Math.PI);
            //return (360 + heading) % 360;
        }

        public override string Solve_2()
        {
            long shipx = 0, shipy = 0;
            long wpx = 10, wpy = -1;

            foreach (var command in commands)
            {
                if (command.Item1 == 'R')
                {
                    double r = GetDistanceExact(shipx, shipy, wpx, wpy);
                    double curheading = GetHeading(shipx, shipy, wpx, wpy);
                    double newheading = (curheading + command.Item2) % 360; 
                    (wpx, wpy) = MoveWaypolongAround(shipx, shipy, r, newheading);
                }
                else if (command.Item1 == 'L')
                {
                    double r = GetDistanceExact(shipx, shipy, wpx, wpy);
                    double curheading = GetHeading(shipx, shipy, wpx, wpy);
                    double newheading = ((curheading + 360) - command.Item2) % 360;
                    (wpx, wpy) = MoveWaypolongAround(shipx, shipy, r, newheading);
                }
                else if (command.Item1 == 'F')
                {
                    long offsetx = 0, offsety = 0;
                    (offsetx, offsety) = GetOffsets(shipx, shipy, wpx, wpy);
                    long distance = GetDistance(shipx, shipy, wpx, wpy);
                    shipx += offsetx * command.Item2;
                    shipy += offsety * command.Item2;
                    wpx = offsetx + shipx; wpy = offsety + shipy;
                }
                else
                {
                    (wpx, wpy) = Move(wpx, wpy, command.Item1, command.Item2);
                }
            }

            return (Math.Abs(shipx) + Math.Abs(shipy)).ToString();
        }
    }
}
