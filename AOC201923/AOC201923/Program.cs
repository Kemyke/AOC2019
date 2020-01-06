﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AOC201923
{
    class Program
    {
        class Packet
        {
            public int Source { get; set; }
            public int Destination { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
        }

        static void SendPacket(Packet packet)
        {
            if (computers.ContainsKey(packet.Destination))
            {
                computers[packet.Destination].EnqueuePacket(packet);
                Console.WriteLine(packet.Source + " -> " + packet.Destination + ": " + packet.X + " " + packet.Y);
            }
            else
            {
                if (packet.Destination == 255)
                {
                    Console.WriteLine(packet.Source + " -> NAT: " + packet.X + " " + packet.Y);
                    Nat.ReceivePacket(packet);
                }
                else
                {
                    Console.WriteLine("!!! " + packet.Source + " -> " + packet.Destination + ": " + packet.X + " " + packet.Y);
                }
            }
        }

        static List<Packet> sentNatPackages = new List<Packet>();

        class NAT
        {
            private object lockObj = new object();
            private Packet LastPacket { get; set; }
            public void ReceivePacket(Packet p)
            {
                lock (lockObj)
                {
                    LastPacket = p;
                }
            }

            public void Run()
            {
                while(true)
                {
                    var networkIdle = computers.Values.All(c => c.Idle);
                    //Console.WriteLine("NAT! Network " + (networkIdle ? "idle" : "busy"));
                    if(networkIdle && LastPacket != null)
                    {
                        lock (lockObj)
                        {
                            ResumeNetwork();
                        }
                    }
                    Thread.Sleep(1000);
                }
            }

            private void ResumeNetwork()
            {
                Packet p = new Packet { Source = 255, Destination = 0, X = LastPacket.X, Y = LastPacket.Y };
                Console.WriteLine("NAT resume network: " + LastPacket.X + " " + LastPacket.Y);
                SendPacket(p);
                if(sentNatPackages.Any() && sentNatPackages.Last().Y == p.Y)
                {
                    Console.WriteLine("Duplicate NAT Y: " + p.Y);
                }
                sentNatPackages.Add(p);
                LastPacket = null;
            }
        }

        class Computer
        {
            public int Id { get; }
            private Queue<Packet> ReceiveQueue = new Queue<Packet>();

            public Computer(List<long> program, int id)
            {
                this.program = program;
                Id = id;
            }

            public bool Idle { get { return !receiving && !sending; } }

            public void EnqueuePacket(Packet packet)
            {
                lock (ReceiveQueue)
                {
                    ReceiveQueue.Enqueue(packet);
                }
            }

            public void Run()
            {
                Compute();
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

            private Packet currPacket = null;
            int i = 0;
            int o = 0;
            private Packet outPacket = null;
            bool booted = false;
            bool sending = false;
            bool receiving = false;

            private Tuple<long, bool> Compute()
            {
                int currOpCode = 0;
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
                            long input = -1;
                            if (booted)
                            {
                                if (i == 0)
                                {
                                    lock (ReceiveQueue)
                                    {
                                        if (ReceiveQueue.Any())
                                        {
                                            receiving = true;
                                            currPacket = ReceiveQueue.Dequeue();
                                        }
                                        else
                                        {
                                            currPacket = null;
                                            receiving = false;
                                            Thread.Sleep(1000);
                                        }
                                    }
                                }
                                if (currPacket != null)
                                {
                                    if (i == 0)
                                    {
                                        input = currPacket.X;
                                        i = 1;
                                    }
                                    else
                                    {
                                        input = currPacket.Y;
                                        currPacket = null;
                                        i = 0;
                                    }
                                }
                            }
                            else
                            {
                                input = Id;
                                booted = true;
                            }
                            program[GetIndex(paramModes[0], currPosition + 1)] = input;
                            currPosition += 2;
                            break;
                        case 4:
                            paramModes = GetModes(inst, 1);
                            long output = GetParameter(paramModes[0], currPosition + 1);
                            if (o == 0)
                            {
                                sending = true;
                                outPacket = new Packet { Source = Id, Destination = (int)output };
                                o = 1;
                            }
                            else if (o == 1)
                            {
                                outPacket.X = output;
                                o = 2;
                            }
                            else if (o == 2)
                            {
                                outPacket.Y = output;
                                o = 0;
                                SendPacket(outPacket);
                                outPacket = null;
                                sending = false;
                            }
                            currPosition += 2;
                            break;
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
                            return new Tuple<long, bool>(0, true);
                        default:
                            throw new Exception();
                    }

                    Thread.Sleep(100);
                }
                throw new Exception();
            }
        }

        static Dictionary<int, Computer> computers = new Dictionary<int, Computer>();
        static NAT Nat = new NAT();

        static void Main(string[] args)
        {
            List<long> program = new List<long> { 3, 62, 1001, 62, 11, 10, 109, 2245, 105, 1, 0, 1553, 1769, 666, 1866, 1699, 2216, 606, 1139, 1965, 2084, 1629, 790, 1104, 1046, 1440, 1660, 1594, 1730, 1075, 571, 1831, 1802, 951, 1339, 1178, 920, 2185, 1207, 2154, 2006, 637, 2045, 695, 1479, 1930, 1899, 1516, 982, 1240, 1269, 852, 730, 2123, 883, 1409, 1378, 1017, 821, 1308, 761, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 64, 1008, 64, -1, 62, 1006, 62, 88, 1006, 61, 170, 1106, 0, 73, 3, 65, 20101, 0, 64, 1, 20102, 1, 66, 2, 21101, 0, 105, 0, 1105, 1, 436, 1201, 1, -1, 64, 1007, 64, 0, 62, 1005, 62, 73, 7, 64, 67, 62, 1006, 62, 73, 1002, 64, 2, 132, 1, 132, 68, 132, 1002, 0, 1, 62, 1001, 132, 1, 140, 8, 0, 65, 63, 2, 63, 62, 62, 1005, 62, 73, 1002, 64, 2, 161, 1, 161, 68, 161, 1102, 1, 1, 0, 1001, 161, 1, 169, 102, 1, 65, 0, 1102, 1, 1, 61, 1102, 1, 0, 63, 7, 63, 67, 62, 1006, 62, 203, 1002, 63, 2, 194, 1, 68, 194, 194, 1006, 0, 73, 1001, 63, 1, 63, 1106, 0, 178, 21101, 0, 210, 0, 105, 1, 69, 1202, 1, 1, 70, 1101, 0, 0, 63, 7, 63, 71, 62, 1006, 62, 250, 1002, 63, 2, 234, 1, 72, 234, 234, 4, 0, 101, 1, 234, 240, 4, 0, 4, 70, 1001, 63, 1, 63, 1106, 0, 218, 1106, 0, 73, 109, 4, 21101, 0, 0, -3, 21101, 0, 0, -2, 20207, -2, 67, -1, 1206, -1, 293, 1202, -2, 2, 283, 101, 1, 283, 283, 1, 68, 283, 283, 22001, 0, -3, -3, 21201, -2, 1, -2, 1105, 1, 263, 21201, -3, 0, -3, 109, -4, 2106, 0, 0, 109, 4, 21101, 1, 0, -3, 21101, 0, 0, -2, 20207, -2, 67, -1, 1206, -1, 342, 1202, -2, 2, 332, 101, 1, 332, 332, 1, 68, 332, 332, 22002, 0, -3, -3, 21201, -2, 1, -2, 1105, 1, 312, 21201, -3, 0, -3, 109, -4, 2105, 1, 0, 109, 1, 101, 1, 68, 359, 20102, 1, 0, 1, 101, 3, 68, 366, 21002, 0, 1, 2, 21101, 0, 376, 0, 1106, 0, 436, 22101, 0, 1, 0, 109, -1, 2106, 0, 0, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576, 2097152, 4194304, 8388608, 16777216, 33554432, 67108864, 134217728, 268435456, 536870912, 1073741824, 2147483648, 4294967296, 8589934592, 17179869184, 34359738368, 68719476736, 137438953472, 274877906944, 549755813888, 1099511627776, 2199023255552, 4398046511104, 8796093022208, 17592186044416, 35184372088832, 70368744177664, 140737488355328, 281474976710656, 562949953421312, 1125899906842624, 109, 8, 21202, -6, 10, -5, 22207, -7, -5, -5, 1205, -5, 521, 21101, 0, 0, -4, 21101, 0, 0, -3, 21102, 51, 1, -2, 21201, -2, -1, -2, 1201, -2, 385, 471, 20101, 0, 0, -1, 21202, -3, 2, -3, 22207, -7, -1, -5, 1205, -5, 496, 21201, -3, 1, -3, 22102, -1, -1, -5, 22201, -7, -5, -7, 22207, -3, -6, -5, 1205, -5, 515, 22102, -1, -6, -5, 22201, -3, -5, -3, 22201, -1, -4, -4, 1205, -2, 461, 1105, 1, 547, 21102, 1, -1, -4, 21202, -6, -1, -6, 21207, -7, 0, -5, 1205, -5, 547, 22201, -7, -6, -7, 21201, -4, 1, -4, 1106, 0, 529, 22101, 0, -4, -7, 109, -8, 2105, 1, 0, 109, 1, 101, 1, 68, 564, 20102, 1, 0, 0, 109, -1, 2106, 0, 0, 1102, 3673, 1, 66, 1101, 0, 3, 67, 1101, 0, 598, 68, 1101, 302, 0, 69, 1101, 0, 1, 71, 1102, 604, 1, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 29, 96377, 1101, 0, 99377, 66, 1101, 1, 0, 67, 1102, 633, 1, 68, 1102, 1, 556, 69, 1101, 0, 1, 71, 1101, 635, 0, 72, 1106, 0, 73, 1, -195, 7, 179734, 1101, 41593, 0, 66, 1102, 1, 1, 67, 1102, 1, 664, 68, 1101, 0, 556, 69, 1102, 1, 0, 71, 1102, 666, 1, 72, 1106, 0, 73, 1, 1977, 1102, 1, 15683, 66, 1102, 1, 1, 67, 1102, 1, 693, 68, 1102, 556, 1, 69, 1102, 0, 1, 71, 1101, 0, 695, 72, 1106, 0, 73, 1, 1403, 1101, 2287, 0, 66, 1101, 0, 1, 67, 1101, 0, 722, 68, 1102, 556, 1, 69, 1101, 3, 0, 71, 1102, 1, 724, 72, 1106, 0, 73, 1, 10, 14, 282265, 33, 291908, 8, 156316, 1101, 68483, 0, 66, 1102, 1, 1, 67, 1102, 757, 1, 68, 1101, 0, 556, 69, 1102, 1, 1, 71, 1102, 759, 1, 72, 1105, 1, 73, 1, -263, 16, 104089, 1102, 1, 89083, 66, 1102, 1, 1, 67, 1102, 788, 1, 68, 1102, 556, 1, 69, 1101, 0, 0, 71, 1102, 1, 790, 72, 1105, 1, 73, 1, 1500, 1102, 1, 74383, 66, 1102, 1, 1, 67, 1102, 817, 1, 68, 1102, 556, 1, 69, 1101, 1, 0, 71, 1102, 1, 819, 72, 1106, 0, 73, 1, 673, 19, 7346, 1102, 1, 19457, 66, 1102, 1, 1, 67, 1102, 1, 848, 68, 1102, 556, 1, 69, 1101, 1, 0, 71, 1101, 850, 0, 72, 1105, 1, 73, 1, 30, 1, 23957, 1102, 54679, 1, 66, 1101, 1, 0, 67, 1101, 879, 0, 68, 1102, 1, 556, 69, 1102, 1, 1, 71, 1101, 881, 0, 72, 1105, 1, 73, 1, 361, 19, 11019, 1101, 34673, 0, 66, 1101, 0, 4, 67, 1102, 910, 1, 68, 1102, 302, 1, 69, 1101, 1, 0, 71, 1101, 0, 918, 72, 1105, 1, 73, 0, 0, 0, 0, 0, 0, 0, 0, 29, 192754, 1102, 9397, 1, 66, 1101, 0, 1, 67, 1101, 0, 947, 68, 1102, 1, 556, 69, 1101, 1, 0, 71, 1101, 0, 949, 72, 1106, 0, 73, 1, 89, 43, 34673, 1102, 1, 11273, 66, 1102, 1, 1, 67, 1101, 978, 0, 68, 1101, 556, 0, 69, 1102, 1, 1, 71, 1102, 1, 980, 72, 1105, 1, 73, 1, 353, 37, 270813, 1102, 90271, 1, 66, 1101, 0, 3, 67, 1102, 1009, 1, 68, 1101, 0, 302, 69, 1101, 0, 1, 71, 1101, 1015, 0, 72, 1105, 1, 73, 0, 0, 0, 0, 0, 0, 14, 225812, 1101, 0, 82567, 66, 1102, 1, 1, 67, 1102, 1044, 1, 68, 1101, 556, 0, 69, 1101, 0, 0, 71, 1102, 1, 1046, 72, 1106, 0, 73, 1, 1385, 1102, 102253, 1, 66, 1101, 1, 0, 67, 1101, 0, 1073, 68, 1102, 1, 556, 69, 1101, 0, 0, 71, 1101, 1075, 0, 72, 1105, 1, 73, 1, 1870, 1101, 5651, 0, 66, 1101, 0, 1, 67, 1102, 1102, 1, 68, 1102, 556, 1, 69, 1102, 1, 0, 71, 1101, 0, 1104, 72, 1106, 0, 73, 1, 1802, 1102, 56093, 1, 66, 1101, 0, 1, 67, 1101, 0, 1131, 68, 1102, 556, 1, 69, 1102, 1, 3, 71, 1102, 1133, 1, 72, 1105, 1, 73, 1, 13, 9, 92383, 31, 211467, 7, 449335, 1101, 0, 89867, 66, 1102, 1, 5, 67, 1102, 1, 1166, 68, 1102, 1, 302, 69, 1102, 1, 1, 71, 1101, 0, 1176, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 272661, 1101, 26881, 0, 66, 1101, 0, 1, 67, 1101, 0, 1205, 68, 1102, 1, 556, 69, 1101, 0, 0, 71, 1102, 1207, 1, 72, 1105, 1, 73, 1, 1015, 1102, 1, 1489, 66, 1101, 0, 2, 67, 1102, 1, 1234, 68, 1102, 1, 302, 69, 1101, 0, 1, 71, 1101, 0, 1238, 72, 1106, 0, 73, 0, 0, 0, 0, 36, 220244, 1101, 13567, 0, 66, 1102, 1, 1, 67, 1102, 1267, 1, 68, 1101, 556, 0, 69, 1101, 0, 0, 71, 1102, 1269, 1, 72, 1105, 1, 73, 1, 1451, 1102, 57089, 1, 66, 1102, 1, 1, 67, 1102, 1296, 1, 68, 1101, 0, 556, 69, 1102, 5, 1, 71, 1102, 1, 1298, 72, 1106, 0, 73, 1, 2, 9, 184766, 31, 70489, 7, 359468, 8, 39079, 8, 195395, 1101, 6661, 0, 66, 1102, 1, 1, 67, 1102, 1, 1335, 68, 1102, 556, 1, 69, 1101, 1, 0, 71, 1102, 1337, 1, 72, 1106, 0, 73, 1, 1733, 29, 481885, 1102, 1, 38651, 66, 1101, 0, 1, 67, 1102, 1366, 1, 68, 1101, 556, 0, 69, 1101, 5, 0, 71, 1101, 1368, 0, 72, 1105, 1, 73, 1, 5, 9, 461915, 31, 281956, 33, 145954, 33, 218931, 8, 78158, 1101, 0, 3389, 66, 1101, 0, 1, 67, 1102, 1, 1405, 68, 1102, 1, 556, 69, 1102, 1, 1, 71, 1102, 1407, 1, 72, 1106, 0, 73, 1, 283, 16, 208178, 1102, 1, 45853, 66, 1101, 0, 1, 67, 1102, 1, 1436, 68, 1102, 1, 556, 69, 1102, 1, 1, 71, 1101, 1438, 0, 72, 1106, 0, 73, 1, 201, 43, 69346, 1101, 56453, 0, 66, 1102, 1, 5, 67, 1101, 1467, 0, 68, 1102, 1, 302, 69, 1102, 1, 1, 71, 1102, 1, 1477, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 27, 2978, 1101, 0, 72977, 66, 1101, 4, 0, 67, 1102, 1, 1506, 68, 1102, 302, 1, 69, 1102, 1, 1, 71, 1102, 1, 1514, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 8, 234474, 1101, 0, 55061, 66, 1101, 0, 4, 67, 1102, 1543, 1, 68, 1102, 253, 1, 69, 1102, 1, 1, 71, 1101, 0, 1551, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 3, 16871, 1101, 0, 37571, 66, 1102, 1, 1, 67, 1102, 1580, 1, 68, 1101, 556, 0, 69, 1101, 0, 6, 71, 1102, 1582, 1, 72, 1106, 0, 73, 1, 24874, 27, 1489, 20, 90887, 20, 181774, 34, 36353, 34, 72706, 34, 109059, 1101, 104089, 0, 66, 1101, 3, 0, 67, 1101, 0, 1621, 68, 1102, 302, 1, 69, 1102, 1, 1, 71, 1101, 1627, 0, 72, 1105, 1, 73, 0, 0, 0, 0, 0, 0, 29, 289131, 1101, 0, 71483, 66, 1101, 1, 0, 67, 1102, 1, 1656, 68, 1102, 1, 556, 69, 1101, 0, 1, 71, 1101, 0, 1658, 72, 1106, 0, 73, 1, 170, 14, 112906, 1101, 31793, 0, 66, 1101, 1, 0, 67, 1101, 0, 1687, 68, 1102, 1, 556, 69, 1102, 1, 5, 71, 1102, 1689, 1, 72, 1106, 0, 73, 1, 1, 16, 312267, 43, 138692, 19, 3673, 37, 90271, 14, 169359, 1102, 1, 21397, 66, 1101, 1, 0, 67, 1101, 1726, 0, 68, 1102, 1, 556, 69, 1102, 1, 1, 71, 1101, 1728, 0, 72, 1105, 1, 73, 1, 125, 33, 72977, 1102, 1, 91571, 66, 1102, 1, 1, 67, 1101, 0, 1757, 68, 1102, 1, 556, 69, 1101, 5, 0, 71, 1102, 1, 1759, 72, 1105, 1, 73, 1, 3, 43, 104019, 9, 277149, 31, 352445, 7, 89867, 7, 269601, 1102, 1, 23957, 66, 1101, 0, 2, 67, 1102, 1796, 1, 68, 1101, 0, 302, 69, 1102, 1, 1, 71, 1102, 1, 1800, 72, 1105, 1, 73, 0, 0, 0, 0, 9, 369532, 1101, 0, 28669, 66, 1102, 1, 1, 67, 1102, 1829, 1, 68, 1101, 556, 0, 69, 1101, 0, 0, 71, 1101, 0, 1831, 72, 1105, 1, 73, 1, 1520, 1102, 1, 90887, 66, 1102, 3, 1, 67, 1102, 1, 1858, 68, 1101, 302, 0, 69, 1102, 1, 1, 71, 1101, 0, 1864, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 36, 55061, 1101, 0, 16871, 66, 1102, 1, 2, 67, 1102, 1, 1893, 68, 1101, 351, 0, 69, 1101, 1, 0, 71, 1102, 1897, 1, 72, 1106, 0, 73, 0, 0, 0, 0, 255, 37571, 1102, 43189, 1, 66, 1102, 1, 1, 67, 1102, 1926, 1, 68, 1102, 1, 556, 69, 1101, 1, 0, 71, 1101, 0, 1928, 72, 1105, 1, 73, 1, 23, 14, 56453, 1101, 0, 36353, 66, 1101, 0, 3, 67, 1102, 1957, 1, 68, 1101, 0, 302, 69, 1102, 1, 1, 71, 1102, 1963, 1, 72, 1105, 1, 73, 0, 0, 0, 0, 0, 0, 36, 165183, 1101, 0, 39079, 66, 1101, 6, 0, 67, 1101, 1992, 0, 68, 1101, 302, 0, 69, 1102, 1, 1, 71, 1102, 2004, 1, 72, 1105, 1, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 33742, 1102, 1, 96377, 66, 1102, 5, 1, 67, 1102, 1, 2033, 68, 1102, 253, 1, 69, 1101, 1, 0, 71, 1101, 2043, 0, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 47914, 1101, 0, 70489, 66, 1102, 5, 1, 67, 1102, 1, 2072, 68, 1102, 1, 302, 69, 1101, 0, 1, 71, 1101, 2082, 0, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 36, 110122, 1101, 92383, 0, 66, 1101, 0, 5, 67, 1102, 1, 2111, 68, 1101, 302, 0, 69, 1102, 1, 1, 71, 1102, 2121, 1, 72, 1106, 0, 73, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, 140978, 1101, 0, 69899, 66, 1101, 0, 1, 67, 1102, 2150, 1, 68, 1102, 1, 556, 69, 1101, 1, 0, 71, 1101, 2152, 0, 72, 1106, 0, 73, 1, 4929, 29, 385508, 1101, 0, 84751, 66, 1101, 1, 0, 67, 1102, 2181, 1, 68, 1101, 0, 556, 69, 1101, 0, 1, 71, 1101, 2183, 0, 72, 1105, 1, 73, 1, 101, 37, 180542, 1101, 0, 35159, 66, 1101, 1, 0, 67, 1102, 2212, 1, 68, 1102, 556, 1, 69, 1101, 1, 0, 71, 1101, 2214, 0, 72, 1105, 1, 73, 1, 160, 8, 117237, 1102, 31181, 1, 66, 1101, 0, 1, 67, 1101, 2243, 0, 68, 1102, 556, 1, 69, 1102, 1, 0, 71, 1101, 0, 2245, 72, 1106, 0, 73, 1, 1796 };
            program.AddRange(Enumerable.Repeat((long)0, program.Count * 100));

            for (int i = 0; i < 50; i++)
            {
                var c = new Computer(program.ToList(), i);
                computers.Add(i, c);
            }

            Task.Run(() => { Nat.Run(); });

            foreach (var c in computers.Values)
            {
                Task.Run(() => { c.Run(); });
            }
            Console.ReadLine();
        }
    }
}
