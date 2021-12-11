
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
                input[i] = Array.ConvertAll(line, c => (int)char.GetNumericValue(c));
            }

            var inputCopy1 = input.Select(i => i.ToArray()).ToArray();
            var inputCopy2 = input.Select(i => i.ToArray()).ToArray();

            // Part 1
            Console.WriteLine(PartOne(inputCopy1));

            // Part 2
            Console.WriteLine(PartTwo(inputCopy2));
        }

        static string PartOne(int[][] octopi)
        {
            int flashCount = 0;

            // For each step
            for (int step = 0; step < 100; step++)
            {
                // Increase energy levels by 1
                AddEnergy(octopi, 1);

                // Determine if a flash happened
                Tuple<int, int> flashPos = DetectFlash(octopi);
                Tuple<int, int> noFlash = new Tuple<int, int>(-1, -1);

                // While octopi are about to flash
                while (!flashPos.Equals(noFlash))
                {
                    // Count number of flashes
                    Flash(octopi, flashPos, 1);

                    // Detect next flash
                    flashPos = DetectFlash(octopi);
                }

                // Count number of octopi flashing
                flashCount += CountFlashes(octopi);
            }

            return flashCount.ToString();
        }

        static void AddEnergy(int[][] octopi, int value)
        {
            for (int y = 0; y < octopi.Length; y++)
            {
                for (int x = 0; x < octopi[y].Length; x++)
                {
                    octopi[y][x] += value;
                }
            }
        }
        
        static Tuple<int, int> DetectFlash(int[][] octopi)
        {
            for (int y = 0; y < octopi.Length; y++)
            {
                for (int x = 0; x < octopi[y].Length; x++)
                {
                    if (octopi[y][x] == 10)
                    {
                        return new Tuple<int, int>(x, y);
                    }
                }
            }

            return new Tuple<int, int>(-1, -1);
        }

        static void Flash(int[][] octopi, Tuple<int, int> pos, int value)
        {
            int xa = Math.Max(0, pos.Item1 - 1);
            int xb = Math.Min(octopi.Length - 1, pos.Item1 + 1);
            int ya = Math.Max(0, pos.Item2 - 1);
            int yb = Math.Min(octopi[pos.Item1].Length - 1, pos.Item2 + 1);

            for (int xx = xa; xx <= xb; xx++)
            {
                for (int yy = ya; yy <= yb; yy++)
                {
                    if (octopi[yy][xx] < 10)
                    {
                        octopi[yy][xx] += value;
                    }
                }
            }

            octopi[pos.Item2][pos.Item1] += value;
        }

        static int CountFlashes(int[][] octopi)
        {
            int flashCount = 0;

            for (int y = 0; y < octopi.Length; y++)
            {
                for (int x = 0; x < octopi[y].Length; x++)
                {
                    if (octopi[y][x] >= 10)
                    {
                        flashCount++;
                        octopi[y][x] = 0;
                    }
                }
            }

            return flashCount;
        }

        static string PartTwo(int[][] octopi)
        {
            int stepCount = 0;

            // For each step
            while (true)
            {
                // Increase energy levels by 1
                AddEnergy(octopi, 1);
                stepCount += 1;

                // Determine if a flash happened
                Tuple<int, int> flashPos = DetectFlash(octopi);
                Tuple<int, int> noFlash = new Tuple<int, int>(-1, -1);

                // While octopi are about to flash
                while (!flashPos.Equals(noFlash))
                {
                    // Count number of flashes
                    Flash(octopi, flashPos, 1);

                    // Detect next flash
                    flashPos = DetectFlash(octopi);
                }

                // Count number of octopi flashing
                int flashCount = CountFlashes(octopi);

                // Check if all octopi are flashing
                if (flashCount == octopi.Length * octopi[0].Length)
                {
                    return stepCount.ToString();
                }
            }
        }
    }
}