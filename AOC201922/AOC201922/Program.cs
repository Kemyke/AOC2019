using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AOC201922
{
    class Program
    {
        static List<int> deck = null;

        static void NewLines2(List<string> ret, BigInteger deckSize)
        {
            int fci = -1;
            while (true)
            {
                var fc = ret.Skip(fci + 1).FirstOrDefault(l => l.StartsWith("deal with increment"));
                fci = ret.IndexOf(fc);
                if (fci > -1 && fci + 1 < ret.Count && ret[fci + 1].StartsWith("deal with increment"))
                {
                    var d1l = ret[fci].Split(" ").Last();
                    BigInteger d1 = BigInteger.Parse(d1l);

                    var d2l = ret[fci + 1].Split(" ").Last();
                    BigInteger d2 = BigInteger.Parse(d2l);

                    ret.RemoveAt(fci);
                    ret.RemoveAt(fci);
                    ret.Insert(fci, "deal with increment " + ((d1 * d2) % deckSize));
                }
                else
                {
                    if (fci == -1 || fci + 1 > ret.Count)
                    {
                        break;
                    }
                }
            }
        }

        static void NewLines3(List<string> ret, BigInteger deckSize)
        {
            int fci = 0;
            while (true)
            {
                var fc = ret.Skip(fci + 1).FirstOrDefault(l => l.StartsWith("cut"));
                fci = ret.IndexOf(fc);
                if (fci + 2 < ret.Count && ret[fci + 1].StartsWith("deal with increment") && ret[fci + 2].StartsWith("cut"))
                {
                    int idx = fci;
                    var c = ret[idx].Split(" ").Last();
                    BigInteger cut = BigInteger.Parse(c);

                    var d = ret[idx + 1].Split(" ").Last();
                    BigInteger dealwithincrement = BigInteger.Parse(d);

                    var nc = ret[idx + 2].Split(" ").Last();
                    BigInteger nextcut = BigInteger.Parse(nc);

                    ret.RemoveAt(idx);
                    ret.RemoveAt(idx);
                    ret.RemoveAt(idx);

                    ret.Insert(idx, "deal with increment " + dealwithincrement);
                    ret.Insert(idx + 1, "cut " + ((cut * dealwithincrement) % deckSize + nextcut) % deckSize);
                }
                else
                {
                    if (fci + 2 > ret.Count)
                    {
                        break;
                    }
                }
            }
        }

        static List<string> NewLines(List<string> lines, BigInteger deckSize)
        {
            List<string> ret = lines.ToList();

            var retL = int.MaxValue;
            while (retL > ret.Count)
            {
                retL = ret.Count;
                NewLines3(ret, deckSize);
            }

            retL = int.MaxValue;
            while (retL > ret.Count)
            {
                retL = ret.Count;
                NewLines2(ret, deckSize);
            }

            retL = int.MaxValue;
            while (retL > ret.Count)
            {
                retL = ret.Count;
                NewLines3(ret, deckSize);
            }

            retL = int.MaxValue;
            while (retL > ret.Count)
            {
                retL = ret.Count;
                NewLines2(ret, deckSize);
            }

            var x = string.Join(Environment.NewLine, ret);
            return ret;
        }
        
        static BigInteger seriessum(BigInteger a, BigInteger n, BigInteger m)
        {
            BigInteger ret;
            if (n == 1)
            {
                return (1 + a) % m;
            }
            if (n % 2 == 0)
            {
                return (BigInteger.ModPow(a, n, m) + seriessum(a, n - 1, m)) % m;
            }

            BigInteger nf = (n - 1) / 2;
            return ret = ((1 + a) * seriessum((a * a) % m, nf, m)) % m;            
        }

        static void Main(string[] args)
        {                            
            BigInteger deckSize = 119315717514047;
            BigInteger shuffleNum = 101741582076661;

            var lines = File.ReadAllLines("input.txt").ToList();
            lines = NewLines(lines, deckSize);

            BigInteger increment = BigInteger.Parse(lines[0].Split(" ").Last());
            increment = (increment + deckSize) % deckSize;
            BigInteger cut = BigInteger.Parse(lines[1].Split(" ").Last());
            cut = (cut + deckSize) % deckSize;

            var mi = (BigInteger.ModPow(increment, shuffleNum, deckSize) + deckSize) % deckSize;
            var mc4 = seriessum(increment, shuffleNum - 1, deckSize);
            mc4 = (mc4 + deckSize) % deckSize;
            mc4 = (mc4 * cut) % deckSize;

            lines.Clear();
            lines.Add("deal with increment " + mi);
            lines.Add("cut " + mc4);

            var testcut = mc4 + 2020;

            Console.ReadLine();
        } 
    }
}
