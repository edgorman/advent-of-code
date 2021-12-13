
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            int numberOfDots = Array.FindIndex(lines, x => x.Length == 0) + 1;

            HashSet<Tuple<int, int>> dots = new HashSet<Tuple<int, int>>();
            for (int i = 0; i < numberOfDots - 1; i++)
            {
                int[] dot = Array.ConvertAll(lines[i].Split(','), int.Parse);
                dots.Add(new Tuple<int, int>(dot[0], dot[1]));
            }

            int numberOfFolds = lines.Length - numberOfDots;

            Tuple<bool, int>[] folds = new Tuple<bool, int>[numberOfFolds];
            for (int i = 0; i < numberOfFolds; i++)
            {
                string[] fold = lines[numberOfDots + i].Substring(11, lines[numberOfDots + i].Length - 11).Split('=');
                folds[i] = new Tuple<bool, int>(fold[0] == "y", int.Parse(fold[1]));
            }

            // Part 1
            Console.WriteLine(PartOne(dots.Select(i => i).ToHashSet(), new Tuple<bool, int>[] { folds[0] }));

            // Part 2
            Console.WriteLine(PartTwo(dots.Select(i => i).ToHashSet(), folds.Select(i => i).ToArray()));
        }

        static string PartOne(HashSet<Tuple<int, int>> dots, Tuple<bool, int>[] folds)
        {
            // Fold dots using only first fold
            dots = FoldDots(dots, folds[0]);

            return dots.Count.ToString();
        }

        static HashSet<Tuple<int, int>> FoldDots(HashSet<Tuple<int, int>> dots, Tuple<bool, int> fold)
        {
            bool isVertical = fold.Item1;
            int foldValue = fold.Item2;
            HashSet<Tuple<int, int>> endDots = new HashSet<Tuple<int, int>>();

            // For each dot
            foreach (Tuple<int, int> dot in dots)
            {
                int x = dot.Item1;
                int y = dot.Item2;

                // If fold is in vertical direction
                if (isVertical)
                {
                    // Check dot's y is greater than value
                    if (y > foldValue)
                    {
                        // Move dot to other side 
                        y = foldValue - (dot.Item2 - foldValue);
                    }
                }
                // Else fold is in horizontal direction
                else
                {
                    // Check dot's y is greater than value
                    if (x > foldValue)
                    {
                        // Move dot to other side 
                        x = foldValue - (dot.Item1 - foldValue);
                    }
                }

                endDots.Add(new Tuple<int, int>(x, y));
            }

            return endDots;
        }

        static string PartTwo(HashSet<Tuple<int, int>> dots, Tuple<bool, int>[] folds)
        {
            // Fold dots using all folds
            foreach(Tuple<bool, int> fold in folds)
            {
                dots = FoldDots(dots, fold);
            }

            // Calculate max x and y dot
            int maxX = dots.Max(d => d.Item1);
            int maxY = dots.Max(d => d.Item2);

            // Print dots
            string line = "";
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    if (dots.Contains(new Tuple<int, int>(x, y)))
                    {
                        line += "#";
                    }
                    else
                    {
                        line += ".";
                    }
                }

                line += "\n";
            }

            return line;
        }
    }
}