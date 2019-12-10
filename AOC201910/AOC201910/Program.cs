using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC201910
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Angle { get; private set; }
        public bool Vaporized { get; set; } = false;

        public void SetAngle(Point s)
        {
            Angle = (Math.Atan2(Y - s.Y, X - s.X)) * (180.0 / Math.PI);
        }

        private int gcd(int a, int b)
        {
            if (b == 0)
                return a;
            return gcd(b, a % b);
        }

        public List<Point> GetPointsInBetween(Point to)
        {
            List<Point> ret = new List<Point>();

            if (X == to.X)
            {
                if (to.Y > Y)
                    for (int i = Y + 1; i < to.Y; i++)
                    {
                        ret.Add(new Point { X = X, Y = i });
                    }
                if (to.Y < Y)
                    for (int i = to.Y + 1; i < Y; i++)
                    {
                        ret.Add(new Point { X = X, Y = i });
                    }
            }
            else
            if (Y == to.Y)
            {
                if (to.X > X)
                    for (int i = X + 1; i < to.X; i++)
                    {
                        ret.Add(new Point { X = i, Y = Y });
                    }
                if (to.X < X)
                    for (int i = to.X + 1; i < X; i++)
                    {
                        ret.Add(new Point { X = i, Y = Y });
                    }
            }
            else
            {
                int dx = Math.Abs(to.X - X);
                int dy = Math.Abs(to.Y - Y);
                int a = gcd(dx, dy);

                if (a > 1)
                {
                    int f = X < to.X ? 1 : -1;
                    int f2 = Y < to.Y ? 1 : -1;
                    int sx = dx / a;
                    int sy = dy / a;

                    for (int i = 1; i < a; i++)
                    {
                        var nextX = X + f * i * sx;
                        var nextY = Y + f2 * i * sy;

                        ret.Add(new Point { X = (int)nextX, Y = (int)nextY });
                    }
                }                
            }
            return ret;
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            int maxx = lines.First().Length;
            int maxy = lines.Length;


            Point laser = null;
            List<Point> asteroids = new List<Point>();
            int max = 0;

            for (int x = 0; x < maxx; x++)
                for (int y = 0; y < maxy; y++)
                {
                    if (lines[y][x] == '#')
                    {

                        asteroids.Add(new Point { X = x, Y = y });
                    }
                }

            foreach (Point baseAsteroid in asteroids)
            {
                if (baseAsteroid.Y == 4)
                {

                }
                int visibleAsteroidNum = 0;
                foreach (Point targetAsteroid in asteroids)
                {
                    if (targetAsteroid.X == baseAsteroid.X && targetAsteroid.Y == baseAsteroid.Y)
                        continue;

                    var middlePoints = baseAsteroid.GetPointsInBetween(targetAsteroid);
                    bool visible = true;
                    foreach (var mp in middlePoints)
                    {
                        if (lines[mp.Y][mp.X] == '#')
                        {
                            visible = false;
                            break;
                        }
                    }
                    if (visible)
                    {
                        visibleAsteroidNum++;
                    }
                }

                if (visibleAsteroidNum > max)
                {
                    max = visibleAsteroidNum;
                    laser = baseAsteroid;
                }
            }

            Console.WriteLine(max);

            foreach (var a in asteroids)
            {                
                a.SetAngle(laser);
            }

            var oa = asteroids.Where(a => a.X != laser.X || a.Y != laser.Y).OrderBy(a => a.Angle).ToList();
            double mindiff = double.MaxValue;
            int minidx = -1;
            var idx = 0;
            foreach(var a in oa)
            {
                var diff = Math.Abs(a.Angle + 90);
                if(diff < mindiff)
                {
                    mindiff = diff;
                    minidx = idx;
                }

                idx++;
            }

            double lastAngle = mindiff;
            int lastIdx = minidx;
            List<Point> vas = new List<Point>();

            while(!oa.All(a=>a.Vaporized))
            {
                bool skip = false;
                if(oa[lastIdx].Vaporized)
                {
                    bool found = false;
                    var nvi = (lastIdx + 1) % oa.Count;
                    while(oa[nvi].Angle == oa[lastIdx].Angle)
                    {
                        if(!oa[nvi].Vaporized)
                        {
                            found = true;
                            break;
                        }
                        nvi = (nvi + 1) % oa.Count;
                    }

                    if (found)
                    {
                        lastIdx = nvi;
                    }
                    else
                    {
                        skip = true;
                    }
                }

                if (!skip)
                {
                    bool hit = false;
                    var bps = laser.GetPointsInBetween(oa[lastIdx]);
                    foreach (var bp in bps)
                    {
                        var va = asteroids.SingleOrDefault(p => p.X == bp.X && p.Y == bp.Y && p.Vaporized == false);
                        if (va != null)
                        {
                            hit = true;
                            va.Vaporized = true;
                            vas.Add(va);
                            break;
                        }
                    }

                    if (!hit)
                    {
                        hit = true;
                        oa[lastIdx].Vaporized = true;
                        vas.Add(oa[lastIdx]);
                    }
                }
                var ni = (lastIdx + 1) % oa.Count;
                while (oa[ni].Angle == oa[lastIdx].Angle)
                {
                    ni = (ni + 1) % oa.Count;
                }
                lastIdx = ni;
            }

            Console.WriteLine($"X: {vas[199].X}, Y: {vas[199].Y}");
            Console.ReadLine();
        }
    }
}