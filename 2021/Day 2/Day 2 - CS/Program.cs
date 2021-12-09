
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            Tuple<String, int>[] input = new Tuple<String, int>[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(' ');
                input[i] = new Tuple<String, int>(line[0], int.Parse(line[1]));
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(Tuple<String, int>[] input)
        {
            int horizontal = 0;
            int depth = 0;

            foreach(Tuple<String, int> i in input)
            {
                switch (i.Item1)
                {
                    case "forward":
                        horizontal += i.Item2;
                        break;
                    case "down":
                        depth += i.Item2;
                        break;
                    case "up":
                        depth -= i.Item2;
                        break;
                }
            }

            int finalPosition = horizontal * depth;

            return finalPosition.ToString();
        }

        static string PartTwo(Tuple<String, int>[] input)
        {
            int horizontal = 0;
            int depth = 0;
            int aim = 0;

            foreach (Tuple<String, int> i in input)
            {
                switch (i.Item1)
                {
                    case "forward":
                        horizontal += i.Item2;
                        depth += i.Item2 * aim;
                        break;
                    case "down":
                        aim += i.Item2;
                        break;
                    case "up":
                        aim -= i.Item2;
                        break;
                }
            }

            int finalPosition = horizontal * depth;

            return finalPosition.ToString();
        }
    }
}