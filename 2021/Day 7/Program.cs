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
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(int[] input)
        {
            // Median
            int alignedValue = GetMedian(input);

            int fuel = 0;
            foreach (int i in input)
            {
                fuel += Math.Abs(i - alignedValue);
            }

            return fuel.ToString();
        }

        static int GetMedian(int[] array)
        {
            Array.Sort(array);

            if (array.Length % 2 == 0)
            {
                int a = array[array.Length / 2];
                int b = array[(array.Length / 2) - 1];
                return (a + b) / 2;
            }
            else
            {
                return array[array.Length / 2];
            }
        }

        static string PartTwo(int[] input)
        {
            // Average
            int alignedValue = GetAverage(input);

            int fuel = 0;
            foreach (int i in input)
            {
                int diff = Math.Abs(i - alignedValue);
                double cost = 0.5 * diff * (diff + 1);
                fuel += (int) cost;
            }

            return fuel.ToString();
        }

        static int GetAverage(int[] array)
        {
            return (int) array.Average();
        }
    }
}