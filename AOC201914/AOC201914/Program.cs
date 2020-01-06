using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC201914
{
    class Program
    {
        class Ingredient
        {
            public string Chemical { get; set; }
            public int Amount { get; set; }
        }

        class Reaction
        {
            public List<Ingredient> Inputs { get; set; } = new List<Ingredient>();
            public Ingredient Output { get; set; }
        }

        static List<Reaction> PossibleReactions = new List<Reaction>();

        static void Parse(string[] lines)
        {
            foreach (var line in lines)
            {
                Reaction nr = new Reaction();
                var io = line.Split(" => ");
                var o = io[1].Split(" ");
                nr.Output = new Ingredient { Chemical = o[1], Amount = int.Parse(o[0]) };

                var i = io[0].Split(", ");
                foreach (var ii in i)
                {
                    var iii = ii.Split(" ");
                    nr.Inputs.Add(new Ingredient { Chemical = iii[1], Amount = int.Parse(iii[0]) });
                }
                PossibleReactions.Add(nr);
            }
        }

        static Dictionary<string, long> inventory = new Dictionary<string, long>();
        static HashSet<string> states = new HashSet<string>();
        static List<string> states2 = new List<string>();
        static List<long> ores = new List<long>();

        static void Main(string[] args)
        {
            Parse(File.ReadAllLines("input.txt"));
            List<Ingredient> fis = PossibleReactions.Single(r => r.Output.Chemical == "FUEL").Inputs;

            inventory.Add("ORE", 0);

            int fuel = 0;
            long fr = 1572358;

            foreach (var fi in fis)
            {
                if (!inventory.ContainsKey(fi.Chemical))
                {
                    inventory.Add(fi.Chemical, 0);
                }
                inventory[fi.Chemical] -= fi.Amount * fr;
            }

            while (inventory.Where(i => i.Key != "ORE").Any(kvp => kvp.Value < 0))
            {
                var c = inventory.Where(i => i.Key != "ORE").First(kvp => kvp.Value < 0);
                var reaction = PossibleReactions.Single(r => r.Output.Chemical == c.Key);
                while (inventory[reaction.Output.Chemical] < 0)
                {
                    var n = (long)Math.Ceiling(Math.Abs((decimal)inventory[reaction.Output.Chemical]) / reaction.Output.Amount);

                    foreach (var ri in reaction.Inputs)
                    {
                        if (!inventory.ContainsKey(ri.Chemical))
                        {
                            inventory.Add(ri.Chemical, 0);
                        }
                        inventory[ri.Chemical] -= n * ri.Amount;
                    }

                    inventory[reaction.Output.Chemical] += n * reaction.Output.Amount;
                }
            }

            Console.WriteLine(inventory["ORE"]);
            Console.WriteLine(fuel);
            Console.ReadLine();
        }
    }
}