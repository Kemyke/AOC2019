using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC201924
{
    class Program
    {
        class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }
            public bool IsBug { get; set; }
        }

        class Map
        {
            public Map(int level)
            {
                Level = level;
                Layout = new List<Tile>();
                
            }

            public void InitEmpty()
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        Layout.Add(new Tile { X = x, Y = y, IsBug = false });
                    }
                }
            }

            public List<Tile> Layout { get; set; }
            public int Level { get; private set; }

            private int DeeperBugs(string dir)
            {
                int ret = 0;
                Map dl;
                if(levels.TryGetValue(Level - 1, out dl))
                {
                    if(dir == "8")
                    {
                        ret += dl.Layout.Single(t => t.X == 0 && t.Y == 0).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 1 && t.Y == 0).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 2 && t.Y == 0).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 3 && t.Y == 0).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 4 && t.Y == 0).IsBug ? 1 : 0;
                    }
                    else if (dir == "18")
                    {
                        ret += dl.Layout.Single(t => t.X == 0 && t.Y == 4).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 1 && t.Y == 4).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 2 && t.Y == 4).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 3 && t.Y == 4).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 4 && t.Y == 4).IsBug ? 1 : 0;
                    }
                    else if (dir == "12")
                    {
                        ret += dl.Layout.Single(t => t.X == 0 && t.Y == 0).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 0 && t.Y == 1).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 0 && t.Y == 2).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 0 && t.Y == 3).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 0 && t.Y == 4).IsBug ? 1 : 0;
                    }
                    else if (dir == "14")
                    {
                        ret += dl.Layout.Single(t => t.X == 4 && t.Y == 0).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 4 && t.Y == 1).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 4 && t.Y == 2).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 4 && t.Y == 3).IsBug ? 1 : 0;
                        ret += dl.Layout.Single(t => t.X == 4 && t.Y == 4).IsBug ? 1 : 0;
                    }
                    else
                    {
                        throw new Exception(dir);
                    }
                }
                return ret;
            }

            private int UpperBugs(string dir)
            {
                int ret = 0;
                Map dl;
                if (levels.TryGetValue(Level + 1, out dl))
                {
                    if (dir == "u")
                    {
                        ret += dl.Layout.Single(t => t.X == 2 && t.Y == 1).IsBug ? 1 : 0;
                    }
                    else if (dir == "d")
                    {
                        ret += dl.Layout.Single(t => t.X == 2 && t.Y == 3).IsBug ? 1 : 0;
                    }
                    else if (dir == "l")
                    {
                        ret += dl.Layout.Single(t => t.X == 1 && t.Y == 2).IsBug ? 1 : 0;
                    }
                    else if (dir == "r")
                    {
                        ret += dl.Layout.Single(t => t.X == 3 && t.Y == 2).IsBug ? 1 : 0;
                    }
                    else
                    {
                        throw new Exception(dir);
                    }
                }
                return ret;
            }


            public Map Evolve()
            {
                Map ret = new Map(Level);
                
                foreach(Tile t in Layout)
                {
                    var l = Layout.SingleOrDefault(tt => tt.X == t.X - 1 && tt.Y == t.Y);
                    var r = Layout.SingleOrDefault(tt => tt.X == t.X + 1 && tt.Y == t.Y);
                    var u = Layout.SingleOrDefault(tt => tt.X == t.X && tt.Y == t.Y - 1);
                    var d = Layout.SingleOrDefault(tt => tt.X == t.X && tt.Y == t.Y + 1);

                    int n = 0;
                    if (l != null)
                    {
                        if (l.X == 2 && l.Y == 2)
                        {
                            n += DeeperBugs("14");
                        }
                        else if(l.IsBug)
                        {
                            n++;
                        }
                    }
                    else if (l == null)
                    {
                        n += UpperBugs("l");
                    }


                    if (r != null)
                    {
                        if (r.X == 2 && r.Y == 2)
                        {
                            n += DeeperBugs("12");
                        }
                        else if(r.IsBug)
                        {
                            n++;
                        }
                    }
                    else if (r == null)
                    {
                        n += UpperBugs("r");
                    }


                    if (u != null)
                    {
                        if (u.X == 2 && u.Y == 2)
                        {
                            n += DeeperBugs("18");
                        }
                        else if(u.IsBug)
                        {
                            n++;
                        }
                    }
                    else if (u == null)
                    {
                        n += UpperBugs("u");
                    }

                    if (d != null)
                    {
                        if (d.X == 2 && d.Y == 2)
                        {
                            n += DeeperBugs("8");
                        }
                        else if (d.IsBug)
                        {
                            n++;
                        }
                    }
                    else if (d == null)
                    {
                        n += UpperBugs("d");
                    }

                    if (t.IsBug)
                    {
                        if (n == 1)
                        {
                            ret.Layout.Add(new Tile { X = t.X, Y = t.Y, IsBug = true });
                        }
                        else
                        {
                            ret.Layout.Add(new Tile { X = t.X, Y = t.Y, IsBug = false });
                        }

                    }
                    else
                    {
                        if (n == 1 || n == 2)
                        {
                            ret.Layout.Add(new Tile { X = t.X, Y = t.Y, IsBug = true });
                        }
                        else
                        {
                            ret.Layout.Add(new Tile { X = t.X, Y = t.Y, IsBug = false });
                        }
                    }
                }

                return ret;
            }

            public void Draw()
            {
                for(int y = 0; y < 5; y++)
                {
                    for(int x = 0; x < 5; x++)
                    {
                        Console.Write(Layout.Single(t => t.X == x && t.Y == y).IsBug ? "#" : ".");
                    }
                    Console.WriteLine();
                }
            }
        }

        static Dictionary<int, Map> levels = new Dictionary<int, Map>();

        static void Parse(string[] lines)
        {
            levels[0].Layout.Clear();
            int y = 0;
            int x = 0;
            foreach(var line in lines)
            {
                foreach(var tile in line)
                {
                    levels[0].Layout.Add(new Tile { X = x, Y = y, IsBug = (tile == '#') });
                    x++;
                }
                x = 0;
                y++;
            }
        }

        static long CalcBugs()
        {
            long ret = 0;
            foreach(var m in levels.Values)
            {
                foreach(var t in m.Layout)
                {
                    if(t.X == 2 && t.Y == 2)
                    {
                        continue;
                    }
                    if(t.IsBug)
                    {
                        ret++;
                    }
                }
            }
            return ret;
        }

        static void Main(string[] args)
        {
            for (int i = -200; i <= 200; i++)
            {
                levels.Add(i, new Map(i));
                levels[i].InitEmpty();
            }

            Parse(File.ReadAllLines("input.txt"));
            for(int i = 0; i < 200; i++)
            {
                Dictionary<int, Map> newStates = new Dictionary<int, Map>();
                foreach(var map in levels.Values)
                {
                    if(map.Level == 1)
                    {

                    }
                    var nm = map.Evolve();
                    newStates.Add(nm.Level, nm);
                }

                levels = newStates;
            }

            for(int i = -5; i <= 5; i++)
            {
                Console.WriteLine("Level: "+i);
                levels[i].Draw();
                Console.WriteLine("----");
            }

            Console.WriteLine(CalcBugs());
            Console.ReadLine();
        }
    }
}
