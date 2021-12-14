
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            string template = lines[0];

            Dictionary<Tuple<char, char>, char> polymerDict = new Dictionary<Tuple<char, char>, char>();
            for (int i = 2; i < lines.Length; i++)
            {
                polymerDict.Add(
                    new Tuple<char, char>(
                        lines[i][0],
                        lines[i][1]
                    ),
                    lines[i].Last()
                );
            }

            // Part 1
            Console.WriteLine(PartOne(template.ToString(), polymerDict, 10));

            // Part 2
            Console.WriteLine(PartTwo(template.ToString(), polymerDict, 40));
        }

        static string PartOne(string template, Dictionary<Tuple<char, char>, char> polymerDict, int steps)
        {
            // Iterate for n number of steps
            for (int n = 0; n < steps; n++)
            {
                string nextTemplate = template[0].ToString();

                // Iterate across every pair of characters
                for (int i = 0; i < template.Length - 1; i++)
                {
                    char a = template[i];
                    char b = template[i + 1];

                    // Check if dict contains key <a,b>
                    if (polymerDict.ContainsKey(new Tuple<char, char>(a, b)))
                    {
                        // Add element to template
                        nextTemplate += polymerDict[new Tuple<char, char>(a, b)];
                    }

                    // Add end of template
                    nextTemplate += b;
                }

                template = nextTemplate.ToString();
            }

            // Calculate difference between most and least occurences in template
            char maxLetter = template.GroupBy(c => c).OrderByDescending(c => c.Count()).First().Key;
            char minLetter = template.GroupBy(c => c).OrderByDescending(c => c.Count()).Last().Key;
            int maxValue = template.ToArray().Count(c => c == maxLetter);
            int minValue = template.ToArray().Count(c => c == minLetter);
            int difference = maxValue - minValue;

            return difference.ToString();
        }

        static string PartTwo(string template, Dictionary<Tuple<char, char>, char> polymerDict, int steps)
        {
            Dictionary<char, long> charDict = new Dictionary<char, long>();
            charDict.Add(template[0], 1);
            Dictionary<Tuple<char, char>, long> combDict = new Dictionary<Tuple<char, char>, long>();

            // Parse template into combinations dictionary
            // And count chars in template
            for (int i = 0; i < template.Length - 1; i++)
            {
                char a = template[i];
                char b = template[i + 1];

                charDict[b] = charDict.GetValueOrDefault(b, 0) + 1;
             
                var k = new Tuple<char, char>(a, b);
                combDict[k] = combDict.GetValueOrDefault(k, 0) + 1;
            }

            // Iterate for n number of steps
            for (int n = 0; n < steps; n++)
            {
                var newCombDict = new Dictionary<Tuple<char, char>, long>();

                // For each combination
                foreach((Tuple<char, char> pair, long count) in combDict)
                {
                    char a = pair.Item1;
                    char b = pair.Item2;
                    var k = new Tuple<char, char>(a, b);

                    // If dict contains key<a,b>
                    if (polymerDict.ContainsKey(k))
                    {
                        char c = polymerDict[k];
                        charDict[c] = charDict.GetValueOrDefault(c, 0) + count;

                        var l = new Tuple<char, char>(a, c);
                        var r = new Tuple<char, char>(c, b);

                        newCombDict[l] = newCombDict.GetValueOrDefault(l, 0) + count;
                        newCombDict[r] = newCombDict.GetValueOrDefault(r, 0) + count;
                    }
                }

                combDict = newCombDict;
            }

            // Calculate difference between most and least occurences in template
            long maxValue = charDict.Max(c => c.Value);
            long minValue = charDict.Min(c => c.Value);
            long difference = maxValue - minValue;

            return difference.ToString();
        }
    }
}