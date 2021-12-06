using System.Text.RegularExpressions;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            int[] input = Array.ConvertAll(lines[0].Split(','), Int32.Parse);

            // Part 1
            Console.WriteLine(PartOne(input, 80));

            input = Array.ConvertAll(lines[0].Split(','), Int32.Parse);

            // Part 2
            Console.WriteLine(PartTwo(input, 256));
        }

        static string PartOne(int[] input, int daysRemaining)
        {
            List<int> fish = input.ToList();

            while (daysRemaining > 0)
            {
                // Create empty list for new fish
                List<int> newFish = new List<int>();

                // For each fish
                foreach (int f in fish)
                {
                    // If it has reached end of cycle
                    if (f == 0)
                    {
                        // Reset cycle and add new fish
                        newFish.Add(6);
                        newFish.Add(8);
                    }
                    else
                    {
                        // Decrement fish value
                        newFish.Add(f - 1);
                    }
                }

                // Update fish list with new values
                fish = newFish;
                daysRemaining--;
            }

            // Size of the fish list is population
            return fish.Count.ToString();
        }

        static string PartTwo(int[] input, int daysRemaining)
        {
            // Store number of fish per age (0-9)
            long[] fishCounts = new long[9];

            for (int i = 0; i < fishCounts.Length; i++)
            {
                fishCounts[i] = input.Where(x => x == i).Count();
            }

            int cycleStep = 0;
            int cycleLength = 9;

            // Iterate until no days remaining
            while (daysRemaining > 0)
            {
                // Calculate index positions of old and new fish
                int oldFishIndex = cycleStep % cycleLength;
                int newFishIndex = (cycleStep + 7) % cycleLength;

                // Add new fish equal to number of old fish
                fishCounts[newFishIndex] += fishCounts[oldFishIndex];

                // Cycle index of fish lives
                cycleStep++;
                daysRemaining--;
            }

            // Sum of each age group is population
            return fishCounts.Sum().ToString();
        }
    }
}