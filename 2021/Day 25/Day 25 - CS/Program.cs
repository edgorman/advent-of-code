
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Extract east and south facing cucumbers from seafloor
            var seafloorShape = Tuple.Create(lines[0].Length, lines.Length);
            var eastCucumbers = new HashSet<Tuple<int, int>>();
            var southCucumbers = new HashSet<Tuple<int, int>>();
            for (int y = 0; y < seafloorShape.Item2; y++)
            {
                for (int x = 0; x < seafloorShape.Item1; x++)
                {
                    char value = lines[y][x];

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

            // Part 1
            Console.WriteLine(PartOne(eastCucumbers, southCucumbers, seafloorShape));

            // Part 2
            Console.WriteLine(PartTwo(eastCucumbers, southCucumbers, seafloorShape));
        }

        static string PartOne(
            HashSet<Tuple<int, int>> eastCucumbers, 
            HashSet<Tuple<int, int>> southCucumbers,
            Tuple<int, int> seafloorShape
        )
        {
            // Keep count of current step and cucumbers moving this step
            var stepCount = 0;
            var hasMoved = true;

            // Iterate until no cucumbers have moved
            while (hasMoved)
            {
                // Set moved to false and initialise empty set of new north cucumbers
                hasMoved = false;
                var newEastCucumbers = new HashSet<Tuple<int, int>>();

                // Move east facing cucumbers first
                foreach (var c in eastCucumbers)
                {
                    // Calculate new position
                    var newX = (c.Item1 + 1) % seafloorShape.Item1;
                    var newY = c.Item2;
                    var newPosition = Tuple.Create(newX, newY);

                    // If there is no cucumber in the position ahead
                    if (!eastCucumbers.Contains(newPosition) && !southCucumbers.Contains(newPosition))
                    {
                        // Move cucumber to new position
                        newEastCucumbers.Add(newPosition);
                        hasMoved = true;
                    }
                    else
                    {
                        // Keep cucumber at same position
                        newEastCucumbers.Add(c);
                    }
                }

                // Update east cucumbers and initialise empty set of new south cucumbers
                eastCucumbers = newEastCucumbers;
                var newSouthCucumbers = new HashSet<Tuple<int, int>>();

                // Move south facing cucumbers second
                foreach (var c in southCucumbers)
                {
                    // Calculate new position
                    var newX = c.Item1;
                    var newY = (c.Item2 + 1) % seafloorShape.Item2;
                    var newPosition = Tuple.Create(newX, newY);

                    // If there is no cucumber in the position ahead
                    if (!newEastCucumbers.Contains(newPosition) && !southCucumbers.Contains(newPosition))
                    {
                        // Move cucumber to new position
                        newSouthCucumbers.Add(newPosition);
                        hasMoved = true;
                    }
                    else
                    {
                        // Keep cucumber at same position
                        newSouthCucumbers.Add(c);
                    }
                }

                // Update south cucumbers and increment step count
                southCucumbers = newSouthCucumbers;
                stepCount++;
            }

            return stepCount.ToString();
        }

        static string PartTwo(
            HashSet<Tuple<int, int>> eastCucumbers,
            HashSet<Tuple<int, int>> southCucumbers,
            Tuple<int, int> seafloorShape
        )
        {
            return "";
        }
    }
}

