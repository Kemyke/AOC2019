using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace AOC201920
{
    class Program
    {
        class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string Type { get; set; }
            public string IO { get; set; }
        }

        static void Parse(string[] lines)
        {
            for(int lineNum = 0; lineNum < lines.Length; lineNum++)
            {
                var line = lines[lineNum];
                for(int charNum = 0; charNum < line.Length; charNum++)
                {
                    var c = line[charNum];
                    string type = " ";
                    int x = charNum;
                    int y = lineNum;

                    if (c == ' ' || c == '.' || c == '#')
                    {
                        type = c.ToString();
                        map.Add(new Point { Y = y, X = x, Type = type });
                    }
                    else
                    {
                        if (map.SingleOrDefault(p => p.X == x && p.Y == y) == null)
                        {
                            if (charNum < line.Length - 1 && char.IsLetter(line[charNum + 1]))
                            {
                                type = c.ToString() + line[charNum + 1].ToString();
                                if (charNum < line.Length - 2 && line[charNum + 2] == '.')
                                {
                                    x = charNum + 1;
                                }
                            }
                            else if (lineNum < lines.Length - 1 && char.IsLetter(lines[lineNum + 1][charNum]))
                            {
                                type = c.ToString() + lines[lineNum + 1][charNum].ToString();
                                if (lineNum < lines.Length - 2 && lines[lineNum + 2][charNum] == '.')
                                {
                                    y = lineNum + 1;
                                }
                            }
                            string io = "inner";
                            if (x == 1 || x == line.Length - 2 || y == 1 || y == lines.Length - 2)
                            {
                                io = "outer";
                            }
                            map.Add(new Point { Y = y, X = x, Type = type, IO = io });
                        }
                    }
                }
            }

        }

        class State
        {
            public Point Point { get; set; }
            public int Steps { get; set; }
            public int Level { get; set; }
            public List<Tuple<string, int>> Path = new List<Tuple<string, int>>();
        }

        static List<State> GetNewPossibleStates(State currentState)
        {
            List<State> ret = new List<State>();
            Point left = map.SingleOrDefault(p => p.X == currentState.Point.X - 1 && p.Y == currentState.Point.Y);
            if (left != null && left.Type != " " && left.Type != "#")
            {
                if (left.Type == ".")
                {
                    ret.Add(new State { Path = currentState.Path, Level = currentState.Level, Point = new Point { X = currentState.Point.X - 1, Y = currentState.Point.Y }, Steps = currentState.Steps + 1 });
                }
                else
                {
                    var oe = map.Where(p => p.Type == left.Type).SingleOrDefault(p => p != left) ?? left;
                    var level = currentState.Level + 1;
                    if(left.IO == "outer")
                    {
                        level = currentState.Level - 1;
                    }
                    if (level >= 0 || oe.Type == "ZZ")
                    {
                        ret.Add(new State { Path = currentState.Path.Append(new Tuple<string, int>(oe.Type, level)).ToList(), Level = level, Point = new Point { X = oe.X, Y = oe.Y }, Steps = currentState.Steps });
                    }
                }
            }

            Point right = map.SingleOrDefault(p => p.X == currentState.Point.X + 1 && p.Y == currentState.Point.Y);
            if (right != null && right.Type != " " && right.Type != "#")
            {
                if (right.Type == ".")
                {
                    ret.Add(new State { Path = currentState.Path, Level = currentState.Level, Point = new Point { X = currentState.Point.X + 1, Y = currentState.Point.Y }, Steps = currentState.Steps + 1 });
                }
                else
                {
                    var oe = map.Where(p => p.Type == right.Type).SingleOrDefault(p => p != right) ?? right;
                    var level = currentState.Level + 1;
                    if (right.IO == "outer")
                    {
                        level = currentState.Level - 1;
                    }
                    if (level >= 0 || oe.Type == "ZZ")
                    {
                        ret.Add(new State { Path = currentState.Path.Append(new Tuple<string, int>(oe.Type, level)).ToList(), Level = level, Point = new Point { X = oe.X, Y = oe.Y }, Steps = currentState.Steps });
                    }
                }
            }

            Point up = map.SingleOrDefault(p => p.X == currentState.Point.X && p.Y == currentState.Point.Y - 1);
            if (up != null && up.Type != " " && up.Type != "#")
            {
                if (up.Type == ".")
                {
                    ret.Add(new State { Path = currentState.Path, Level = currentState.Level, Point = new Point { X = currentState.Point.X, Y = currentState.Point.Y - 1 }, Steps = currentState.Steps + 1 });
                }
                else
                {
                    var oe = map.Where(p => p.Type == up.Type).SingleOrDefault(p => p != up) ?? up;
                    var level = currentState.Level + 1;
                    if (up.IO == "outer")
                    {
                        level = currentState.Level - 1;
                    }
                    if (level >= 0 || oe.Type == "ZZ")
                    {
                        ret.Add(new State { Path = currentState.Path.Append(new Tuple<string, int>(oe.Type, level)).ToList(), Level = level, Point = new Point { X = oe.X, Y = oe.Y }, Steps = currentState.Steps });
                    }
                }
            }

            Point down = map.SingleOrDefault(p => p.X == currentState.Point.X && p.Y == currentState.Point.Y + 1);
            if (down != null && down.Type != " " && down.Type != "#")
            {
                if (down.Type == ".")
                {
                    ret.Add(new State { Path = currentState.Path, Level = currentState.Level, Point = new Point { X = currentState.Point.X, Y = currentState.Point.Y + 1 }, Steps = currentState.Steps + 1 });
                }
                else
                {
                    var oe = map.Where(p => p.Type == down.Type).SingleOrDefault(p => p != down) ?? down;
                    var level = currentState.Level + 1;
                    if (down.IO == "outer")
                    {
                        level = currentState.Level - 1;
                    }
                    if (level >= 0 || oe.Type == "ZZ")
                    {
                        ret.Add(new State { Path = currentState.Path.Append(new Tuple<string, int>(oe.Type, level)).ToList(), Level = level, Point = new Point { X = oe.X, Y = oe.Y }, Steps = currentState.Steps });
                    }
                }
            }

            return ret;
        }


        static List<Point> map = new List<Point>();
        static Dictionary<int, Dictionary<int, Dictionary<int, int>>> allStates = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();
        //static Dictionary<int, Dictionary<int, int>> allStates = new Dictionary<int, Dictionary<int, int>>();

        static void Main(string[] args)
        {
            Parse(File.ReadAllLines("input.txt"));
            var aa = map.Single(p => p.Type == "AA");
            aa.Type = "#";
            var start = new Point { X = aa.X + 1 , Y = aa.Y };
            var initState = new State { Point = start, Steps = 0, Level = 0 };
            List<State> shouldVisit = new List<State>();

            var ns = GetNewPossibleStates(initState);
            foreach (var s in ns)
            {
                shouldVisit.Add(s);
            }
            
            State endState = null;

            while (shouldVisit.Any())
            {

                var visitState = shouldVisit.OrderBy(k => k.Level).First();
                shouldVisit.Remove(visitState);

                var currP = map.Single(p => p.X == visitState.Point.X && p.Y == visitState.Point.Y);
                if (currP.Type == "ZZ")
                {
                    if (visitState.Level == -1)
                    {
                        if (endState == null || endState.Steps > visitState.Steps)
                        {
                            endState = visitState;
                        }
                    }
                    continue;
                }

                bool cont = true;
                Dictionary<int, Dictionary<int, int>> allStatesInLevel;

                if (allStates.TryGetValue(visitState.Level, out allStatesInLevel))
                {
                    Dictionary<int, int> sls;
                    if (allStatesInLevel.TryGetValue(currP.Y, out sls))
                    {
                        if (sls.ContainsKey(currP.X))
                        {
                            if (sls[currP.X] <= visitState.Steps)
                            {
                                cont = false;
                            }
                            else
                            {
                                sls[currP.X] = visitState.Steps;
                            }
                        }
                        else
                        {
                            allStates[visitState.Level][currP.Y].Add(currP.X, visitState.Steps);
                        }
                    }
                    else
                    {
                        allStates[visitState.Level].Add(currP.Y, new Dictionary<int, int>());
                        allStates[visitState.Level][currP.Y].Add(currP.X, visitState.Steps);
                    }
                }
                else
                {
                    allStates.Add(visitState.Level, new Dictionary<int, Dictionary<int, int>>());
                    allStates[visitState.Level].Add(currP.Y, new Dictionary<int, int>());
                    allStates[visitState.Level][currP.Y].Add(currP.X, visitState.Steps);
                }

                if (cont)
                {
                    ns = GetNewPossibleStates(visitState);
                    foreach (var s in ns)
                    {
                        shouldVisit.Add(s);
                    }
                }
            }

            if(endState != null)
            {
                Console.WriteLine(endState.Steps);
            }
            Console.WriteLine("Completed!");
            Console.ReadLine();
        }
    }
}
