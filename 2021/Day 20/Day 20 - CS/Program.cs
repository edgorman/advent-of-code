
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            char[] algorithm = lines[0].ToCharArray();
            HashSet<Tuple<int, int>> lights = new HashSet<Tuple<int, int>>();

            for (int y = 2; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        lights.Add(Tuple.Create(x, y));
                    }
                }
            }

            // Part 1
            Console.WriteLine(PartOne(algorithm, lights));

            // Part 2
            Console.WriteLine(PartTwo(algorithm, lights));
        }

        static string PartOne(char[] algorithm, HashSet<Tuple<int, int>> lights)
        {
            // Enhance the image twice
            for (int i = 0; i < 2; i++)
            {
                var newLights = new HashSet<Tuple<int, int>>();

                // Find topmost, leftmost, rightmost, bottommost values
                var top = int.MaxValue;
                var right = int.MinValue;
                var bottom = int.MinValue;
                var left = int.MaxValue;
                foreach (var light in lights)
                {
                    if (light.Item2 < top) { top = light.Item2; }
                    if (light.Item1 > right) { right = light.Item1; }
                    if (light.Item2 > bottom) { bottom = light.Item2; }
                    if (light.Item1 < left) { left = light.Item1; }
                }

                // Iterate across every value in range
                for (int y = top - 1; y <= bottom + 1; y++)
                {
                    for (int x = left - 1; x <= right + 1; x++)
                    {
                        var bin = "";

                        // Read 9 values surrounding x, y
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            for (int k = x - 1; k <= x + 1; k++)
                            {
                                if (lights.Contains(Tuple.Create(k, j)))
                                {
                                    bin += "1";
                                }
                                else
                                {
                                    bin += "0";
                                }
                            }
                        }

                        // Convert binary to integer
                        var index = Convert.ToInt32(bin, 2);

                        // Read value from algorithm index
                        var value = algorithm[index];

                        // If value is a light, add to new lights
                        if (value == '#')
                        {
                            newLights.Add(Tuple.Create(x, y));
                        }
                    }
                }

                lights = newLights;
            }

            return lights.Count.ToString();
        }

        static string PartTwo(char[] algorithm, HashSet<Tuple<int, int>> lights)
        {
            return "";
        }
    }

}
