
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");

            // Clean input
            SFNumber[] input = new SFNumber[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                input[i] = ParseSFNumber(lines[i]);
            }

            // Part 1
            Console.WriteLine(PartOne(input));

            // Part 2
            Console.WriteLine(PartTwo(input));
        }

        static string PartOne(SFNumber[] input)
        {
            SFNumber number = input[0];

            // Repeat until no reductions remain
            bool reductionsMade;
            do
            {
                // Explode SFNumbers
                var eResult = number.Explode();

                // Split SFNumbers
                var sResult = number.Split();

                // Determine if reduction was made
                reductionsMade = eResult.Item1 || sResult.Item1;
            }
            while (reductionsMade);

            return "";
        }

        static SFNumber ParseSFNumber(string line)
        {
            // Base case: line contains regular number
            if (line.Length == 1)
            {
                return new SFNumber(Convert.ToInt32(line));
            }

            // Find the index of char separating the pair
            int index = 0;
            Stack<char> brackets = new Stack<char>();

            for (int i = 1; i < line.Length - 1; i++)
            {
                // If character is open bracket
                if (line[i] == '[')
                {
                    brackets.Push(line[i]);
                }
                // Else if character is open bracket
                else if (line[i] == ']')
                {
                    brackets.Pop();
                }
                // Else if character is separator AND brackets are closed
                else if (line[i] == ',' && brackets.Count == 0)
                {
                    index = i;
                    break;
                }
            }

            // Recurse case: parse each pair and return
            SFNumber left = ParseSFNumber(line[1..index]);
            SFNumber right = ParseSFNumber(line[(index+1)..(line.Length-1)]);

            return new SFNumber(left, right);
        }

        static string PartTwo(SFNumber[] input)
        {
            return "";
        }
    }

    class SFNumber
    {
        private int _regular;
        private bool _isRegular;
        private SFNumber _left;
        private SFNumber _right;

        public SFNumber(SFNumber left, SFNumber right)
        {
            _left = left;
            _right = right;
            _isRegular = false;
        }

        public SFNumber(int regular)
        {
            _regular = regular;
            _isRegular = true;
        }

        public bool IsRegular()
        {
            return _isRegular;
        }

        public int GetRegular()
        {
            return _regular;
        }

        public Tuple<bool, int, int> Explode(int depth = 0)
        {
            // Base case: depth is enough to explode and children are regular
            if (depth >= 4 && _left.IsRegular() && _right.IsRegular())
            {
                return Tuple.Create(true, _left.GetRegular(), _right.GetRegular());
            }

            // Check if left will explode
            var left = _left.Explode(depth + 1);
            if (left.Item1)
            {
                // If left SFNumber is a regular, it exploded
                if (_left.IsRegular())
                {
                    // Change left SFNumber to be 0
                    _left = new SFNumber(0);
                }
                // Else 
                else
                {

                }

                // If right SFNumber is a regular
                if (_right.IsRegular())
                {
                    // Change right SFNumber to be exploded right value
                    _right = new SFNumber(left.Item3);
                }

                return Tuple.Create(left.Item1, left.Item2, left.Item3);
            }

            // Check if right explode
            var right = _right.Explode();
            if (right.Item1)
            {

            }

            // Neither child exploded, return
            return Tuple.Create(false, -1, -1);
        }

        public Tuple<SFNumber, bool> Split()
        {
            return Tuple.Create(new SFNumber(0), false);
        }
    }
}

