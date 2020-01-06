using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC201916
{
    class Program
    {
        static List<int> GetOnesForItem(int item, int length)
        {
            List<int> ret = new List<int>();
            int m = (item + 1) * 4;
            for (int i = 0; i <= length; i++)
            {
                if (i % m >= item + 1 && i % m < 2 * (item + 1))
                {
                    ret.Add(i - 1);
                }
            }
            return ret;
        }

        static List<int> GetMinusOnesForItem(int item, int length)
        {
            List<int> ret = new List<int>();
            int m = (item + 1) * 4;
            for (int i = 0; i <= length; i++)
            {
                if (i % m >= 3 * (item + 1) && i % m < 4 * (item + 1))
                {
                    ret.Add(i - 1);
                }
            }
            return ret;
        }

        static int GetItem(Dictionary<int, int> items, List<int> ones, List<int> minusOnes)
        {
            long ret = 0;
            foreach (var one in ones)
            {
                ret += items[one];
            }
            foreach (var minusone in minusOnes)
            {
                ret -= items[minusone];
            }
            return int.Parse(Math.Abs(ret).ToString().Last().ToString());
        }

        static Dictionary<int, int> GetSignal(string input)
        {
            Dictionary<int, int> ret = new Dictionary<int, int>();
            int i = 0;
            foreach (var item in input)
            {
                ret.Add(i++, int.Parse(item.ToString()));
            }
            return ret;
        }

        static void Main(string[] args)
        {
            int phaseNum = 100;
            string input = "59781998462438675006185496762485925436970503472751174459080326994618036736403094024111488348676644802419244196591075975610084280308059415695059918368911890852851760032000543205724091764633390765212561307082338287866715489545069566330303873914343745198297391838950197434577938472242535458546669655890258618400619467693925185601880453581947475741536786956920286681271937042394272034410161080365044440682830248774547018223347551308590698989219880430394446893636437913072055636558787182933357009123440661477321673973877875974028654688639313502382365854245311641198762520478010015968789202270746880399268251176490599427469385384364675153461448007234636949";
            StringBuilder sb = new StringBuilder(input.Length * 10000);
            for (int i = 0; i < 10000; i++)
                sb.Append(input);
            var realInput = sb.ToString();

            string partInput = realInput.Substring(5978199);
            for (int j = 0; j < 100; j++)
            {

                StringBuilder r = new StringBuilder(partInput.Length);
                int sum = 0;
                for (int i = 0; i < partInput.Length; i++)
                {
                    var idx = partInput.Length - 1 - i;
                    sum += int.Parse(partInput[idx].ToString());
                    r.Append(sum % 10);
                }
                partInput = new string(r.ToString().Reverse().ToArray());
            }
            //Pt2
            Console.WriteLine(partInput.Substring(0, 8));


            Dictionary<int, int> signal = GetSignal(input);
            Dictionary<int, List<int>> onesPerItem = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> minusOnesPerItem = new Dictionary<int, List<int>>();
            List<string> rets = new List<string>();
            for (int i = 0; i < signal.Count; i++)
            {
                onesPerItem.Add(i, GetOnesForItem(i, signal.Count));
            }
            for (int i = 0; i < signal.Count; i++)
            {
                minusOnesPerItem.Add(i, GetMinusOnesForItem(i, signal.Count));
            }

            for (int i = 0; i < phaseNum; i++)
            {
                Dictionary<int, int> newSignal = new Dictionary<int, int>();
                foreach (var kvp in signal)
                {
                    newSignal.Add(kvp.Key, GetItem(signal, onesPerItem[kvp.Key], minusOnesPerItem[kvp.Key]));
                }

                signal = newSignal;
                rets.Add(string.Join("", signal.Values));
            }
            //Pt1
            Console.WriteLine(string.Join("", signal.Values.Take(8)));
            Console.ReadLine();
        }
    }
}