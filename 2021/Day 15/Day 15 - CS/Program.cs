
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
            int risk = 0;
            var start = new Tuple<int, int>(0, 0);
            var dest  = new Tuple<int, int>(input[0].Length - 1, input.Length - 1);

            // Use dijkstra's algorithm to find shorted path to destination
            var path = Dijkstra(input, start, dest);

            // Backtrack path to get list of nodes
            var next = dest;
            while (!next.Equals(start))
            {
                risk += input[next.Item2][next.Item1];
                next = path[next];
            }

            return risk.ToString();
        }

        static Dictionary<Tuple<int, int>, Tuple<int, int>> Dijkstra(int[][] input, Tuple<int, int> source, Tuple<int, int> dest)
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

            dist[source] = 0;

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
                    return prev;
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

            return prev;
        }

        static string PartTwo(int[][] input)
        {
            return "";
        }
    }
}