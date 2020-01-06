using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC201912
{
    class Program
    {
        class Moon
        {
            public Moon(int x, int y, int z) { X = x; Y = y; Z = z; }
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }

            public int vX { get; set; } = 0;
            public int vY { get; set; } = 0;
            public int vZ { get; set; } = 0;

            public void Step()
            {
                X += vX;
                Y += vY;
                Z += vZ;                
            }

            public int Distance(Moon m)
            {
                return Math.Abs(X - m.X) + Math.Abs(Y - m.Y) + Math.Abs(Z - m.Z);
            }

            public string Distances(List<Moon> moons)
            {
                List<int> ret = new List<int>();
                foreach(var m in moons)
                {
                    ret.Add(Distance(m));
                }
                return string.Join(",", ret.OrderBy(d => d).Select(d => d.ToString()));
            }

            public int TotalEnergy
            {
                get
                {
                    var pt = Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
                    var kin = Math.Abs(vX) + Math.Abs(vY) + Math.Abs(vZ);
                    return pt * kin;
                }
            }

            public int KineticEnergy
            {
                get
                {
                    var kin = Math.Abs(vX) + Math.Abs(vY) + Math.Abs(vZ);
                    return kin;
                }
            }

            public override bool Equals(object obj)
            {
                var m = (Moon)obj;
                return m.X == X && m.Y == Y && m.Z == Z && m.vX == vX && m.vY == vY && m.vZ == vZ;
            }

            public Moon Copy()
            {
                return new Moon(X , Y , Z ) { vX = vX, vY = vY, vZ = vZ };
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    // Suitable nullity checks etc, of course :)
                    hash = hash * 23 + X.GetHashCode();
                    hash = hash * 23 + Y.GetHashCode();
                    hash = hash * 23 + Z.GetHashCode();
                    hash = hash * 23 + vX.GetHashCode();
                    hash = hash * 23 + vY.GetHashCode();
                    hash = hash * 23 + vZ.GetHashCode();
                    return hash;
                }
            }
        }

        static void ApplyGravity(Moon m1, Moon m2)
        {
            if(m1.X > m2.X)
            {
                m1.vX -= 1;
                m2.vX += 1;
            }
            else if (m1.X < m2.X)
            {
                m1.vX += 1;
                m2.vX -= 1;
            }

            if (m1.Y > m2.Y)
            {
                m1.vY -= 1;
                m2.vY += 1;
            }
            else if (m1.Y < m2.Y)
            {
                m1.vY += 1;
                m2.vY -= 1;
            }

            if (m1.Z > m2.Z)
            {
                m1.vZ -= 1;
                m2.vZ += 1;
            }
            else if (m1.Z < m2.Z)
            {
                m1.vZ += 1;
                m2.vZ -= 1;
            }
        }

        static string GetConstellation(List<int> distances, List<int> kineticenergies)
        {
            return string.Join("", distances.OrderBy(c => c).Select(d=>d.ToString().PadLeft(3, '0'))) + string.Join("", kineticenergies.Select(k=>k.ToString()));
        }

        static string GetConstellation(List<Moon> moons)
        {
            return string.Join(",", moons.Select(m => m.TotalZEnergy).OrderBy(m => m));
        }

        static List<int> GetDistances(List<Moon> moons)
        {
            List<int> dists = new List<int>();
            foreach (Moon m in moons)
            {
                int mi = moons.IndexOf(m);
                foreach (Moon n in moons)
                {
                    if (moons.IndexOf(n) > mi)
                    {
                        dists.Add(m.Distance(n));
                    }
                }
            }
            return dists;
        }
        
        static void Main(string[] args)
        {
            List<Moon> moons = new List<Moon>();
            moons.Add(new Moon(-5, 6, -11));
            moons.Add(new Moon(-8, -4, -2));
            moons.Add(new Moon(1, 16, 4));
            moons.Add(new Moon(11, 11, -4));

            //moons.Add(new Moon(-1, 0, 2));
            //moons.Add(new Moon(2, -10, -7));
            //moons.Add(new Moon(4, -8, 8));
            //moons.Add(new Moon(3, 5, -1));

            //moons.Add(new Moon(-8, -10, 0));
            //moons.Add(new Moon(5, 5, 10));
            //moons.Add(new Moon(2, -7, 3));
            //moons.Add(new Moon(9, -8, -3));

            bool done = false;
            long i = 1;
            long lasti = 0;
            var orig = moons.Select(m=>m.Copy()).ToList();
            List<long> steps = new List<long>();
            while (!done)
            {
                foreach(Moon m in moons)
                {
                    int mi = moons.IndexOf(m);
                    foreach(Moon n in moons)
                    {
                        if(moons.IndexOf(n) > mi)
                        {
                            ApplyGravity(m, n);
                        }
                    }
                }

                
                foreach (Moon m in moons)
                {
                    m.Step();
                }
                //286332 161428 108344

                if (orig[0].Z == moons[0].Z &&
                    orig[1].Z == moons[1].Z &&
                    orig[2].Z == moons[2].Z &&
                    orig[3].Z == moons[3].Z)
                {
                    Console.WriteLine(i - lasti);
                    lasti = i;
                }

                
                i++;
            }

            Console.ReadKey();
        }
    }
}
