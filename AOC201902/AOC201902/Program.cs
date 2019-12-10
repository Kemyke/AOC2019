using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC201902
{
    class Program
    {
        static int Compute(List<int> program)
        {
            int currPosition = 0;
            int currOpCode = 0;
            while (currOpCode != 99)
            {
                currOpCode = program[currPosition];
                switch (currOpCode)
                {
                    case 1:
                        program[program[currPosition + 3]] = program[program[currPosition + 1]] + program[program[currPosition + 2]];
                        break;
                    case 2:
                        program[program[currPosition + 3]] = program[program[currPosition + 1]] * program[program[currPosition + 2]];
                        break;
                    case 99:
                        break;
                    default:
                        throw new Exception();
                }
                currPosition += 4;
            }
            return program[0];
        }

        static void Main(string[] args)
        {
            List<int> program = new List<int> { 1, 0, 0, 3, 1, 1, 2, 3, 1, 3, 4, 3, 1, 5, 0, 3, 2, 10, 1, 19, 1, 19, 9, 23, 1, 23, 13, 27, 1, 10, 27, 31, 2, 31, 13, 35, 1, 10, 35, 39, 2, 9, 39, 43, 2, 43, 9, 47, 1, 6, 47, 51, 1, 10, 51, 55, 2, 55, 13, 59, 1, 59, 10, 63, 2, 63, 13, 67, 2, 67, 9, 71, 1, 6, 71, 75, 2, 75, 9, 79, 1, 79, 5, 83, 2, 83, 13, 87, 1, 9, 87, 91, 1, 13, 91, 95, 1, 2, 95, 99, 1, 99, 6, 0, 99, 2, 14, 0, 0 };
            int n = 0, v = 0;
            for(int noun = 0; noun < 100; noun++)
                for(int verb = 0; verb < 100; verb++)
                {
                    var np = program.ToList();
                    np[1] = noun;
                    np[2] = verb;
                    var result = Compute(np);
                    if(result == 19690720)
                    {
                        n = noun;
                        v = verb;
                        break;
                    }
                }

            Console.WriteLine(100 * n + v);
            Console.ReadLine();
        }
    }
}
