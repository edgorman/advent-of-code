
using System.Text.RegularExpressions;

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

        static string PartOne(string[] input)
        {
            string value = input[0];

            // Add each input to value successively
            for (int i = 1; i < input.Length; i++)
            {
                value = Add(value, input[i]);

                // Reduce number until none remain
                while (true)
                {
                    // Check for explosion
                    string explodeValue = Explode(value);
                    if (explodeValue != value) 
                    { 
                        value = explodeValue; 
                        continue; 
                    }

                    // Check for split
                    string splitValue = Split(value);
                    if (splitValue != value) 
                    { 
                        value = splitValue; 
                        continue; 
                    }

                    // No reductions happened, exit loop
                    break;
                }
            }

            return Magnitude(value);
        }
        static string Add(string left, string right)
        {
            return "[" + left + "," + right + "]";
        }
        static string Explode(string number)
        {
            int openBracketCount = 0;

            // For each character in the number
            for (int i = 0; i < number.Length; i++)
            {
                char c = number[i];

                // Increment or decrement open bracket count
                if (c == '[')
                {
                    openBracketCount++;
                    continue;
                }
                else if (c == ']')
                {
                    openBracketCount--;
                    continue;
                }

                // If count is greater than four
                if (openBracketCount > 4)
                {
                    string remainingChars = number.Substring(i);

                    // If the next characters are a pair
                    if (Regex.IsMatch(remainingChars, "^\\d+,\\d+"))
                    {
                        // Extract pair of numbers
                        var match = Regex.Match(remainingChars, "^\\d+,\\d+");
                        var pair = match.Value.Split(',');
                        var leftValue = pair[0];
                        var rightValue = pair[1];

                        // Replace match area with a zero
                        number = number[..(i - 1)] + "0" + number[(i + 1 + match.Value.Length)..];

                        // Add value to next left number
                        var leftMatch = Regex.Match(number[..(i - 1)], "\\d+", RegexOptions.RightToLeft);

                        if (leftMatch.Success)
                        {
                            int value = int.Parse(leftMatch.Value) + int.Parse(leftValue);
                            number = number[..leftMatch.Index] + value + number[(leftMatch.Index + leftMatch.Length)..];
                        }

                        // Add value to next right number
                        var rightMatch = Regex.Match(number[(i + 1)..], "\\d+");

                        if (rightMatch.Success)
                        {
                            int value = int.Parse(rightMatch.Value) + int.Parse(rightValue);
                            number = number[..(1 + i + rightMatch.Index)] + value + number[(1 + i + rightMatch.Index + rightMatch.Length)..];
                        }

                        return number;
                    }
                }
            }

            return number;
        }
        
        static string Split(string number)
        {
            var matches = Regex.Matches(number, "\\d+");

            // For each number in the string
            foreach (Match match in matches)
            {
                int value = int.Parse(match.Value);

                // If number should be split
                if (value > 9)
                {
                    var left = Math.Floor(value / 2f).ToString();
                    var right = Math.Ceiling(value / 2f).ToString();
                    var result = Add(left, right);

                    return number[..match.Index] + result + number[(match.Index + match.Length)..];
                }
            }

            return number;
        }

        static string Magnitude(string number)
        {
            // Iterate until a single number remains
            while(number.Contains(','))
            {
                // Find next regular pair
                var match = Regex.Match(number, "\\[\\d+,\\d+\\]");
                var pair = match.Value.Split(',');
                var leftValue = pair[0][1..];
                var rightValue = pair[1][0..^1];

                // Calculate magnitude of pair
                var magnitude = int.Parse(leftValue) * 3 + int.Parse(rightValue) * 2;
                
                // Replace pair with magnitude
                number = number[..match.Index] + magnitude + number[(match.Index + match.Length)..];
            }

            return number;
        }
        static string PartTwo(string[] input)
        {
            return "";
        }
    }

}

