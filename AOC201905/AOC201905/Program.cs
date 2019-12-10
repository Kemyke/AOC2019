using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC201905
{
    class Program
    {
        static int GetOpcode(int inst)
        {
            var s = inst.ToString();
            if (s.Length < 2)
                return inst;
            return int.Parse(s.Substring(s.Length - 2));
        }

        static List<int> GetModes(int inst, int pNum)
        {
            List<int> ret = new List<int>(pNum);
            ret.AddRange(Enumerable.Repeat(0, pNum));
            int i = 0;
            foreach(var m in inst.ToString().Reverse().Skip(2))
            {
                if(m == '1')
                {
                    ret[i] = 1;
                }
                i++;
            }
            return ret;
        }

        static int Compute(List<int> program)
        {
            int currPosition = 0;
            int currOpCode = 0;
            while (currOpCode != 99)
            {
                var inst = program[currPosition];
                currOpCode = GetOpcode(inst);
                List<int> paramModes;
                switch (currOpCode)
                {
                    case 1:
                        paramModes = GetModes(inst, 3);
                        program[program[currPosition + 3]] = (paramModes[0] == 0 ? program[program[currPosition + 1]] : program[currPosition + 1]) + (paramModes[1] == 0 ? program[program[currPosition + 2]] : program[currPosition + 2]);
                        currPosition += 4;
                        break;
                    case 2:
                        paramModes = GetModes(inst, 3);
                        program[program[currPosition + 3]] = (paramModes[0] == 0 ? program[program[currPosition + 1]] : program[currPosition + 1]) * (paramModes[1] == 0 ? program[program[currPosition + 2]] : program[currPosition + 2]);
                        currPosition += 4;
                        break;
                    case 3:
                        Console.WriteLine("Input: ");
                        var ip = Console.ReadLine();
                        var input = int.Parse(ip);
                        program[program[currPosition + 1]] = input;
                        currPosition += 2;
                        break;
                    case 4:
                        Console.WriteLine(program[program[currPosition + 1]]);
                        currPosition += 2;
                        break;
                    case 5:
                        paramModes = GetModes(inst, 2);
                        var v = (paramModes[0] == 0 ? program[program[currPosition + 1]] : program[currPosition + 1]);
                        if (v != 0)
                        {
                            currPosition = (paramModes[1] == 0 ? program[program[currPosition + 2]] : program[currPosition + 2]);
                        }
                        else
                        {
                            currPosition += 3;
                        }
                        break;
                    case 6:
                        paramModes = GetModes(inst, 2);
                        var v2 = (paramModes[0] == 0 ? program[program[currPosition + 1]] : program[currPosition + 1]);
                        if (v2 == 0)
                        {
                            currPosition = (paramModes[1] == 0 ? program[program[currPosition + 2]] : program[currPosition + 2]);
                        }
                        else
                        {
                            currPosition += 3;
                        }
                        break;
                    case 7:
                        paramModes = GetModes(inst, 3);
                        var v3 = (paramModes[0] == 0 ? program[program[currPosition + 1]] : program[currPosition + 1]);
                        var v4 = (paramModes[1] == 0 ? program[program[currPosition + 2]] : program[currPosition + 2]);
                        if(v3 < v4)
                        {
                            program[program[currPosition + 3]] = 1;
                        }
                        else
                        {
                            program[program[currPosition + 3]] = 0;
                        }
                        currPosition += 4;
                        break;
                    case 8:
                        paramModes = GetModes(inst, 3);
                        var v5 = (paramModes[0] == 0 ? program[program[currPosition + 1]] : program[currPosition + 1]);
                        var v6 = (paramModes[1] == 0 ? program[program[currPosition + 2]] : program[currPosition + 2]);
                        if (v5 == v6)
                        {
                            program[program[currPosition + 3]] = 1;
                        }
                        else
                        {
                            program[program[currPosition + 3]] = 0;
                        }
                        currPosition += 4;
                        break;
                    case 99:
                        break;
                    default:
                        throw new Exception();
                }
                
            }
            return program[0];
        }

        static void Main(string[] args)
        {
            List<int> program = new List<int> { 3, 225, 1, 225, 6, 6, 1100, 1, 238, 225, 104, 0, 1102, 40, 93, 224, 1001, 224, -3720, 224, 4, 224, 102, 8, 223, 223, 101, 3, 224, 224, 1, 224, 223, 223, 1101, 56, 23, 225, 1102, 64, 78, 225, 1102, 14, 11, 225, 1101, 84, 27, 225, 1101, 7, 82, 224, 1001, 224, -89, 224, 4, 224, 1002, 223, 8, 223, 1001, 224, 1, 224, 1, 224, 223, 223, 1, 35, 47, 224, 1001, 224, -140, 224, 4, 224, 1002, 223, 8, 223, 101, 5, 224, 224, 1, 224, 223, 223, 1101, 75, 90, 225, 101, 9, 122, 224, 101, -72, 224, 224, 4, 224, 1002, 223, 8, 223, 101, 6, 224, 224, 1, 224, 223, 223, 1102, 36, 63, 225, 1002, 192, 29, 224, 1001, 224, -1218, 224, 4, 224, 1002, 223, 8, 223, 1001, 224, 7, 224, 1, 223, 224, 223, 102, 31, 218, 224, 101, -2046, 224, 224, 4, 224, 102, 8, 223, 223, 101, 4, 224, 224, 1, 224, 223, 223, 1001, 43, 38, 224, 101, -52, 224, 224, 4, 224, 1002, 223, 8, 223, 101, 5, 224, 224, 1, 223, 224, 223, 1102, 33, 42, 225, 2, 95, 40, 224, 101, -5850, 224, 224, 4, 224, 1002, 223, 8, 223, 1001, 224, 7, 224, 1, 224, 223, 223, 1102, 37, 66, 225, 4, 223, 99, 0, 0, 0, 677, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1105, 0, 99999, 1105, 227, 247, 1105, 1, 99999, 1005, 227, 99999, 1005, 0, 256, 1105, 1, 99999, 1106, 227, 99999, 1106, 0, 265, 1105, 1, 99999, 1006, 0, 99999, 1006, 227, 274, 1105, 1, 99999, 1105, 1, 280, 1105, 1, 99999, 1, 225, 225, 225, 1101, 294, 0, 0, 105, 1, 0, 1105, 1, 99999, 1106, 0, 300, 1105, 1, 99999, 1, 225, 225, 225, 1101, 314, 0, 0, 106, 0, 0, 1105, 1, 99999, 1007, 226, 677, 224, 1002, 223, 2, 223, 1005, 224, 329, 1001, 223, 1, 223, 1007, 226, 226, 224, 1002, 223, 2, 223, 1006, 224, 344, 101, 1, 223, 223, 1107, 677, 226, 224, 102, 2, 223, 223, 1006, 224, 359, 1001, 223, 1, 223, 108, 677, 677, 224, 1002, 223, 2, 223, 1006, 224, 374, 1001, 223, 1, 223, 107, 677, 677, 224, 1002, 223, 2, 223, 1005, 224, 389, 101, 1, 223, 223, 8, 677, 677, 224, 1002, 223, 2, 223, 1005, 224, 404, 1001, 223, 1, 223, 108, 226, 226, 224, 1002, 223, 2, 223, 1005, 224, 419, 101, 1, 223, 223, 1008, 677, 677, 224, 1002, 223, 2, 223, 1005, 224, 434, 101, 1, 223, 223, 1008, 226, 226, 224, 1002, 223, 2, 223, 1005, 224, 449, 101, 1, 223, 223, 7, 677, 226, 224, 1002, 223, 2, 223, 1006, 224, 464, 1001, 223, 1, 223, 7, 226, 226, 224, 1002, 223, 2, 223, 1005, 224, 479, 1001, 223, 1, 223, 1007, 677, 677, 224, 102, 2, 223, 223, 1005, 224, 494, 101, 1, 223, 223, 1108, 677, 226, 224, 102, 2, 223, 223, 1006, 224, 509, 1001, 223, 1, 223, 8, 677, 226, 224, 102, 2, 223, 223, 1005, 224, 524, 1001, 223, 1, 223, 1107, 226, 226, 224, 102, 2, 223, 223, 1006, 224, 539, 1001, 223, 1, 223, 1008, 226, 677, 224, 1002, 223, 2, 223, 1006, 224, 554, 1001, 223, 1, 223, 1107, 226, 677, 224, 1002, 223, 2, 223, 1006, 224, 569, 1001, 223, 1, 223, 1108, 677, 677, 224, 102, 2, 223, 223, 1005, 224, 584, 101, 1, 223, 223, 7, 226, 677, 224, 102, 2, 223, 223, 1006, 224, 599, 1001, 223, 1, 223, 1108, 226, 677, 224, 102, 2, 223, 223, 1006, 224, 614, 101, 1, 223, 223, 107, 226, 677, 224, 1002, 223, 2, 223, 1005, 224, 629, 101, 1, 223, 223, 108, 226, 677, 224, 1002, 223, 2, 223, 1005, 224, 644, 101, 1, 223, 223, 8, 226, 677, 224, 1002, 223, 2, 223, 1005, 224, 659, 1001, 223, 1, 223, 107, 226, 226, 224, 1002, 223, 2, 223, 1006, 224, 674, 101, 1, 223, 223, 4, 223, 99, 226 };
            Compute(program);

            Console.WriteLine("Terminated!");
            Console.ReadLine();
        }
    }
}
