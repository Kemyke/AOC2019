﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace AOC201908
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "222222222022202221221222200020120222202020222120222201212202122222222212222222222222222202222222222222212222222222222212222222222222202222222222212222222222222122202222222222211121122222202020222220222200222202222222222222222222222222222202222222222222212222222222222212222222222222212222222222212222222222222022222222221222201020022222212020222121222222202222022222222202222022222222222222222222222222202222222222222212222222222222202222122222202222222222222122212220220222212021120222202222222220222210222202222222222202222222222222222202222222222222202222222222222222222222222222212222120222212222222222222122202222222222222121021222212121222022222211202222122222222222222122222222222202222222222222202222222222222202222222222222222222221222222222222222222222202220222222210121222222222122222221222220212212022222222212222122222222222202222222222222202222222222222202222222222222202222020222222222202222222022212222221222210221021222202222222021222201202202122222222202222122222222222212222222222222222222222222222222222222222222202222121222202222222222222122212221222222211220221222212220222020222221222212022222222222222222222222222222222222222222222222222222222202222222222222222222222222222222202222222222202222221222210121020222202122222121222212202222122222222222222222222222222202222222222222202222222222222222222222222222202222020220202222202222222022222221222222200022222222222022222121222201222222022222222212222122222222222212222222222222212222222222222222222222222222202222222221202222212222222122202220222222200121121222222120222122222221212212122222222212222122222222222202222222222222212222222222222222222222222222202222022220202222222222222022202220221222222121221222222020222020222200202202222222222202222122222222222202222222222222202222222222222202222222222222202222220222212222222222222222222222221222221021222222222021222021222222212202022222222222222122222222222202222222222222212222222222222212222222222222212222122220212222202222222022222222222222212220022222202021222222222200222212022222222202222122222222222202222222222222222222222222222212222222222222202222220222202222202222222122212221222222221022121222222121222122222222212222222222222222222222222222222222222222222222222222222222222222222222222222222202020221222222202222222022222222220222211220222222202121222120222200210212222222222222222222202222222212222222222222212222222222222212222222222222222212220222212222222222222222222220220222010122221222202222222122222202221202122222222222222122202222222212222222222220222222222222222222222222222222202202022222212222202222222122222220202212002120021222212222222122222222201202222222222202222222212222222222222222222220212222222222222202222222222222212222002220202222212222222122222222201202122121122222212020222020222210211222022222222202222222212222222222222222222220202222222222222212222222222222202212121221222222202222222122222220221202010022022222222020022122222200222222122222222212222022222222222202222222222222212222222022222202222222222222212222222221222222202222222022222222200222212022120222222221222220222210201202102222222202222222212222222212222222222222202222222022222222222222222222202212102220212222212222222122212221221212020220022222202221222021222210211222122222222202222122222222222202222222222222212222222122222202222222222222222212010221222222222222222022222220202212021121222222222221222122222221201222102222222212222222212222222222222222222221202222222122222222222222222222222202120222222222222222222222222221200212121021222222222212122120222201200212112222222222222122202222222222222222222220222222222122222212222222222222220222010220202222202222222022222222201222111221022222202102022221222201211222102222222202222122222222222212222222222220222222222222222222222222222222210212212220222222212222222022202222201202022221120222212102222221222201212202112222222212222022202222222212222222222222202222222022222212222222222222221212212221222222212222222022222222211222201120021222222011122021222210220212212222222222222122222222222202222222222220222222222122222212222222222222200212120220202222212222222122212220211222111121021222222010022220222210201222122222222222222222202222222222222222222221202222122222222202222222222222221222210222212222202222222022202222220202220221220222212000022121222202201222212222222202222122202222222222222222222220222222222222222202222222222222202222101222221222202222222022222221201222110021120222222202022220222222210202012222222212222122212222222212222222222222202220122122222212222222222222211212120222022222212222222222212221202212012021021222212201122222222202211222122222222212222022202222222212222222222221202222002022222202122222222222222222102220000222222222222222212220210222010220121222202011022221222222202222222222222222222222202222222212222222222220222221012122222222102222222222201212122221102222202222222122202221221222200022220222202221022120222210210212002202222222222122222222222212222222222222222221102022222222202222222222221222200221020222212222222222222222201202201022020222202021122221222202202222122222222212222022212222222212222222202221212222112122222212002222222222220202221220201222202222222122202222222212102120022222222100022122222200210222112212202222222122202222222222222222212221212220012022222202112222222222212202220220202222202222222222212221200222110122121222212022122221222210202222002202212202222122212222222222222222211221212220122222222222222222222222220202012221012222202222222022222221221222000021222222222012122020222211222222212212202202222122212222222212222222220220212221222122222212012222222222202202010221121222222222222022202220221222021211222222222210222220202220221222112212202212222022222222222222222222221222212220222022222202202222222222221222002221202222212202222222222221221202200202120222222100222121200220220212022222202222222022212222222222222222222020222220222222222202202222222222211222211222202222222202222122202220211222221112221222222022012022222220220202102202212202222022222222222202222222202220222221222022222202122222222222222202100222020222202222222222202221200222101002220222212211012020220221201202222222222212222022222222222222222222201121202220202222222202212222222222222212002220012222212212222022202221200212210020021222222100202221220221210202022212202202222222222202222222222222221120212221202222222202012222222222212212111220200222202212222122212221202222221120020222212101222120220201222212212212212202222222212202222222222222210122222222022122222202112222222222200202111221010222202212222122222222210202122010220222202120212222212202202222102212202222222122212212222200122222210222222222122222222222122222222222202222212221022222222202222222212222211212020110221222222221222221202211201212222202202202222122222202222222222222201221202221022122222202002222222222200212011220101222202222222122222222202212011012120222212220102021211211211002222222212212222022212212022221122222200220222222022022222212102222222222201202110220001222212212222122212220202212212201222222222121222221201210220102222202222222222022202202022221022222222220202222122122222212012222222222202202001221001222202212222022222220221212001100021222202022112220202222211102012212212002222222202212022212122222201220222220122122222212012222222222220222112222122222222202222122220221220222101010121222212210022120221211202022002212212212222022222212222222022222220021212222112222222222022222222222200202120220221222212202222222220221211212100101121222202111002220202202221002222212202022222122202212122211222222200222222220012122222222022222222222222222122221012222212212222222201222222222202101020222222001212021220212201112222202202202222022222222122202122222221122212221112222222212212222222222202222020210011222222212222222201221210202200220121201222201222121220201201022122212222002222222202212022221022222201220202221002022222202202222222222221222011221200222212212220222200220221222002001221211222122012222201202222002112222212212222022202222022211022222200222212220022112222202012222222222211202001201001222222222221222211221202212201111022212202002112022201202211222012212202002222122212202222201122222200121202222212012222202212220222222211202022210120222202202220222201221220202101122021201212120222222221200221122012212222012222222222222222221222222211220222222222212222222012221122202201212120212211222212222222122200222212222202110222211212020022122210220202002112202222002222022212202222202122222200120212222202122222212112221022212200202210201221222212222120122212220222222020101222212222201222021200210212202202212212102222022202212222221222222201022222220212122222212002222222202201212020220012222202222220222212221211202121020221202212120012221222210212102222222202012222022212222022211022222212021222221102212222222222222222202211212101200100222202222121222220220220202100101020212212221102121212222200102212222202122222222222222122102222222202122202220002022222220212222222212211212221201200222202202122222200221201202120102120221212101222120212200212012022222212012222222222202022010222222200022222222002202222200122221122202212222001222122222222212221122221220202202001022021200212012010222201221201012012212202022222222212212222002022222222221202220222222222220202221022212221202012212000222222212220122211221222202121000222211202111012120201220202212102202212002222222202212222022122222210022212221202102222211222220022212211222010220102222212222021222211220222222220021220201202122201122200202220222122202202012222122212202222000222222212222212221212022222202022121122222201202122220112222202202020122210220220212012121021212202200212122211211221112102202210122222122222202022021222222202021202221012102222211112221222202220222111210000222212202021122201221212222012221120212212001211021201211200012122202212002222022202222222220122222202020222222012022222222202020222202221202002200122222202212220122222220220202012100220221202001102221222212222202012222201222222122212222022221122222202020222222202102222222022222122202201202110222010222202222021122221220201222120012020222222200221221200221212002222222202102222022212202122002122022201022222222022122222220122120022222202202112222100222222222120122222221221112222202221222212010002222200202200002222212002112222222202212022222122022222220212220222202222221202221222222200212020220000222222212120222221220212002110222120212212000010022222221222222102202022222222022222202222121022222210122202220102202222211202121022212221202112211110222212212021022220222010222010001021211212022022122210201221102222222021102222222212202122210122102222220202222002022222210022020122222222212210201101222222222122222221220001012110121120200001121220022221212202002212222102202122022222212222110222102211222222221202202222220222121122212202212010200101220202212120222202222121012022001021211000222011020212201222002002222000102222122222202022220222212200022222221002002222202202220122202212222000222102221212212221122201220212002200021121211211021100020211220222202212222101022022022222212222220222202201211212220202202222201002020122222212222111202002220212222020212212222002022120221122211200102001120221201200012222222201002222222211222222210122112220022222202122102222201022021222212211212122202211220222222122002220221220012021001221201001102101022202202220022222212111011022222201222122202122222222200202202022122222202102121222202221202112211110222222222020222202221102122120221122221010112211121210201011122122202212112222122201202022010022112202012222201102112222212122020222222222202111202120222202202122102211222211012100121022220200210000121212200011102202202120011222222200222022220022222211102222220122202222200212220022222210212000210111220222212120122212220101212010102020200210002120122222222001022002212202102222022220222222101022002212111202211202002222212212122022202221202110201200222212212120012201222122202212011020212012222100221212202001112022212100212222122112222022102022222220211210201222202222200202021022002210222201201012222222212021122201221201102100001121200210222010020210211210002012222121002122122001202112100122122202111222210212202222212002221022122212212012211212221212212121022211222222102011000122121001202212022212212222022122202102202002222220212122022022022202020210211022002222222122021022012210222012202111222212212022102220222221112001012122111022022110220212220021002012212111211122022200222002211222012010211201211002002222221222121122102221222101210002221222222120102212220101102220200122022220000212222221212211222122222222001212220000212002021222202121212201222212222222211112021222112202222200221220222202202020022210221210102122211222122021112000121210212000012122202122001012220121022002121222012122000201221012212222210002021122222222212201222121222202202220102201222102212020211221011202011222221212210221102112212122002012121101102102102022112001221220220122222220212212121222002221202110201020221212202022102220222202002210211221220201200122221222211201122002122200211122120122002002020022212012211220210022222222220002020022102212222101220120222212212221112221222002212222012222000010211021020220222111122222022112121012221201112222200022012120020220202102122221200002022122112211202020211201220222222120012212221102222212221221022101200002022211220201112122222112120102222102002212002222112201220201221212220221201102221122102201222112210020220212222120012102220211022012002120000211110000022211220112002222222210220002121222102002120222102002110202211012210221220102220122222200212020200021220202222120212202222000222121211221210210121000021211212101102212112201011002121211112112222222002000201202222102010210200022000022012220222200222020222212222121002001222121212101000021212012202212020222211102022202022012010012222021110112200222021111000220200022000201201102121122102200202001222220222012212222212121220011112111022221210200111011022200220100012002112101012112021210011122220022102022212222200202110222221112001222012211212220200121220112212020212012220221112111200001102002201122220200222002112202002202011012022020211202002222120122022210211122201210222102001122002222102122220222220012212122022000221212012011222221000202102010220221221222022012102122111202222112222212111122102022222201220122120202220112002122202221222000221102201012212121112110222111022100102120101201020202221201222022102222122102012112220002001222120122222022211221210002121202202222202222122222102012212012210102202220112112020001122011002100000200122200221222222121220222202002221002021121001222210222100110022200220222121221211112212122012201222202221101200202202222212100022012212212222200120110210121022101200210021102112022012122221120020112220222000020020210212202021222221112120022112201122210212020202112202120122010020201112101222220000120002002022210210022220012112200011212221201012002222122211020101212211222121220221202021222202222112121222220212202212121212201022110212110111110010102220112221101212202102212222211201102022112022011011222210221002210201112020222201012121022112100022120222121220211120211001120210211020102000200102112021201102012000220121010201212122010202001110200012200100022020022110111220010011010002010011201000010122212121";
            //string input = "123456789012";

            List<string> layers = new List<string>();
            int layersize = 25 * 6;
            int layernum = input.Length / layersize;

            for (int i = 0; i < layernum; i++)
            {
                layers.Add(input.Substring(i * layersize, layersize));
            }

            var minLayerNum = layers.Min(s => s.Count(c => c == '0'));
            var minLayer = layers.Single(s => s.Count(c => c == '0') == minLayerNum);
            var checksum = minLayer.Count(c => c == '1') * minLayer.Count(c => c == '2');
            Console.WriteLine(checksum);


            List<char> visibleLayer = new List<char>();
            for(int i =0;i<layers.First().Length;i++)
            {
                visibleLayer.Add('2');
            }

            foreach (var layer in layers)
            {
                int i = 0;
                foreach(var p in layer)
                {
                    if(visibleLayer[i] == '2')
                    {
                        visibleLayer[i] = p;
                    }
                    i++;
                }
            }

            int j = 0;
            foreach(var p in visibleLayer)
            {
                if(j % 25 == 0)
                {
                    Console.WriteLine();
                }
                if (p == '0')
                {
                    Console.Write(" ");
                }
                if(p == '1')
                    {
                    Console.Write("X");

                }
                j++;
            }

            Console.ReadLine();
        }
    }
}
