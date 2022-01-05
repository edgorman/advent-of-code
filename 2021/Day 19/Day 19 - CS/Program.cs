
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            Queue<Scanner> scanners = new Queue<Scanner>();
            Scanner scanner = new Scanner();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("--- scanner"))
                {
                    scanner = new Scanner();
                }
                else if (lines[i].Length == 0)
                {
                    scanners.Enqueue(scanner);
                }
                else
                {
                    var values = lines[i].Split(',').ToList().ConvertAll(int.Parse);
                    scanner.AddBeacon(new Beacon(values[0], values[1], values[2]));
                }
            }
            scanners.Enqueue(scanner);

            // Part 1
            Console.WriteLine(PartOne(scanners));

            // Part 2
            Console.WriteLine(PartTwo(scanners));
        }

        static string PartOne(Queue<Scanner> scanners)
        {
            var s0 = scanners.Dequeue();
            int scannerCheckCount = 0;

            // While there are scanners remaining
            while (scanners.Count > 0)
            {
                // Get the front scanner
                var si = scanners.Dequeue();
                scannerCheckCount++;

                // Check if number of scanners checked equals those remaining
                // Replace s0 with another scanner, add s0 to the queue
                if (scannerCheckCount > scanners.Count)
                {
                    scanners.Enqueue(s0);
                    s0 = si;
                    si = scanners.Dequeue();
                    scannerCheckCount = 0;
                }

                // Get dictionary of distances between beacons in scanner 1 and scanner i
                // Key is distance, value is pair of beacons
                // Only keep those shorter than max distance 500
                var d0 = s0.GetDistancesPairs(500);
                var di = si.GetDistancesPairs(500);

                // Find the distances that match
                var k0 = d0.Keys;
                var ki = di.Keys;
                var matchingKeys = k0.Intersect(ki);

                // If at least 12 pairs of beacons match
                // Then try all orientations of scanner i to identify the matching pairs
                if (matchingKeys.Any())
                {
                    // Get indexes of first matching pairs
                    var key = matchingKeys.First();
                    var i0 = d0[key];
                    var ii = di[key];

                    // Try every possible orientation and translation
                    // Until a match is found
                    Scanner? match = null;
                    foreach (var so in si.GetAllOrientations())
                    {
                        // Get pairs of beacons that matched
                        var p0 = Tuple.Create(s0.GetIndex(i0.Item1), s0.GetIndex(i0.Item2));
                        var po = Tuple.Create(so.GetIndex(ii.Item1), so.GetIndex(ii.Item2));

                        // Get possible translations from pair i to pair 0
                        var translations = new List<Beacon>();
                        translations.Add(po.Item1.Subtract(p0.Item1));
                        translations.Add(po.Item1.Subtract(p0.Item2));
                        translations.Add(p0.Item1.Subtract(po.Item1));
                        translations.Add(p0.Item1.Subtract(po.Item2));

                        foreach (var translation in translations)
                        {
                            var ti = so.ApplyTranslation(translation);

                            // Find beacons that match and consider the orientation a match
                            // When there are at least 12 matching beacons
                            var matchingBeacons = s0.GetMatchingBeacons(ti);

                            if (matchingBeacons.Count >= Math.Min(ti.Count(), 12))
                            {
                                match = ti;
                                break;
                            }
                        }

                        if (match != null) { break; }
                    }

                    // If match was found, add beacons from match to scanner 0
                    if (match != null)
                    {
                        scannerCheckCount = 0;

                        for (int j = 0; j < match.Count(); j++)
                        {
                            s0.AddBeacon(match.GetIndex(j));
                        }
                    }
                    // Else add scanner to back of queue
                    else
                    {
                        scanners.Enqueue(si);
                    }
                }
                // Else add scanner to back of queue
                else
                {
                    scanners.Enqueue(si);
                }

            }

            return s0.Count().ToString();
        }

        static string PartTwo(Queue<Scanner> scanners)
        {
            return "";
        }
    }

    class Beacon
    {
        private int _x;
        private int _y;
        private int _z;

        public Beacon(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public int X() { return _x; }
        public int Y() { return _y; }
        public int Z() { return _z; }

        public Tuple<int, int, int> Get()
        {
            return Tuple.Create(_x, _y, _z);
        }

        public Beacon Subtract(Beacon b)
        {
            return new Beacon(X() - b.X(), Y() - b.Y(), Z() - b.Z());
        }

        public Beacon Translate(int x, int y, int z)
        {
            return new Beacon(X() + x, Y() + y, Z() + z);
        }

        public double DistanceTo(Beacon b)
        {
            return Math.Sqrt(
                Math.Pow(Math.Abs(X() - b.X()), 2) + 
                Math.Pow(Math.Abs(Y() - b.Y()), 2) + 
                Math.Pow(Math.Abs(Z() - b.Z()), 2)
            );
        }

        public bool Equals(Beacon other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return X().Equals(other.X())
                   && Y().Equals(other.Y())
                   && Z().Equals(other.Z());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Beacon);
        }

        public override string ToString()
        {
            return X().ToString() + ", " + Y().ToString() + ", " + Z().ToString();
        }
    }

    class Scanner
    {
        private List<Beacon> beacons;

        public Scanner()
        {
            beacons = new List<Beacon>();
        }

        public void AddBeacon(Beacon b)
        {
            if (!Contains(b))
            {
                beacons.Add(b);
            }
        }

        public Beacon GetIndex(int index)
        {
            return beacons[index];
        }

        public int Count()
        {
            return beacons.Count;
        }

        public bool Contains(Beacon b)
        {
            return beacons.Contains(b);
        }

        public List<Beacon> GetMatchingBeacons(Scanner other)
        {
            return beacons.FindAll(b => other.Contains(b)).ToList();
        }

        public Dictionary<double, Tuple<int, int>> GetDistancesPairs(int maxDistance)
        {
            var result = new Dictionary<double, Tuple<int, int>>();

            for (int i = 0; i < beacons.Count; i++)
            {
                for (int j = i + 1; j < beacons.Count; j++)
                {
                    var distance = beacons[i].DistanceTo(beacons[j]);

                    if (distance > 0 && distance < maxDistance && !result.ContainsKey(distance))
                    {
                        result.Add(distance, Tuple.Create(i, j));
                    }
                }
            }

            return result;
        }

        public Scanner ApplyTranslation(Beacon t)
        {
            Scanner translated = new Scanner();

            foreach(var beacon in beacons)
            {
                translated.AddBeacon(beacon.Translate(t.X(), t.Y(), t.Z()));
            }

            return translated;
        }

        public List<Scanner> GetAllOrientations()
        {
            var orientations = new List<Scanner>();
            for (int i = 0; i < 24; i++)
            {
                orientations.Add(new Scanner());
            }

            foreach (var beacon in beacons)
            {
                var x = beacon.X();
                var y = beacon.Y();
                var z = beacon.Z();

                orientations[0].AddBeacon(new Beacon(x, y, z));
                orientations[1].AddBeacon(new Beacon(-y, x, z));
                orientations[2].AddBeacon(new Beacon(-x, -y, z));
                orientations[3].AddBeacon(new Beacon(y, -x, z));
                orientations[4].AddBeacon(new Beacon(-z, y, x));
                orientations[5].AddBeacon(new Beacon(-y, -z, x));
                orientations[6].AddBeacon(new Beacon(z, -y, x));
                orientations[7].AddBeacon(new Beacon(y, z, x));
                orientations[8].AddBeacon(new Beacon(-x, y, -z));
                orientations[9].AddBeacon(new Beacon(-y, -x, -z));
                orientations[10].AddBeacon(new Beacon(x, -y, -z));
                orientations[11].AddBeacon(new Beacon(y, x, -z));
                orientations[12].AddBeacon(new Beacon(z, y, -x));
                orientations[13].AddBeacon(new Beacon(-y, z, -x));
                orientations[14].AddBeacon(new Beacon(-z, -y, -x));
                orientations[15].AddBeacon(new Beacon(y, -z, -x));
                orientations[16].AddBeacon(new Beacon(x, -z, y));
                orientations[17].AddBeacon(new Beacon(z, x, y));
                orientations[18].AddBeacon(new Beacon(-x, z, y));
                orientations[19].AddBeacon(new Beacon(-z, -x, y));
                orientations[20].AddBeacon(new Beacon(x, z, -y));
                orientations[21].AddBeacon(new Beacon(-z, x, -y));
                orientations[22].AddBeacon(new Beacon(-x, -z, -y));
                orientations[23].AddBeacon(new Beacon(z, -x, -y));
            }

            return orientations.ToList();
        }
    }

}
