
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");

            // Clean input
            var scanners = new List<Scanner>();
            var scanner = new Scanner();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("--- scanner"))
                {
                    scanner = new Scanner(int.Parse(lines[i].Split(' ')[2]));
                }
                else if (lines[i].Length == 0)
                {
                    scanners.Add(scanner);
                }
                else
                {
                    var values = lines[i].Split(',').ToList().ConvertAll(int.Parse);
                    scanner.beacons.Add(new Position(values[0], values[1], values[2]));
                }
            }
            scanners.Add(scanner);

            foreach (Scanner s in scanners)
            {
                s.GenerateFingerprint();
            }

            var scanner0 = AssembleScannerMap(scanners);

            // Part 1
            Console.WriteLine(PartOne(scanner0));

            // Part 2
            Console.WriteLine(PartTwo(scanner0));
        }

        static Scanner AssembleScannerMap(List<Scanner> scanners)
        {
            // Generate similarity matrix of scanners
            var scannerPairSimilarity = new PriorityQueue<Tuple<Scanner, Scanner>, int>();
            for (int i = 0; i < scanners.Count - 1; i++)
            {
                for (int j = i + 1; j < scanners.Count; j++)
                {
                    // Using "500 - similarity" because PQ sort by min to max
                    var similarity = 500 - scanners[i].SimilarityTo(scanners[j]);
                    scannerPairSimilarity.Enqueue(Tuple.Create(scanners[i], scanners[j]), similarity);
                }
            }

            // Iterate until no scanner pairs remain
            while (scannerPairSimilarity.TryDequeue(out Tuple<Scanner, Scanner> pair, out int similarity))
            {
                // Extract next most similar scanners from the PQ
                var scannerA = pair.Item1;
                var scannerB = pair.Item2;
                bool foundMatch = false;

                // Check both scanners still exist in the list
                if (!scanners.Contains(scannerA) || !scanners.Contains(scannerB))
                {
                    continue;
                }

                // Verify the two scanners still have the same similarity
                var newSimilarity = 500 - scannerA.SimilarityTo(scannerB);
                if (similarity != newSimilarity)
                {
                    // Add back into PQ with updated similarity score
                    scannerPairSimilarity.Enqueue(Tuple.Create(scannerA, scannerB), newSimilarity);
                    continue;
                }

                // Find any pair of matching beacons that are in both scanners
                var beaconPairs = scannerA.FindMatchingBeacons(scannerB);
                if (beaconPairs.Count >= 66)
                {
                    // For each possible pair of matching beacons
                    foreach (var beaconPair in beaconPairs)
                    {
                        // If a match was found on previous look, exit
                        if (foundMatch) { break; }

                        // Extract indexes of beacon pairs
                        var sA1 = beaconPair.Item1.Item1;
                        var sA2 = beaconPair.Item1.Item2;
                        var sB1 = beaconPair.Item2.Item1;
                        var sB2 = beaconPair.Item2.Item2;

                        // Select either scanner to make variations as long as its not scanner 0
                        Scanner fixedScanner;
                        Scanner variationsScanner;
                        Tuple<Position, Position> fixedBeacons;
                        Tuple<int, int> variationsBeacons;

                        // If scanner A is the fixed scanner
                        if (scannerA.index == 0)
                        {
                            fixedScanner = scannerA;
                            variationsScanner = scannerB;
                            fixedBeacons = Tuple.Create(scannerA.beacons[sA1], scannerA.beacons[sA2]);
                            variationsBeacons = Tuple.Create(sB1, sB2);
                        }
                        // Else scanner B is the fixed scanner
                        else
                        {
                            fixedScanner = scannerB;
                            variationsScanner = scannerA;
                            fixedBeacons = Tuple.Create(scannerB.beacons[sB1], scannerB.beacons[sB2]);
                            variationsBeacons = Tuple.Create(sA1, sA2);
                        }

                        // Generate all orientations and translations until match is found
                        foreach (var variation in variationsScanner.GenerateVariations(fixedBeacons, variationsBeacons))
                        {
                            // If a match was found on previous look, exit
                            if (foundMatch) { break; }

                            // If number of matches is greater than 12, we have a matching scanner
                            var matches = fixedScanner.CountMatches(variation);
                            if (matches >= Math.Min(12, fixedScanner.beacons.Count))
                            {
                                // Assign all beacons to the fixed scanner
                                foreach (var beacon in variation.beacons)
                                {
                                    if (!fixedScanner.beacons.Contains(beacon))
                                    {
                                        fixedScanner.beacons.Add(beacon);
                                    }
                                }

                                // Tell fixed scanner where the variations scanner was
                                // as well as all scanners they knew the positions of
                                fixedScanner.scanners.Add(variation.position);
                                fixedScanner.scanners.AddRange(variation.scanners);

                                // Generate new fingerprint for the fixed scanner
                                fixedScanner.GenerateFingerprint();

                                // Remove variations scanner from list
                                scanners.Remove(variationsScanner);

                                // Generate similarity scores with every other scanner
                                foreach (var otherScanner in scanners)
                                {
                                    if (!fixedScanner.Equals(otherScanner))
                                    {
                                        // Using "500 - similarity" because PQ sort by min to max
                                        var updatedSimilarity = 500 - fixedScanner.SimilarityTo(otherScanner);
                                        scannerPairSimilarity.Enqueue(Tuple.Create(fixedScanner, otherScanner), updatedSimilarity);
                                    }
                                }

                                // Exit for loops
                                foundMatch = true;
                            }
                        }
                    }
                }
            }

            // Should be one scanner remaining
            return scanners[0];
        }

        static string PartOne(Scanner scanner)
        {
            // Return number of beacons in the only scanner remaining
            return scanner.beacons.Count.ToString();
        }

        static string PartTwo(Scanner scanner)
        {
            // Add all scanner locations into single list
            var locations = new List<Position>() { scanner.position };
            locations.AddRange(scanner.scanners);

            // Calculate manhatten distance for each pair
            var largestManhatten = 0;

            for (int i = 0; i < locations.Count - 1; i++)
            {
                for (int j = i; j < locations.Count; j++)
                {
                    var manhatten = locations[i].ManhattenTo(locations[j]);
                    if (manhatten > largestManhatten)
                    {
                        largestManhatten = manhatten;
                    }
                }
            }

            return largestManhatten.ToString();
        }
    }

    class Position
    {
        public int x;
        public int y;
        public int z;

        public Position(int X, int Y, int Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public bool Equals(Position other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return x == other.x && 
                y == other.y && 
                z == other.z;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Position);
        }

        public override string ToString()
        {
            return x + ", " + y + ", " + z;
        }

        public Position Add(Position o)
        {
            return new Position(
                x + o.x,
                y + o.y,
                z + o.z
            );
        }

        public Position Subtract(Position o)
        {
            return new Position(
                x - o.x,
                y - o.y,
                z - o.z
            );
        }

        public double DistanceTo(Position o)
        {
            return Math.Sqrt(
                Math.Pow(Math.Abs(x - o.x), 2) +
                Math.Pow(Math.Abs(y - o.y), 2) +
                Math.Pow(Math.Abs(z - o.z), 2)
            );
        }

        public int ManhattenTo(Position o)
        {
            return Math.Abs(x - o.x) + 
                Math.Abs(y - o.y) + 
                Math.Abs(z - o.z);
        }
    }

    class Scanner
    {
        public int index;
        public Position position;
        public List<Position> beacons;
        public List<Position> scanners;
        public Dictionary<double, Tuple<int, int>> fingerprint;

        public Scanner(int i = 0)
        {
            index = i;
            position = new Position(0, 0, 0);
            beacons = new List<Position>();
            scanners = new List<Position>();
            fingerprint = new Dictionary<double, Tuple<int, int>>();
        }

        public bool Equals(Scanner other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return index == other.index;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Scanner);
        }

        public override string ToString()
        {
            return index + ", Count=" + beacons.Count;
        }

        public void GenerateFingerprint()
        {
            fingerprint = new Dictionary<double, Tuple<int, int>>();

            for (int i = 0; i < beacons.Count - 1; i++)
            {
                for (int j = i + 1; j < beacons.Count; j++)
                {
                    var d = beacons[i].DistanceTo(beacons[j]);

                    if (fingerprint.ContainsKey(d)) { 
                        continue; 
                    }

                    fingerprint.Add(d, Tuple.Create(i, j));
                }
            }
        }

        public int SimilarityTo(Scanner o)
        {
            int count = 0;

            foreach (var distance in fingerprint.Keys)
            {
                if (o.fingerprint.ContainsKey(distance))
                {
                    count += 1;
                }
            }

            return count;
        }

        public List<Tuple<Tuple<int, int>, Tuple<int, int>>> FindMatchingBeacons(Scanner o)
        {
            var matchingBeacons = new List<Tuple<Tuple<int, int>, Tuple<int, int>>>();

            foreach(var distance in fingerprint.Keys)
            {
                if (o.fingerprint.ContainsKey(distance))
                {
                    matchingBeacons.Add(
                        Tuple.Create(
                            fingerprint[distance],
                            o.fingerprint[distance]
                        )
                    );
                }
            }

            return matchingBeacons;
        }

        public List<Scanner> GenerateVariations(Tuple<Position, Position> fixedBeacons, Tuple<int, int> variationBeacons)
        {
            // Create and initialise orientation variation scanners
            var oriVariations = new List<Scanner>();
            for (int i = 0; i < 24; i++)
            {
                oriVariations.Add(new Scanner(index));
            }

            // For each beacon in this scanner
            foreach (var beacon in beacons)
            {
                var x = beacon.x;
                var y = beacon.y;
                var z = beacon.z;

                // Apply orientations
                oriVariations[0].beacons.Add(new Position(x, y, z));
                oriVariations[1].beacons.Add(new Position(-y, x, z));
                oriVariations[2].beacons.Add(new Position(-x, -y, z));
                oriVariations[3].beacons.Add(new Position(y, -x, z));
                oriVariations[4].beacons.Add(new Position(-z, y, x));
                oriVariations[5].beacons.Add(new Position(-y, -z, x));
                oriVariations[6].beacons.Add(new Position(z, -y, x));
                oriVariations[7].beacons.Add(new Position(y, z, x));
                oriVariations[8].beacons.Add(new Position(-x, y, -z));
                oriVariations[9].beacons.Add(new Position(-y, -x, -z));
                oriVariations[10].beacons.Add(new Position(x, -y, -z));
                oriVariations[11].beacons.Add(new Position(y, x, -z));
                oriVariations[12].beacons.Add(new Position(z, y, -x));
                oriVariations[13].beacons.Add(new Position(-y, z, -x));
                oriVariations[14].beacons.Add(new Position(-z, -y, -x));
                oriVariations[15].beacons.Add(new Position(y, -z, -x));
                oriVariations[16].beacons.Add(new Position(x, -z, y));
                oriVariations[17].beacons.Add(new Position(z, x, y));
                oriVariations[18].beacons.Add(new Position(-x, z, y));
                oriVariations[19].beacons.Add(new Position(-z, -x, y));
                oriVariations[20].beacons.Add(new Position(x, z, -y));
                oriVariations[21].beacons.Add(new Position(-z, x, -y));
                oriVariations[22].beacons.Add(new Position(-x, -z, -y));
                oriVariations[23].beacons.Add(new Position(z, -x, -y));
            }

            // Create transition variations
            var variations = new List<Scanner>();

            // For each orientation scanner
            foreach (var orientation in oriVariations)
            {
                // Calculate the translation required to move to fixed beacon positions
                var beaconA1 = fixedBeacons.Item1;
                var beaconA2 = fixedBeacons.Item2;
                var beaconB1 = orientation.beacons[variationBeacons.Item1];
                var beaconB2 = orientation.beacons[variationBeacons.Item2];

                // Generate all translations
                var translations = new List<Position>();
                translations.Add(beaconA1.Subtract(beaconB1));
                translations.Add(beaconA2.Subtract(beaconB2));
                translations.Add(beaconB1.Subtract(beaconA1));
                translations.Add(beaconB2.Subtract(beaconA2));

                // For each translation
                foreach (var translation in translations)
                {
                    // Set position of translation
                    var traVariation = new Scanner(index);
                    traVariation.position = translation;

                    // Translate all beacons
                    traVariation.beacons = orientation.beacons.Select(b => b.Add(translation)).ToList();

                    // Add to variations list
                    variations.Add(traVariation);
                }
            }

            return variations;
        }

        public int CountMatches(Scanner o)
        {
            return beacons.FindAll(b => o.beacons.Contains(b)).Count;
        }
    }

}
