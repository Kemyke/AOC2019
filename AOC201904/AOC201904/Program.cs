using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC201904
{
    class Program
    {
        static bool DoubleDigit(string number)
        {
            for (int i = 0; i < number.Length - 1; i++)
            {
               if(number.Count(c=>c == number[i]) == 2)
                {
                    return true;
                }
            }
            return false;
        }

        static bool Decreasing(string number)
        {
            for (int i = 0; i < number.Length - 1; i++)
            {
                if (int.Parse(number[i].ToString()) > int.Parse(number[i + 1].ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            int count = 0;

            for(int i = 264793; i <= 803935; i++)
            {
                var n = i.ToString();
                if(DoubleDigit(n) && !Decreasing(n))
                {
                    count++;
                }
            }

            Console.WriteLine(count);
            Console.ReadLine();
        }
    }
}
