
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");

            // Clean input
            Tuple<string, string>[] input = new Tuple<string, string>[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] connection = lines[i].Split('-');
                input[i] = new Tuple<string, string>(connection[0], connection[1]);
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(Tuple<string, string>[] connections)
        {
            // Create dictionary of connections
            Dictionary<string, List<string>> connectDict = CreateConnectionDict(connections);

            // Find paths that go start to end
            List<List<string>> routes = FindRoutes("start", new List<string>(), connectDict);

            return routes.Count.ToString();
        }

        static Dictionary<string, List<string>> CreateConnectionDict(Tuple<string, string>[] connections)
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            // For each connection listed in input
            foreach (Tuple<string, string> c in connections)
            {
                // If the key has been seen before
                if (dict.ContainsKey(c.Item1))
                {
                    dict[c.Item1].Add(c.Item2);
                }
                // Else the key is unique
                else
                {
                    dict.Add(c.Item1, new List<string> { c.Item2 });
                }

                // Repeat for value as key
                if (dict.ContainsKey(c.Item2))
                {
                    dict[c.Item2].Add(c.Item1);
                }
                // Else the key is unique
                else
                {
                    dict.Add(c.Item2, new List<string> { c.Item1 });
                }
            }

            return dict;
        }

        static List<List<string>> FindRoutes(string current, List<string> visited, Dictionary<string, List<string>> dict)
        {
            // Base case: Check if reached end
            if (current == "end")
            {
                return new List<List<string>>() { new List<string>() { "end" } };
            }

            // Get possible routes forward
            List<string> routes = dict[current].Select(s => s).ToList();

            // Remove small caves visited before
            routes.RemoveAll(r => visited.Contains(r));

            // Base case: Check if any routes remain
            if (routes.Count == 0)
            {
                return new List<List<string>>();
            }

            // Add small caves to visited list
            if (char.IsLower(current, 0))
            {
                visited.Add(current);
            }

            // Store list of routes that reach the end
            List<List<string>> endRoutes = new List<List<string>>();

            // Recurse: For each route, recurse
            foreach (string route in routes)
            {
                List<List<string>> possibleRoutes = FindRoutes(route, visited.Select(v => v).ToList(), dict);

                // If there are no routes forward
                if (possibleRoutes.Count == 0)
                {
                    continue;
                }

                // Filter routes that end with "end"
                possibleRoutes = possibleRoutes.Where(r => r.Last() == "end").ToList();

                // For each possible route forwards
                foreach (List<string> pRoute in possibleRoutes)
                {
                    pRoute.Insert(0, current);
                    endRoutes.Add(pRoute);
                }
            }

            return endRoutes;
        }

        static string PartTwo(Tuple<string, string>[] connections)
        {
            // Create dictionary of connections
            Dictionary<string, List<string>> connectDict = CreateConnectionDict(connections);

            // Find paths that go start to end
            List<List<string>> routes = FindRoutes2("start", new List<string>(), "", connectDict);

            return routes.Count.ToString();
        }

        static List<List<string>> FindRoutes2(string current, List<string> visited, string visitedTwice, Dictionary<string, List<string>> dict)
        {
            // Base case: Check if reached end
            if (current == "end")
            {
                return new List<List<string>>() { new List<string>() { "end" } };
            }

            // Get possible routes forward
            List<string> routes = dict[current].Select(s => s).ToList();

            // If a small caves has been visited twice, remove small caves
            if (visitedTwice != "")
            {
                routes.RemoveAll(r => visited.Contains(r));
            }

            // If routes contains "start" remove it
            if (routes.Contains("start"))
            {
                routes.Remove("start");
            }

            // Base case: Check if any routes remain
            if (routes.Count == 0)
            {
                return new List<List<string>>();
            }

            // Add small caves to visited list
            if (char.IsLower(current, 0))
            {
                // If current is already visited
                if (visited.Contains(current))
                {
                    visitedTwice = current;
                }
                // Else add current to visited
                else
                { 
                    visited.Add(current);
                }
            }

            // Store list of routes that reach the end
            List<List<string>> endRoutes = new List<List<string>>();

            // Recurse: For each route, recurse
            foreach (string route in routes)
            {
                List<List<string>> possibleRoutes = FindRoutes2(route, visited.Select(v => v).ToList(), visitedTwice, dict);

                // If there are no routes forward
                if (possibleRoutes.Count == 0)
                {
                    continue;
                }

                // Filter routes that end with "end"
                possibleRoutes = possibleRoutes.Where(r => r.Last() == "end").ToList();

                // For each possible route forwards
                foreach (List<string> pRoute in possibleRoutes)
                {
                    pRoute.Insert(0, current);
                    endRoutes.Add(pRoute);
                }
            }

            return endRoutes;
        }
    }
}