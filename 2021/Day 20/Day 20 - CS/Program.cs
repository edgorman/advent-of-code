
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
            Console.WriteLine(PartOne(algorithm, lights, 2));

            // Part 2
            Console.WriteLine(PartOne(algorithm, lights, 50));
        }

        static string PartOne(char[] algorithm, HashSet<Tuple<int, int>> lights, int iterations)
        {
            // Calculate boundaries of lights
            var topmost = lights.MinBy(l => l.Item2).Item2;
            var rightmost = lights.MaxBy(l => l.Item1).Item1;
            var bottommost = lights.MaxBy(l => l.Item2).Item2;
            var leftmost = lights.MinBy(l => l.Item1).Item1;

            // Enhance the image twice
            for (int i = 0; i < iterations; i++)
            {
                // Infinite region surrounding lights will depend on the iteration
                // Initially they are all black, but after first iteration will be all light
                // This is not recorded in the new lights that are generated because
                // They would be outside the top/right/bottom/leftmost values
                var newLights = new HashSet<Tuple<int, int>>();

                // For each possible point of light (x, y)
                for (int y = topmost - 1; y <= bottommost + 1; y++)
                {
                    for (int x = leftmost - 1; x <= rightmost + 1; x++)
                    {
                        // Get surrounding pixels of point (j, k)
                        var binary = "";
                        for (int k = y - 1; k <= y + 1; k++)
                        {
                            for (int j = x - 1; j <= x + 1; j++)
                            {
                                if (algorithm[0] == '#' && i % 2 == 1 && 
                                    (k < topmost || k > bottommost || 
                                    j < leftmost || j > rightmost))
                                {
                                    binary += "1";
                                }
                                else if (lights.Contains(Tuple.Create(j, k)))
                                {
                                    binary += "1";
                                }
                                else
                                {
                                    binary += "0";
                                }
                            }
                        }

                        // Convert to index position
                        var index = Convert.ToInt32(binary, 2);
                        var value = algorithm[index];

                        // If algorithm is a point of light, add to new image
                        if (value == '#')
                        {
                            newLights.Add(Tuple.Create(x, y));
                        }
                    }
                }

                // Update lights with enhanced version
                lights = newLights;

                // Update boundary for which light/dark pixels will exist
                topmost--;
                rightmost++;
                bottommost++;
                leftmost--;
            }

            return lights.Count.ToString();
        }

    }

}
