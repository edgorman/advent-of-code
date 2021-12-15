
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            int[][] input = new int[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                input[i] = lines[i].Select(n => n - '0').ToArray();
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(int[][] input)
        {
            // Set start and destination positions
            var start = new Tuple<int, int>(0, 0);
            var dest  = new Tuple<int, int>(input[0].Length - 1, input.Length - 1);

            // Find shortest path to destination
            var risk = AStar(input, start, dest);
            return risk.ToString();
        }

        static int Dijkstra(int[][] input, Tuple<int, int> start, Tuple<int, int> dest)
        {
            HashSet<Tuple<int, int>> nodes = new HashSet<Tuple<int, int>>();
            Dictionary<Tuple<int, int>, int> dist = new Dictionary<Tuple<int, int>,int>();
            Dictionary<Tuple<int, int>, Tuple<int, int>> prev = new Dictionary<Tuple<int, int>, Tuple<int, int>> ();

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    Tuple<int, int> node = new Tuple<int, int>(x, y);
                    
                    nodes.Add(node);
                    dist.Add(node, int.MaxValue);
                    prev.Add(node, null);
                }
            }

            dist[start] = 0;

            while (nodes.Count > 0)
            {
                Tuple<int, int> u = new Tuple<int, int>(0, 0);

                // Find next node with lowest distance
                // that hasn't already been seen
                foreach (var pair in dist.OrderBy(d => d.Value))
                {
                    u = pair.Key;

                    if (nodes.Contains(u))
                    {
                        nodes.Remove(u);
                        break;
                    }
                }

                // Check if u is target, if so end
                if (dest.Equals(u))
                {
                    break;
                }

                // Find neighbours around the node
                // Up, Right, Down, Left
                HashSet<Tuple<int, int>> neighbours = new HashSet<Tuple<int, int>>();
                neighbours.Add(new Tuple<int, int>(u.Item1, Math.Max(0, u.Item2 - 1)));
                neighbours.Add(new Tuple<int, int>(Math.Min(input[0].Length - 1, u.Item1 + 1), u.Item2));
                neighbours.Add(new Tuple<int, int>(u.Item1, Math.Min(input.Length - 1, u.Item2 + 1)));
                neighbours.Add(new Tuple<int, int>(Math.Max(0, u.Item1 - 1), u.Item2));
                neighbours.Remove(u);

                // For each neighbour, 
                // that hasn't already been seen
                foreach (var v in neighbours)
                {
                    // Get distance value for this neighbour
                    int alt = dist[u] + input[v.Item2][v.Item1];
                    
                    // If distance is less than the current distance for neighbour
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            // Calculate risk by backtracking
            int risk = 0;
            var next = dest;
            while (!next.Equals(start))
            {
                risk += input[next.Item2][next.Item1];
                next = prev[next];
            }

            return risk;
        }

        static int AStar(int[][] input, Tuple<int, int> source, Tuple<int, int> dest)
        {
            var candidates = new SortedSet<(int risk, int manhattan, Tuple<int, int> position)>();
            candidates.Add((0, 0, Tuple.Create(0, 0)));

            var visited = new HashSet<Tuple<int, int>>();
            visited.Add(Tuple.Create(0, 0));

            while (candidates.Count > 0)
            {
                var next = candidates.First();
                candidates.Remove(next);

                var neighbours = new HashSet<Tuple<int, int>>();
                neighbours.Add(new Tuple<int, int>(next.position.Item1, Math.Max(0, next.position.Item2 - 1)));
                neighbours.Add(new Tuple<int, int>(Math.Min(input[0].Length - 1, next.position.Item1 + 1), next.position.Item2));
                neighbours.Add(new Tuple<int, int>(next.position.Item1, Math.Min(input.Length - 1, next.position.Item2 + 1)));
                neighbours.Add(new Tuple<int, int>(Math.Max(0, next.position.Item1 - 1), next.position.Item2));
                neighbours.Remove(next.position);

                foreach(var neighbor in neighbours)
                {
                    if (neighbor.Equals(dest))
                    {
                        return next.risk + input[dest.Item2][dest.Item1];
                    }

                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        candidates.Add(
                            (
                                next.risk + input[neighbor.Item2][neighbor.Item1],
                                (input.Length - 1 - neighbor.Item2) + (input[0].Length - 1 - neighbor.Item1),
                                neighbor
                            )
                        );
                    }
                }
            }

            return -1;
        }

        static string PartTwo(int[][] input)
        {
            // Generate new input with repeated 5x5 structure
            var newInput = new int[input.Length * 5][];

            for (int y = 0; y < newInput.Length; y++)
            {
                newInput[y] = new int[input[0].Length * 5];

                for (int x = 0; x < newInput.Length; x++)
                {
                    var value = input[y % input.Length][x % input[0].Length] + (y / input.Length) + (x / input[0].Length);

                    while (value > 9)
                    {
                        value -= 9;
                    }

                    newInput[y][x] = value;
                }
            }

            // Set start and destination positions
            var start = new Tuple<int, int>(0, 0);
            var dest = new Tuple<int, int>(newInput[0].Length - 1, newInput.Length - 1);

            // Find shortest path to destination
            var risk = AStar(newInput, start, dest);
            return risk.ToString();
        }
    }
}