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
            Tuple<int, int, int, int>[] input = new Tuple<int, int, int, int>[lines.Length];

            for (int i = 0; i < input.Length; i++)
            {
                string[] line = lines[i].Split(" -> ");
                int[] start = Array.ConvertAll(line[0].Split(","), Int32.Parse);
                int[] end = Array.ConvertAll(line[1].Split(","), Int32.Parse);

                input[i] = new Tuple<int, int, int, int>(start[0], start[1], end[0], end[1]);
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(Tuple<int, int, int, int>[] input)
        {
            Dictionary<(int, int), int> overlaps = new Dictionary<(int, int), int>();

            // For each line in input
            for (int i = 0; i < input.Length - 1; i++)
            {
                int aStartX = Math.Min(input[i].Item1, input[i].Item3);
                int aStartY = Math.Min(input[i].Item2, input[i].Item4);
                int aEndX = Math.Max(input[i].Item1, input[i].Item3);
                int aEndY = Math.Max(input[i].Item2, input[i].Item4);

                // Don't use diagonal lines
                if (aStartX != aEndX && aStartY != aEndY)
                {
                    continue;
                }

                // Compare against every following line
                for (int j = i + 1; j < input.Length; j++)
                {
                    int bStartX = Math.Min(input[j].Item1, input[j].Item3);
                    int bStartY = Math.Min(input[j].Item2, input[j].Item4);
                    int bEndX = Math.Max(input[j].Item1, input[j].Item3);
                    int bEndY = Math.Max(input[j].Item2, input[j].Item4);

                    // Don't use diagonal lines
                    if (bStartX != bEndX && bStartY != bEndY)
                    {
                        continue;
                    }

                    // Identify overlaps between a and b lines

                    // Check if they align in horizontal direction
                    if (aStartY == bStartY && aEndY == bEndY)
                    {
                        // For each position that overlaps
                        for (int x = Math.Max(aStartX, bStartX); x <= Math.Min(aEndX, bEndX); x++)
                        {
                            // Update overlaps dictionary
                            if (overlaps.ContainsKey((x, aStartY)))
                            {
                                overlaps[(x, aStartY)] += 1;
                            }
                            else
                            {
                                overlaps.Add((x, aStartY), 1);
                            }
                        }
                    }
                    
                    // Check if they align in vertical direction
                    if (aStartX == bStartX && aEndX == bEndX)
                    {
                        // For each position that overlaps
                        for (int y = Math.Max(aStartY, bStartY); y <= Math.Min(aEndY, bEndY); y++)
                        {
                            // Update overlaps dictionary
                            if (overlaps.ContainsKey((aStartX, y)))
                            {
                                overlaps[(aStartX, y)] += 1;
                            }
                            else
                            {
                                overlaps.Add((aStartX, y), 1);
                            }
                        }
                    }

                    // Calculate where the two lines would cross
                    int crossX = aStartX == aEndX ? aStartX : bStartX;
                    int crossY = aStartY == aEndY ? aStartY : bStartY;

                    // Check cross position is in both lines
                    if (aStartX <= crossX && crossX <= aEndX &&
                        aStartY <= crossY && crossY <= aEndY &&
                        bStartX <= crossX && crossX <= bEndX &&
                        bStartY <= crossY && crossY <= bEndY)
                    {
                        // Update overlaps dictionary
                        if (overlaps.ContainsKey((crossX, crossY)))
                        {
                            overlaps[(crossX, crossY)] += 1;
                        }
                        else
                        {
                            overlaps.Add((crossX, crossY), 1);
                        }
                    }
                }
            }
            
            return overlaps.Count().ToString();
        }

        static string PartTwo(Tuple<int, int, int, int>[] input)
        {
            // Had to go for the slower aproach for part two, may rework this
            Dictionary<(int, int), int> overlaps = new Dictionary<(int, int), int>();

            // For each line in input
            for (int i = 0; i < input.Length - 1; i++)
            {
                Console.WriteLine(i);
                Tuple<int, int>[] aCoords = calculateCoords(input[i]);

                // Compare against every following line
                for (int j = i + 1; j < input.Length; j++)
                {
                    Tuple<int, int>[] bCoords = calculateCoords(input[j]);

                    // Identify overlaps between a and b lines
                    for (int m = 0; m < aCoords.Length; m++)
                    {
                        for (int n = 0; n < bCoords.Length; n++)
                        {
                            // Check if coordinates match between lines
                            if (aCoords[m].Item1 == bCoords[n].Item1 &&
                                aCoords[m].Item2 == bCoords[n].Item2)
                            {
                                // Update overlaps dictionary
                                if (overlaps.ContainsKey((aCoords[m].Item1, aCoords[m].Item2)))
                                {
                                    overlaps[(aCoords[m].Item1, aCoords[m].Item2)] += 1;
                                }
                                else
                                {
                                    overlaps.Add((aCoords[m].Item1, aCoords[m].Item2), 1);
                                }
                            }
                        }
                    }
                    
                }
            }

            return overlaps.Count().ToString();
        }

        static Tuple<int, int>[] calculateCoords(Tuple<int, int, int, int> line)
        {

            int startX = line.Item1;
            int startY = line.Item2;
            int endX = line.Item3;
            int endY = line.Item4;

            int currX = startX;
            int currY = startY;
            int dX = 0;
            int dY = 0;

            if (startX != endX)
            {
                dX = startX < endX ? 1 : -1;
            }

            if (startY != endY)
            {
                dY = startY < endY ? 1 : -1;
            }

            List<Tuple<int, int>> coords = new List<Tuple<int, int>>();

            // Step through and add all coords in the line
            while (currX != endX + dX || currY != endY + dY)
            {
                coords.Add(new Tuple<int, int>(currX, currY));
                currX += dX;
                currY += dY;
            }

            return coords.ToArray();
        }
    }
}