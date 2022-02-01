
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Part 1
            Console.WriteLine(PartOne(lines));

            // Part 2
            Console.WriteLine(PartTwo(lines));

        }

        static string PartOne(string[] seafloor)
        {
            // Extract east and south facing cucumbers from seafloor
            var eastCucumbers = new HashSet<Tuple<int, int>>();
            var southCucumbers = new HashSet<Tuple<int, int>>();

            for (int y = 0; y < seafloor.Length; y++)
            {
                for (int x = 0; x < seafloor[y].Length; x++)
                {
                    char value = seafloor[y][x];

                    if (value.Equals('>'))
                    {
                        eastCucumbers.Add(Tuple.Create(x, y));
                    }
                    else if (value.Equals('v'))
                    {
                        southCucumbers.Add(Tuple.Create(x, y));
                    }
                }
            }

            // Keep count of current step and cucumbers moving this step
            var stepCount = 0;
            var movingCount = int.MaxValue;

            // Iterate until no cucumbers can move
            while (movingCount > 0)
            {
                //Console.WriteLine(stepCount);
                //for (int y = 0; y < seafloor.Length; y++)
                //{
                //    for (int x = 0; x < seafloor[y].Length; x++)
                //    {
                //        if (eastCucumbers.Contains(Tuple.Create(x, y)))
                //        {
                //            Console.Write('>');
                //        }
                //        else if (southCucumbers.Contains(Tuple.Create(x, y)))
                //        {
                //            Console.Write('v');
                //        }
                //        else
                //        {
                //            Console.Write('.');
                //        }
                //    }
                //    Console.WriteLine();
                //}
                //Console.WriteLine();

                movingCount = 0;
                var newEastCucumbers = new HashSet<Tuple<int, int>>();
                var newSouthCucumbers = new HashSet<Tuple<int, int>>();

                // Move east facing cucumbers first
                foreach (var c in eastCucumbers)
                {
                    // If there is no cucumber in the position ahead
                    var newPosition = Tuple.Create((c.Item1 + 1) % seafloor[0].Length, c.Item2);
                    if (!eastCucumbers.Contains(newPosition) && !southCucumbers.Contains(newPosition))
                    {
                        newEastCucumbers.Add(newPosition);
                        movingCount++;
                    }
                    else
                    {
                        newEastCucumbers.Add(c);
                    }
                }

                // Move east facing cucumbers first
                foreach (var c in southCucumbers)
                {
                    // If there is no cucumber in the position ahead
                    var newPosition = Tuple.Create(c.Item1, (c.Item2 + 1) % seafloor.Length);
                    if (!newEastCucumbers.Contains(newPosition) && !southCucumbers.Contains(newPosition))
                    {
                        newSouthCucumbers.Add(newPosition);
                        movingCount++;
                    }
                    else
                    {
                        newSouthCucumbers.Add(c);
                    }
                }

                eastCucumbers = newEastCucumbers;
                southCucumbers = newSouthCucumbers;
                stepCount++;
            }

            return stepCount.ToString();
        }

        static string PartTwo(string[] input)
        {
            return "";
        }
    }
}

