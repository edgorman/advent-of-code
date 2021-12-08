
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            Tuple<string[], string[]>[] input = new Tuple<string[], string[]>[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(" | ");
                string[] signals = line[0].Split(" ");
                string[] digits = line[1].Split(" ");

                input[i] = new Tuple<string[], string[]>(signals, digits);
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(Tuple<string[], string[]>[] input)
        {
            int digitCount = 0;

            for (int i = 0; i < input.Length; i++)
            {
                string[] signals = input[i].Item1;
                string[] digits = input[i].Item2;

                // Find 1, 4, 7, 8 from signal lengths
                string one = signals.Where(x => x.Length == 2).First();
                string four = signals.Where(x => x.Length == 4).First();
                string seven = signals.Where(x => x.Length == 3).First();
                string eight = signals.Where(x => x.Length == 7).First();

                // Sort segments by alphabetical order
                one = string.Concat(one.OrderBy(c => c));
                four = string.Concat(four.OrderBy(c => c));
                seven = string.Concat(seven.OrderBy(c => c));
                eight = string.Concat(eight.OrderBy(c => c));

                // For each digit, count number of 1, 4, 7, 8
                foreach (string digit in digits)
                {
                    string sortedDigit = string.Concat(digit.OrderBy(c => c));
                    
                    if (sortedDigit == one ||
                        sortedDigit == four ||
                        sortedDigit == seven ||
                        sortedDigit == eight)
                    {
                        digitCount++;
                    }
                }
            }

            return digitCount.ToString();
        }

        static string PartTwo(Tuple<string[], string[]>[] input)
        {
            int digitSum = 0;

            for (int i = 0; i < input.Length; i++)
            {
                string[] signals = input[i].Item1;
                string[] digits = input[i].Item2;

                // Find 1, 4, 7, 8 from signal lengths
                string one = signals.Where(x => x.Length == 2).First();
                string four = signals.Where(x => x.Length == 4).First();
                string seven = signals.Where(x => x.Length == 3).First();
                string eight = signals.Where(x => x.Length == 7).First();

                // Six will have a length of 6 and not contain the characters in one
                string six = signals.Where(x => x.Length == 6).First(x => !StringContainsString(x, one));
                // Nine will have a length of 6 and contain the characters in four
                string nine = signals.Where(x => x.Length == 6).First(x => StringContainsString(x, four));
                // Zero will have a length of 6 and be the remaining unidentified signal
                string zero = signals.Where(x => x.Length == 6).First(x => x != six && x != nine);
                // Three will have a length of 5 and contain the characters in one
                string three = signals.Where(x => x.Length == 5).First(x => StringContainsString(x, one));
                // Five will have a length of 5 and have more characters in common with four than two
                int mostSimilarToFour = signals.Where(x => x.Length == 5 && x != three).Max(x => CharactersInCommon(x, four));
                string five = signals.Where(x => x.Length == 5 && x != three).First(x => CharactersInCommon(x, four) == mostSimilarToFour);
                // Two will have a length of 5 and be the remaining unidentified signal
                string two = signals.Where(x => x.Length == 5).First(x => x != three && x != five);

                // Sort segments by alphabetical order
                zero = string.Concat(zero.OrderBy(c => c));
                one = string.Concat(one.OrderBy(c => c));
                two = string.Concat(two.OrderBy(c => c));
                three = string.Concat(three.OrderBy(c => c));
                four = string.Concat(four.OrderBy(c => c));
                five = string.Concat(five.OrderBy(c => c));
                six = string.Concat(six.OrderBy(c => c));
                seven = string.Concat(seven.OrderBy(c => c));
                eight = string.Concat(eight.OrderBy(c => c));
                nine = string.Concat(nine.OrderBy(c => c));

                // For each digit, decode and add to string
                string digitString = "";
                foreach (string digit in digits)
                {
                    string sortedDigit = string.Concat(digit.OrderBy(c => c));

                    if (sortedDigit == zero) { digitString += "0"; }
                    else if (sortedDigit == one) { digitString += "1"; }
                    else if (sortedDigit == two) { digitString += "2"; }
                    else if (sortedDigit == three) { digitString += "3"; }
                    else if (sortedDigit == four) { digitString += "4"; }
                    else if (sortedDigit == five) { digitString += "5"; }
                    else if (sortedDigit == six) { digitString += "6"; }
                    else if (sortedDigit == seven) { digitString += "7"; }
                    else if (sortedDigit == eight) { digitString += "8"; }
                    else if (sortedDigit == nine) { digitString += "9"; }

                }

                // Parse to int and add to sum of digits
                digitSum += Int32.Parse(digitString);
            }

            return digitSum.ToString();
        }

        static Boolean StringContainsString(string outer, string inner)
        {
            foreach (char i in inner)
            {
                if (!outer.Contains(i))
                {
                    return false;
                }
            }

            return true;
        }

        static int CharactersInCommon(string outer, string inner)
        {
            int count = 0;

            foreach (char i in inner)
            {
                if (outer.Contains(i))
                {
                    count++;
                }
            }

            return count;
        }
    }
}