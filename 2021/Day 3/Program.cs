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
            int[][] input = new int[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                char[] line = lines[i].ToCharArray();
                input[i] = line.Select(a => a - '0').ToArray();
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(int[][] input)
        {
            // Note to future Ed: 
            // Working with bits in c# is hard, don't do

            string gammaString = "";
            string epsilonString = "";

            for (int x = 0; x < input[0].Length; x++)
            {
                int count = 0;

                for (int y = 0; y < input.Length; y++)
                {
                    if (input[y][x] == 1)
                    {
                        count++;
                    }
                }

                if (count >= Math.Ceiling(input.Length / 2f))
                {
                    gammaString += "1";
                    epsilonString += "0";
                }
                else
                {
                    gammaString += "0";
                    epsilonString += "1";
                }
            }

            uint gammaValue = Convert.ToUInt32(gammaString, 2);
            uint epsilonValue = Convert.ToUInt32(epsilonString, 2);
            uint powerConsumption = gammaValue * epsilonValue;

            return powerConsumption.ToString();
        }

        static string PartTwo(int[][] input)
        {
            uint oxygenValue = PartTwoHelper(input, true, 0);
            uint c02scrubberValue = PartTwoHelper(input, false, 0);
            uint lifesupportValue = oxygenValue * c02scrubberValue;

            return lifesupportValue.ToString();
        }

        static uint PartTwoHelper(int[][] input, bool mostCommon, int index)
        {
            // Base case: There is only one input left, return value as uint
            if (input.Length == 1)
            {
                return Convert.ToUInt32(string.Join("", input[0]), 2);
            }

            // Store ints in separate lists on whether t5heir index position contains a 1 or 0
            List<List<int>> oneList = new List<List<int>>();
            List<List<int>> zeroList = new List<List<int>>();

            for (int y = 0; y < input.Length; y++)
            {
                if (input[y][index] == 1)
                {
                    oneList.Add(input[y].ToList());
                }
                else
                {
                    zeroList.Add(input[y].ToList());
                }
            }

            // Convert int lists back into arrays
            int[][] oneArray = oneList.Select(list => list.ToArray()).ToArray();
            int[][] zeroArray = zeroList.Select(list => list.ToArray()).ToArray();

            // Recurse cases:
            // If should return most common bit
            if (mostCommon)
            {
                if (oneArray.Length >= zeroArray.Length)
                {
                    return PartTwoHelper(oneArray, mostCommon, index + 1);
                }
                else
                {
                    return PartTwoHelper(zeroArray, mostCommon, index + 1);
                }
            }
            // Else should return least common bit
            else
            {
                if (oneArray.Length >= zeroArray.Length)
                {
                    return PartTwoHelper(zeroArray, mostCommon, index + 1);
                }
                else
                {
                    return PartTwoHelper(oneArray, mostCommon, index + 1);
                }
            }

        }
    }
}