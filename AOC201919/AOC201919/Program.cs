using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC201919
{
    class Program
    {
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

            int stdNum = 0;
            public Tuple<long, bool> Compute(int x, int y)
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
                            program[GetIndex(paramModes[0], currPosition + 1)] = stdNum++ == 0 ? x : y;
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
            List<long> program = new List<long> { 109, 424, 203, 1, 21101, 0, 11, 0, 1105, 1, 282, 21101, 18, 0, 0, 1105, 1, 259, 2101, 0, 1, 221, 203, 1, 21102, 1, 31, 0, 1105, 1, 282, 21101, 0, 38, 0, 1106, 0, 259, 21002, 23, 1, 2, 22101, 0, 1, 3, 21101, 0, 1, 1, 21102, 1, 57, 0, 1106, 0, 303, 2102, 1, 1, 222, 21001, 221, 0, 3, 20102, 1, 221, 2, 21101, 0, 259, 1, 21101, 80, 0, 0, 1106, 0, 225, 21101, 0, 23, 2, 21102, 91, 1, 0, 1106, 0, 303, 1201, 1, 0, 223, 20101, 0, 222, 4, 21101, 0, 259, 3, 21102, 1, 225, 2, 21102, 1, 225, 1, 21102, 1, 118, 0, 1105, 1, 225, 20102, 1, 222, 3, 21101, 0, 87, 2, 21101, 133, 0, 0, 1106, 0, 303, 21202, 1, -1, 1, 22001, 223, 1, 1, 21101, 0, 148, 0, 1105, 1, 259, 2101, 0, 1, 223, 20102, 1, 221, 4, 21002, 222, 1, 3, 21101, 0, 9, 2, 1001, 132, -2, 224, 1002, 224, 2, 224, 1001, 224, 3, 224, 1002, 132, -1, 132, 1, 224, 132, 224, 21001, 224, 1, 1, 21102, 1, 195, 0, 106, 0, 109, 20207, 1, 223, 2, 21001, 23, 0, 1, 21102, 1, -1, 3, 21101, 0, 214, 0, 1106, 0, 303, 22101, 1, 1, 1, 204, 1, 99, 0, 0, 0, 0, 109, 5, 2102, 1, -4, 249, 21201, -3, 0, 1, 22101, 0, -2, 2, 21202, -1, 1, 3, 21102, 250, 1, 0, 1106, 0, 225, 21202, 1, 1, -4, 109, -5, 2106, 0, 0, 109, 3, 22107, 0, -2, -1, 21202, -1, 2, -1, 21201, -1, -1, -1, 22202, -1, -2, -2, 109, -3, 2105, 1, 0, 109, 3, 21207, -2, 0, -1, 1206, -1, 294, 104, 0, 99, 21202, -2, 1, -2, 109, -3, 2105, 1, 0, 109, 5, 22207, -3, -4, -1, 1206, -1, 346, 22201, -4, -3, -4, 21202, -3, -1, -1, 22201, -4, -1, 2, 21202, 2, -1, -1, 22201, -4, -1, 1, 22102, 1, -2, 3, 21102, 1, 343, 0, 1106, 0, 303, 1106, 0, 415, 22207, -2, -3, -1, 1206, -1, 387, 22201, -3, -2, -3, 21202, -2, -1, -1, 22201, -3, -1, 3, 21202, 3, -1, -1, 22201, -3, -1, 2, 21201, -4, 0, 1, 21102, 384, 1, 0, 1105, 1, 303, 1106, 0, 415, 21202, -4, -1, -4, 22201, -4, -3, -4, 22202, -3, -2, -2, 22202, -2, -4, -4, 22202, -3, -2, -3, 21202, -4, -1, -2, 22201, -3, -2, 1, 21202, 1, 1, -4, 109, -5, 2106, 0, 0 };
            program.AddRange(Enumerable.Repeat((long)0, program.Count * 100));            
            long pperline = 0;
            List<string> output = new List<string>();
            int rx = 0;
            int ry = 0;
            int lastBeamStart = 0;
            for (int y = 0; y < 2550; y++)
            {                
                int fx = 0;
                int fy = 0;
                bool firstInline = true;
                //StringBuilder sb = new StringBuilder(250);
                for (int x = lastBeamStart; x < 5250; x++)
                {
                    Computer a = new Computer(program.ToList());
                    var p = a.Compute(x, y).Item1;
                    //pulled += p;
                    pperline += p;

                    if (p == 0)
                    {
                        //sb.Append(".");
                        if(!firstInline)
                        {
                            break;
                        }
                    }
                    else if(p == 1)
                    {
                        if (firstInline)
                        {
                            lastBeamStart = Math.Max(x - 10, 0);
                            firstInline = false;
                            fx = x;
                            fy = y;
                        }
                        //sb.Append("#");
                    }
                }
                //File.AppendAllText("output.txt", sb.ToString());
                //File.AppendAllText("output.txt", Environment.NewLine);
                if(pperline >= 100)
                {
                    Computer a = new Computer(program.ToList());
                    var p = a.Compute(fx, fy - 99).Item1;
                    if(p == 1)
                    {
                        Computer a2 = new Computer(program.ToList());
                        var p2 = a2.Compute(fx + 99, fy - 99).Item1;
                        if (p2 == 1)
                        {
                            rx = fx;
                            ry = fy - 99;
                            break;
                        }
                    }
                }
                pperline = 0;
            }
            Console.WriteLine(rx * 10000 + ry);
            Console.ReadLine();
        }
    }
}
