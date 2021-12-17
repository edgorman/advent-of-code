
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            string line = lines[0][13..(lines[0].Length)];
            lines = line.Split(", ");

            int[] x = lines[0][2..lines[0].Length].Split("..").Select(int.Parse).ToArray();
            int[] y = lines[1][2..lines[1].Length].Split("..").Select(int.Parse).ToArray();

            // Part 1
            Console.WriteLine(PartOne(x, y));

            // Part 2
            Console.WriteLine(PartTwo(x, y));
        }

        static string PartOne(int[] xRange, int[] yRange)
        {
            int maxY = 0;

            // Brute force x and y velocities
            for (int x = 1; x < xRange.Max(); x++)
            {
                for (int y = 1; y <= -yRange.Min() - 1; y++)
                {
                    // Fire probe and get result
                    var result = FireProbe(x, y, xRange, yRange);

                    if (result.Item2 && result.Item1 > maxY)
                    {
                        // Update max y
                        maxY = result.Item1;
                    }
                }
            }

            return maxY.ToString();
        }

        static Tuple<int, bool> FireProbe(int dx, int dy, int[] tx, int[] ty)
        {
            int x = 0;
            int y = 0;
            int maxY = y;

            // While probe will still hit target
            while (x <= tx.Max() && y >= ty.Min())
            {
                // Increment x and y by their velocities
                x += dx;
                y += dy;

                // Decrement x and y velocities
                if (dx > 0)
                {
                    dx -= 1;
                }
                else if (dx < 0)
                {
                    dx += 1;
                }
                dy -= 1;

                // Check if y is greater than max y
                if (y > maxY)
                {
                    maxY = y;
                }

                // Check if x and y are in bounds 
                if (x >= tx.Min() && x <= tx.Max() &&
                    y >= ty.Min() && y <= ty.Max())
                {
                    return Tuple.Create(maxY, true);
                }
            }

            // Return target miss
            return Tuple.Create(maxY, false);
        }

        static string PartTwo(int[] xRange, int[] yRange)
        {
            int hitCount = 0;

            // Brute force x and y velocities
            for (int x = 1; x <= xRange.Max(); x++)
            {
                for (int y = yRange.Min(); y <= -yRange.Min() - 1; y++)
                {
                    // Fire probe and get result
                    var result = FireProbe(x, y, xRange, yRange);
                    
                    // If hit, increment counter
                    if (result.Item2)
                    {
                        hitCount++;
                    }
                }
            }

            return hitCount.ToString();
        }
    }
}

