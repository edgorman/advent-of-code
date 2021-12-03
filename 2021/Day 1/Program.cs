using System.Reflection;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            int[] input = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                input[i] = int.Parse(lines[i]);
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(int[] input)
        {
            int numberOfIncreases = 0;

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] > input[i - 1])
                {
                    numberOfIncreases++;
                }
            }

            return numberOfIncreases.ToString();
        }

        static string PartTwo(int[] input)
        {
            int numberOfWindowIncreases = 0;

            for (int i = 1; i < input.Length; i++)
            {
                int prevWindow = input.Skip(i - 1).Take(3).Sum();
                int nextWindow = input.Skip(i).Take(3).Sum();

                if (nextWindow > prevWindow)
                {
                    numberOfWindowIncreases++;
                }
            }

            return numberOfWindowIncreases.ToString();
        }
    }
}