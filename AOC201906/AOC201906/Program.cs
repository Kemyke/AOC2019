using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC201906
{
    class Orbit
    {
        public string Name { get; set; }
        public Orbit CenterMass { get; set; }
        public List<Orbit> Orbits { get; set; }
        public int PathLength { get; set; } = 0;
    }

    class Program
    {
        static Dictionary<string, Orbit> orbits = new Dictionary<string, Orbit>();
        static List<Orbit> visitedOrbits = new List<Orbit>();

        static int ShortestPath(Orbit o, Orbit end)
        {
            List<Orbit> visitableOrbits = new List<Orbit>();
            o.PathLength = 0;
            visitableOrbits.Add(o);
            while (visitableOrbits.Any())
            {
                var currentOrbit = visitableOrbits.First();
                visitableOrbits.Remove(currentOrbit);
                visitedOrbits.Add(currentOrbit);

                foreach (var co in currentOrbit.Orbits.Where(a => !visitedOrbits.Contains(a)))
                {
                    co.PathLength = currentOrbit.PathLength + 1;
                    visitableOrbits.Add(co);    
                }

                if(currentOrbit.CenterMass != null && !visitedOrbits.Contains(currentOrbit.CenterMass))
                {
                    currentOrbit.CenterMass.PathLength = currentOrbit.PathLength + 1;
                    visitableOrbits.Add(currentOrbit.CenterMass);
                }
            }

            return end.PathLength;
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            foreach(var line in lines)
            {
                var os = line.Split(")");
                var cm = os[0];
                var o = os[1];
                Orbit centermass = null;
                if(!orbits.ContainsKey(cm))
                {
                    centermass = new Orbit { Name = cm, CenterMass = null, Orbits = new List<Orbit>() };
                    orbits.Add(cm, centermass);
                }
                else
                {
                    centermass = orbits[cm];
                }

                Orbit orbit = null;
                if (!orbits.ContainsKey(o))
                {
                    orbit = new Orbit { Name = o, CenterMass = centermass, Orbits = new List<Orbit>() };
                    orbits.Add(o, orbit);
                }
                else
                {
                    orbit = orbits[o];
                    if(orbit.CenterMass == null)
                    {
                        orbit.CenterMass = centermass;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                centermass.Orbits.Add(orbit);
            }

            int checksum = 0;
            foreach(var orbit in orbits.Values)
            {
                var co = orbit;
                while(co.CenterMass != null)
                {
                    checksum++;
                    co = co.CenterMass;
                }
            }

            var sp = ShortestPath(orbits["YOU"], orbits["SAN"]);

            Console.WriteLine(checksum);
            Console.WriteLine(sp);
            Console.ReadLine();
        }
    }
}
