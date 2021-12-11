
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            char[][] input = new char[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                input[i] = lines[i].ToCharArray();
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(char[][] input)
        {
            int errorScore = 0;

            // For each line
            for (int i = 0; i < input.Length; i++)
            {
                Stack<char> history = new Stack<char>();

                // For each character in line
                foreach (char nextChar in input[i])
                {
                    // If next char is opening
                    if (nextChar == '(' || 
                        nextChar == '[' ||
                        nextChar == '{' ||
                        nextChar == '<')
                    {
                        history.Push(nextChar);
                    }
                    // Else next char is closing
                    else
                    {
                        char topChar = history.Pop();

                        // If top char matches next char
                        if (topChar == '(' && nextChar == ')' ||
                            topChar == '[' && nextChar == ']' ||
                            topChar == '{' && nextChar == '}' ||
                            topChar == '<' && nextChar == '>')
                        {
                            continue;
                        }
                        // Else exit loop due to syntax error
                        else
                        {
                            // Add error value for character
                            switch(nextChar)
                            {
                                case ')': errorScore += 3; break;
                                case ']': errorScore += 57; break;
                                case '}': errorScore += 1197; break;
                                case '>': errorScore += 25137; break;
                            }

                            break;
                        }
                    }
                }
            }

            return errorScore.ToString();
        }

        static string PartTwo(char[][] input)
        {
            List<char[]> incompleteLines = input.ToList();

            // For each line
            for (int i = 0; i < input.Length; i++)
            {
                Stack<char> history = new Stack<char>();

                // For each character in line
                foreach(char nextChar in input[i])
                {
                    // If next char is opening
                    if (nextChar == '(' ||
                        nextChar == '[' ||
                        nextChar == '{' ||
                        nextChar == '<')
                    {
                        history.Push(nextChar);
                    }
                    // Else next char is closing
                    else
                    {
                        char topChar = history.Pop();

                        // If top char matches next char
                        if (topChar == '(' && nextChar == ')' ||
                            topChar == '[' && nextChar == ']' ||
                            topChar == '{' && nextChar == '}' ||
                            topChar == '<' && nextChar == '>')
                        {
                            continue;
                        }
                        // Else exit loop due to syntax error
                        else
                        {
                            incompleteLines.Remove(input[i]);
                            break;
                        }
                    }
                }
            }

            long[] autocompleteScores = new long[incompleteLines.Count];

            // For each incomplete line
            for (int i = 0; i < incompleteLines.Count; i++)
            {
                Stack<char> history = new Stack<char>();

                // For each character in line
                foreach (char nextChar in incompleteLines[i])
                {
                    // If next char is opening
                    if (nextChar == '(' ||
                        nextChar == '[' ||
                        nextChar == '{' ||
                        nextChar == '<')
                    {
                        history.Push(nextChar);
                    }
                    // Else next char is closing
                    else
                    {
                        history.Pop();
                    }
                }

                // History will contain remaining characters
                // Iterate until history is empty
                while(history.Count > 0)
                {
                    char nextChar = history.Pop();
                    autocompleteScores[i] *= 5;

                    // Add autocomplete value for character
                    switch (nextChar)
                    {
                        case '(': autocompleteScores[i] += 1; break;
                        case '[': autocompleteScores[i] += 2; break;
                        case '{': autocompleteScores[i] += 3; break;
                        case '<': autocompleteScores[i] += 4; break;
                    }
                }
            }

            // Sort autocomplete scores
            Array.Sort(autocompleteScores);

            // Return the middle score
            return autocompleteScores[autocompleteScores.Length / 2].ToString();
        }
    }
}