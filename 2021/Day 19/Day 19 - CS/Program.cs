
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");

            // Clean input
            List<List<List<int>>> scanners = new List<List<List<int>>>();
            List<List<int>> scanner = new List<List<int>>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("--- scanner"))
                {
                    scanner = new List<List<int>>();
                }
                else if (lines[i].Length == 0)
                {
                    scanners.Add(scanner);
                }
                else
                {
                    scanner.Add(lines[i].Split(',').ToList().ConvertAll(int.Parse));
                }
            }
            scanners.Add(scanner);

            // Part 1
            Console.WriteLine(PartOne(scanners));

            // Part 2
            Console.WriteLine(PartTwo(scanners));
        }

        static string PartOne(List<List<List<int>>> scanners)
        {
            return "";
        }

        static List<List<int>> GetOrientations(List<List<int>> beacons)
        {
            List<List<int>> orientations = new List<List<>>
        }

        static string PartTwo(List<List<List<int>>> scanners)
        {
            return "";
        }
    }

}
