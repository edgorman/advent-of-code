
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
                char[] line = lines[i].ToCharArray();
                input[i] = Array.ConvertAll(line, c => (int)Char.GetNumericValue(c));
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(int[][] input)
        {
            int riskSum = 0;

            // Iterate across every input
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    // Get current positions value and that of neighbours
                    int value = input[y][x];
                    int[] neighbours = GetNeighbours(input, x, y);

                    // Get the list of neighbours higher than the current position
                    int[] higherNeighbours = neighbours.Where(n => n > value).ToArray();

                    // Check if this is a low point
                    if (neighbours.Length == higherNeighbours.Length)
                    {
                        // Calculate risk and add to sum risk
                        riskSum += 1 + value;
                    }
                }
            }

            return riskSum.ToString();
        }

        static int[] GetNeighbours(int[][] map, int x, int y)
        {
            List<int> neighbours = new List<int>();

            if (y > 0) { neighbours.Add(map[y - 1][x]); }
            if (y < map.Length - 1) { neighbours.Add(map[y + 1][x]); }
            if (x > 0) { neighbours.Add(map[y][x - 1]); }
            if (x < map[0].Length - 1) { neighbours.Add(map[y][x + 1]); }

            return neighbours.ToArray();
        }

        static string PartTwo(int[][] input)
        {
            List<Tuple<int, int>> lowPoints = new List<Tuple<int, int>>();

            // Iterate across every input
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    // Get current positions value and that of neighbours
                    int value = input[y][x];
                    int[] neighbours = GetNeighbours(input, x, y);

                    // Get the list of neighbours higher than the current position
                    int[] higherNeighbours = neighbours.Where(n => n > value).ToArray();

                    // Check if this is a low point
                    if (neighbours.Length == higherNeighbours.Length)
                    {
                        // Add to list of low points
                        lowPoints.Add(new Tuple<int, int>(x, y));
                    }
                }
            }

            List<int> basinSizes = new List<int>();

            // For each low point, calculate the basin size
            foreach (Tuple<int, int> pos in lowPoints)
            {
                int basinSize = GetBasinSize(input, pos, new List<Tuple<int, int>>());
                basinSizes.Add(basinSize);
            }

            // Find the three largest basins
            List<int> largestBasins = basinSizes.OrderByDescending(n => n).Take(3).ToList();

            // Calculate them multiplied together
            int largestBasinsProds = largestBasins.Aggregate((a, b) => a * b);

            return largestBasinsProds.ToString();
        }

        static int GetBasinSize(int[][] map, Tuple<int, int> pos, List<Tuple<int, int>> visited)
        {
            int x = pos.Item1;
            int y = pos.Item2;

            // Base case: If reached basin boundary, return
            if (map[y][x] == 9)
            {
                return 0;
            }

            // Base case: If position has already been visited, return
            if (visited.Contains(pos))
            {
                return 0;
            }

            // Recurse case: Get all neighbours and recurse, return 1 + their values
            List<Tuple<int, int>> neighbours = new List<Tuple<int, int>>();

            if (y > 0) { neighbours.Add(new Tuple<int, int>(x, y - 1)); }
            if (y < map.Length - 1) { neighbours.Add(new Tuple<int, int>(x, y + 1)); }
            if (x > 0) { neighbours.Add(new Tuple<int, int>(x - 1, y)); }
            if (x < map[0].Length - 1) { neighbours.Add(new Tuple<int, int>(x + 1, y)); }

            int size = 1;
            visited.Add(pos);

            foreach (Tuple<int, int> neighbour in neighbours)
            {
                size += GetBasinSize(map, neighbour, visited);
            }

            return size;
        }
    }
}