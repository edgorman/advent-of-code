
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");

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
            return "";
        }

        static string PartTwo(int[] input)
        {
            return "";
        }
    }
}