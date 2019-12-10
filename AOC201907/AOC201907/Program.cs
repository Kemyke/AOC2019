using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC201907
{
    class Program
    {
        class Amp
        {
            public Amp(List<int> program, int phase)
            {
                this.program = program;
                this.phase = phase;
            }

            List<int> program;
            int phase;

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
                    i++;
                }
                return ret;
            }
            bool init = false;
            int currPosition = 0;

            public Tuple<int, bool> Compute(int stdinput)
            {
                int currOpCode = 0;
                int output = 0;
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
                            program[program[currPosition + 1]] = !init ? phase : stdinput;
                            init = true;
                            currPosition += 2;
                            break;
                        case 4:
                            output = program[program[currPosition + 1]];
                            currPosition += 2;                           
                            return new Tuple<int, bool>(output, false);
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
                            if (v3 < v4)
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
                            return new Tuple<int, bool>(output, true);
                        default:
                            throw new Exception();
                    }

                }
                throw new Exception();
            }
        }

        static void Main(string[] args)
        {
            List<int> program = new List<int> { 3, 8, 1001, 8, 10, 8, 105, 1, 0, 0, 21, 42, 67, 84, 109, 126, 207, 288, 369, 450, 99999, 3, 9, 102, 4, 9, 9, 1001, 9, 4, 9, 102, 2, 9, 9, 101, 2, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 5, 9, 1002, 9, 5, 9, 1001, 9, 5, 9, 1002, 9, 5, 9, 101, 5, 9, 9, 4, 9, 99, 3, 9, 101, 5, 9, 9, 1002, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 102, 4, 9, 9, 101, 2, 9, 9, 102, 4, 9, 9, 1001, 9, 2, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 101, 5, 9, 9, 1002, 9, 2, 9, 4, 9, 99, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 99, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 99, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 101, 2, 9, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 1, 9, 4, 9, 3, 9, 1002, 9, 2, 9, 4, 9, 3, 9, 101, 1, 9, 9, 4, 9, 3, 9, 102, 2, 9, 9, 4, 9, 3, 9, 1001, 9, 2, 9, 4, 9, 99 };
            int max = 0;
            List<int> settings = null;

            for (int i = 5; i <= 9; i++)
            {
                for (int j = 5; j <= 9; j++)
                {
                    for (int k = 5; k <= 9; k++)
                    {
                        for (int l = 5; l <= 9; l++)
                        {
                            for (int m = 5; m <= 9; m++)
                            {
                                var csettings = new List<int> { i, j, k, l, m };
                                if (csettings.Distinct().Count() == 5)
                                {
                                    Amp A = new Amp(program.ToList(), i);
                                    Amp B = new Amp(program.ToList(), j);
                                    Amp C = new Amp(program.ToList(), k);
                                    Amp D = new Amp(program.ToList(), l);
                                    Amp E = new Amp(program.ToList(), m);

                                    bool halt = false;
                                    int output = 0;
                                    while (!halt)
                                    {
                                        var res = E.Compute(D.Compute(C.Compute(B.Compute(A.Compute(output).Item1).Item1).Item1).Item1);
                                        halt = res.Item2;
                                        if (!halt)
                                            output = res.Item1;
                                    }
                                    if (output > max)
                                    {
                                        max = output;
                                        settings = new List<int> { i, j, k, l, m };
                                    }

                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine(max);
            Console.WriteLine(string.Join(",", settings));

            Console.WriteLine("Terminated!");
            Console.ReadLine();
        }
    }
}
