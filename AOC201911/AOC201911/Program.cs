using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC201911
{
    class Program
    {
        class Panel
        {
            public int X { get; set; } = 0;
            public int Y { get; set; } = 0;
            public int Color { get; set; } = 0;
        }

        class Robot
        {
            public int X { get; set; } = 0;
            public int Y { get; set; } = 0;
            public int dX { get; set; } = 0;
            public int dY { get; set; } = -1;
            public List<Panel> PaintedPanels { get; set; } = new List<Panel>() { new Panel { X = 0, Y = 0, Color = 1} };

            public void Move()
            {
                X += dX;
                Y += dY;
            }

            public void Turn(int d)
            {
                if(d == 0)
                {
                    var ndX = dY < 0 ? (dX - 1) % 2 : (dX + 1) % 2;
                    dY = (dX < 0) ? (dY + 1) % 2 : (dY - 1) % 2;
                    dX = ndX;
                }
                else
                {
                    var ndX = dY < 0 ? (dX + 1) % 2 : (dX - 1) % 2;
                    dY = (dX < 0) ? (dY - 1) % 2 : (dY + 1) % 2;
                    dX = ndX;
                }
            }

            public int CurrentColor()
            {
                var panel = PaintedPanels.SingleOrDefault(p => p.X == X && p.Y == Y);
                if(panel != null)
                {
                    return panel.Color;
                }
                return 0;
            }

            public void Paint(int color)
            {
                var panel = PaintedPanels.SingleOrDefault(p => p.X == X && p.Y == Y);
                if(panel != null)
                {
                    panel.Color = color;
                }
                else
                {
                    var np = new Panel { X = X, Y = Y, Color = color };
                    PaintedPanels.Add(np);
                }
            }

        }
        class Computer
        {
            public Computer(List<long> program)
            {
                this.program = program;
            }

            List<long> program;

            private int GetOpcode(int inst)
            {
                var s = inst.ToString();
                if (s.Length < 2)
                    return inst;
                return int.Parse(s.Substring(s.Length - 2));
            }

            private List<int> GetModes(int inst, int pNum)
            {
                List<int> ret = new List<int>(pNum);
                ret.AddRange(Enumerable.Repeat(0, pNum));
                int i = 0;
                foreach (var m in inst.ToString().Reverse().Skip(2))
                {
                    if (m == '1')
                    {
                        ret[i] = 1;
                    }
                    if (m == '2')
                    {
                        ret[i] = 2;
                    }
                    i++;
                }
                return ret;
            }
            int currPosition = 0;
            int relativeBase = 0;

            private long GetParameter(int paramMode, int position)
            {
                switch (paramMode)
                {
                    case 0:
                        return program[(int)program[position]];
                    case 1:
                        return program[position];
                    case 2:
                        return program[relativeBase + (int)program[position]];
                    default:
                        throw new Exception();
                }
            }

            private int GetIndex(int paramMode, int position)
            {
                switch (paramMode)
                {
                    case 0:
                        return (int)program[position];
                    case 2:
                        return relativeBase + (int)program[position];
                    default:
                        throw new Exception();
                }
            }

            public Tuple<long, bool> Compute(int stdinput)
            {
                int currOpCode = 0;
                long output = 0;
                while (currOpCode != 99)
                {
                    var inst = (int)program[currPosition];
                    currOpCode = GetOpcode(inst);
                    List<int> paramModes;
                    switch (currOpCode)
                    {
                        case 1:
                            paramModes = GetModes(inst, 3);
                            program[GetIndex(paramModes[2], currPosition + 3)] = GetParameter(paramModes[0], currPosition + 1) + GetParameter(paramModes[1], currPosition + 2);
                            currPosition += 4;
                            break;
                        case 2:
                            paramModes = GetModes(inst, 3);
                            program[GetIndex(paramModes[2], currPosition + 3)] = GetParameter(paramModes[0], currPosition + 1) * GetParameter(paramModes[1], currPosition + 2);
                            currPosition += 4;
                            break;
                        case 3:
                            paramModes = GetModes(inst, 1);
                            program[GetIndex(paramModes[0], currPosition + 1)] = stdinput;
                            currPosition += 2;
                            break;
                        case 4:
                            paramModes = GetModes(inst, 1);
                            output = GetParameter(paramModes[0], currPosition + 1); ;
                            currPosition += 2;
                            return new Tuple<long, bool>(output, false);
                        case 5:
                            paramModes = GetModes(inst, 2);
                            var v = GetParameter(paramModes[0], currPosition + 1);
                            if (v != 0)
                            {
                                currPosition = (int)GetParameter(paramModes[1], currPosition + 2);
                            }
                            else
                            {
                                currPosition += 3;
                            }
                            break;
                        case 6:
                            paramModes = GetModes(inst, 2);
                            var v2 = GetParameter(paramModes[0], currPosition + 1);
                            if (v2 == 0)
                            {
                                currPosition = (int)GetParameter(paramModes[1], currPosition + 2);
                            }
                            else
                            {
                                currPosition += 3;
                            }
                            break;
                        case 7:
                            paramModes = GetModes(inst, 3);
                            var v3 = GetParameter(paramModes[0], currPosition + 1);
                            var v4 = GetParameter(paramModes[1], currPosition + 2);
                            if (v3 < v4)
                            {
                                program[GetIndex(paramModes[2], currPosition + 3)] = 1;
                            }
                            else
                            {
                                program[GetIndex(paramModes[2], currPosition + 3)] = 0;
                            }
                            currPosition += 4;
                            break;
                        case 8:
                            paramModes = GetModes(inst, 3);
                            var v5 = GetParameter(paramModes[0], currPosition + 1);
                            var v6 = GetParameter(paramModes[1], currPosition + 2);
                            if (v5 == v6)
                            {
                                program[GetIndex(paramModes[2], currPosition + 3)] = 1;
                            }
                            else
                            {
                                program[GetIndex(paramModes[2], currPosition + 3)] = 0;
                            }
                            currPosition += 4;
                            break;
                        case 9:
                            paramModes = GetModes(inst, 1);
                            relativeBase += (int)GetParameter(paramModes[0], currPosition + 1);
                            currPosition += 2;
                            break;
                        case 99:
                            return new Tuple<long, bool>(output, true);
                        default:
                            throw new Exception();
                    }

                }
                throw new Exception();
            }
        }

        static void Main(string[] args)
        {
            Robot r = new Robot();
            List<long> program = new List<long> { 3, 8, 1005, 8, 352, 1106, 0, 11, 0, 0, 0, 104, 1, 104, 0, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 1, 8, 10, 4, 10, 102, 1, 8, 28, 1, 1003, 20, 10, 2, 106, 11, 10, 2, 1107, 1, 10, 1, 1001, 14, 10, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 0, 10, 4, 10, 1002, 8, 1, 67, 2, 1009, 7, 10, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 108, 0, 8, 10, 4, 10, 101, 0, 8, 92, 1, 105, 9, 10, 1006, 0, 89, 1, 108, 9, 10, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1002, 8, 1, 126, 1, 1101, 14, 10, 1, 1005, 3, 10, 1006, 0, 29, 1006, 0, 91, 3, 8, 102, -1, 8, 10, 101, 1, 10, 10, 4, 10, 108, 1, 8, 10, 4, 10, 1002, 8, 1, 161, 1, 1, 6, 10, 1006, 0, 65, 2, 106, 13, 10, 1006, 0, 36, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 102, 1, 8, 198, 1, 105, 15, 10, 1, 1004, 0, 10, 3, 8, 1002, 8, -1, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 0, 10, 4, 10, 101, 0, 8, 228, 2, 1006, 8, 10, 2, 1001, 16, 10, 3, 8, 102, -1, 8, 10, 1001, 10, 1, 10, 4, 10, 108, 0, 8, 10, 4, 10, 1001, 8, 0, 257, 1006, 0, 19, 2, 6, 10, 10, 2, 4, 13, 10, 2, 1002, 4, 10, 3, 8, 102, -1, 8, 10, 1001, 10, 1, 10, 4, 10, 1008, 8, 1, 10, 4, 10, 1002, 8, 1, 295, 3, 8, 1002, 8, -1, 10, 101, 1, 10, 10, 4, 10, 108, 0, 8, 10, 4, 10, 101, 0, 8, 316, 2, 101, 6, 10, 1006, 0, 84, 2, 1004, 13, 10, 1, 1109, 3, 10, 101, 1, 9, 9, 1007, 9, 1046, 10, 1005, 10, 15, 99, 109, 674, 104, 0, 104, 1, 21101, 387365315340, 0, 1, 21102, 369, 1, 0, 1105, 1, 473, 21101, 666685514536, 0, 1, 21102, 380, 1, 0, 1106, 0, 473, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 1, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 1, 21102, 1, 46266346536, 1, 21102, 427, 1, 0, 1105, 1, 473, 21101, 235152829659, 0, 1, 21101, 438, 0, 0, 1105, 1, 473, 3, 10, 104, 0, 104, 0, 3, 10, 104, 0, 104, 0, 21102, 838337188620, 1, 1, 21101, 461, 0, 0, 1105, 1, 473, 21102, 988753429268, 1, 1, 21102, 1, 472, 0, 1106, 0, 473, 99, 109, 2, 22101, 0, -1, 1, 21101, 40, 0, 2, 21101, 504, 0, 3, 21102, 494, 1, 0, 1106, 0, 537, 109, -2, 2105, 1, 0, 0, 1, 0, 0, 1, 109, 2, 3, 10, 204, -1, 1001, 499, 500, 515, 4, 0, 1001, 499, 1, 499, 108, 4, 499, 10, 1006, 10, 531, 1101, 0, 0, 499, 109, -2, 2106, 0, 0, 0, 109, 4, 2101, 0, -1, 536, 1207, -3, 0, 10, 1006, 10, 554, 21102, 1, 0, -3, 21202, -3, 1, 1, 21201, -2, 0, 2, 21102, 1, 1, 3, 21101, 573, 0, 0, 1105, 1, 578, 109, -4, 2105, 1, 0, 109, 5, 1207, -3, 1, 10, 1006, 10, 601, 2207, -4, -2, 10, 1006, 10, 601, 21201, -4, 0, -4, 1105, 1, 669, 22101, 0, -4, 1, 21201, -3, -1, 2, 21202, -2, 2, 3, 21101, 620, 0, 0, 1106, 0, 578, 22102, 1, 1, -4, 21101, 0, 1, -1, 2207, -4, -2, 10, 1006, 10, 639, 21101, 0, 0, -1, 22202, -2, -1, -2, 2107, 0, -3, 10, 1006, 10, 661, 22101, 0, -1, 1, 21102, 661, 1, 0, 106, 0, 536, 21202, -2, -1, -2, 22201, -4, -2, -4, 109, -5, 2106, 0, 0 };
            program.AddRange(Enumerable.Repeat((long)0, program.Count * 100));

            Computer a = new Computer(program);
            bool done = false;
            while (!done)
            {
                var paintColor = a.Compute(r.CurrentColor());
                var turn = a.Compute(r.CurrentColor());
                if(paintColor.Item2 || turn.Item2)
                {
                    break;
                }

                r.Paint((int)paintColor.Item1);
                r.Turn((int)turn.Item1);
                r.Move();
            }

            var minx = r.PaintedPanels.Min(p => p.X);
            var miny = r.PaintedPanels.Min(p => p.Y);
            var maxx = r.PaintedPanels.Max(p => p.X);
            var maxy = r.PaintedPanels.Max(p => p.Y);

                for (int y = miny; y <= maxy; y++)
                {
            for (int x = minx; x <= maxx; x++)
                {
                    var panel = r.PaintedPanels.SingleOrDefault(p => p.X == x && p.Y == y);
                    if (panel != null && panel.Color == 1)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("Terminated!");
            Console.ReadLine();
        }
    }
}
