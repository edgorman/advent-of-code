
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
            SFNumber value = input[0];

            // For each number
            for (int i = 1; i < input.Length; i++)
            {
                // Add next number to value
                value = new SFNumber(value, input[i]);

                // Reduce number
                value.Reduce();
            }

            // Calculate magniute
            int magnitude = value.Magnitude();

            return magnitude.ToString();
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

        public bool ContainsRegulars()
        {
            return !IsRegular() && _left.IsRegular() && _right.IsRegular();
        }

        public string ToString()
        {
            if (_isRegular)
            {
                return _regular.ToString();
            }
            else
            {
                return "[" + _left.ToString() + "," + _right.ToString() + "]";
            }
        }

        public void AddToLeftmost(int value)
        {
            // Base case: number is regular
            if (_isRegular)
            {
                _regular += value;
            }
            // Base case: left is regular
            else if (_left.IsRegular())
            {
                // Add value to left child
                _left = new SFNumber(_left.GetRegular() + value);
            }
            // Recurse case: 
            else
            {
                _left.AddToLeftmost(value);
            }
        }

        public void Reduce()
        {
            // Repeat until no reductions remain
            bool reductionsMade;
            do
            {
                // Explode value
                var eResult = Explode();
                reductionsMade = eResult.Item1;

                // If explosion didn't reduce
                if (!reductionsMade)
                {
                    // Split value
                    var sResult = Split();
                    reductionsMade = sResult.Item1;
                }
            }
            while (reductionsMade);
        }

        public Tuple<bool, int, int, bool> Explode(int depth = 0)
        {
            // Base case: number is regular
            if (_isRegular)
            {
                return Tuple.Create(false, -1, -1, false);
            }

            // Base case: depth is enough to explode and children are regular
            if (depth >= 4 && ContainsRegulars())
            {
                return Tuple.Create(true, _left.GetRegular(), _right.GetRegular(), false);
            }

            // Check if either side will explode
            int leftValue = -1;
            int rightValue = -1;
            bool exploded = false;
            bool leftExploded = false;
            bool rightExploded = false;
            bool isRightmost = false;
            
            // Check if left side exploded
            var left = _left.Explode(depth + 1);
            if (left.Item1)
            {
                exploded = left.Item1;
                leftValue = left.Item2;
                rightValue = left.Item3;
                isRightmost = left.Item4;

                // If left children are regulars
                if (_left.ContainsRegulars())
                {
                    if (leftValue != -1 && rightValue != -1)
                    {
                        _left = new SFNumber(0);
                    }
                    leftExploded = true;
                }

                // If value returned is rightmost
                if (rightValue != -1 && isRightmost)
                {
                    // Add right value to leftmost child of right
                    _right.AddToLeftmost(rightValue);
                    rightValue = -1;
                }
            }
            // Else check if right side exploded
            else
            {
                var right = _right.Explode(depth + 1);
                if (right.Item1)
                {
                    exploded = right.Item1;
                    leftValue = right.Item2;
                    rightValue = right.Item3;
                    isRightmost = right.Item4;

                    // If right children are regulars
                    if (_right.ContainsRegulars())
                    {
                        if (leftValue != -1 && rightValue != -1)
                        {
                            _right = new SFNumber(0);
                        }
                        rightExploded = true;
                        isRightmost = true;
                    }
                }
            }

            // If an explosion has occurred
            if (exploded)
            {
                // Check if left child is regular and left value is valid
                if (_left.IsRegular() && leftValue != -1 && !leftExploded)
                {
                    // Add left value to left child
                    _left = new SFNumber(_left.GetRegular() + leftValue);
                    leftValue = -1;
                }

                // Check if right child is regular and right value is valid
                if (_right.IsRegular() && rightValue != -1 && !rightExploded)
                {
                    // Add right value to right child
                    _right = new SFNumber(_right.GetRegular() + rightValue);
                    rightValue = -1;
                }
            }

            return Tuple.Create(exploded, leftValue, rightValue, isRightmost);
        }

        public Tuple<bool, int, int> Split()
        {
            // Base case: if number is regular
            if (_isRegular)
            {
                // If number should be split
                if (_regular >= 10)
                {
                    return Tuple.Create(
                        true,
                        (int)Math.Floor(_regular / 2f),
                        (int)Math.Ceiling(_regular / 2f)
                    );
                }
                // Else number is not split
                else
                {
                    return Tuple.Create(false, -1, -1);
                }
            }

            // Check if either side will split
            bool split = false;

            // Check if left side split
            var left = _left.Split();
            if (left.Item1)
            {
                split = left.Item1;

                if (left.Item2 != -1 && left.Item3 != -1)
                {
                    _left = new SFNumber(
                        new SFNumber(left.Item2),
                        new SFNumber(left.Item3)
                    );
                }
            }
            // Else check if right side exploded
            else
            {
                var right = _right.Split();
                if (right.Item1)
                {
                    split = right.Item1;

                    if (right.Item2 != -1 && right.Item3 != -1)
                    {
                        _right = new SFNumber(
                            new SFNumber(right.Item2),
                            new SFNumber(right.Item3)
                        );
                    }
                }
            }

            return Tuple.Create(split, -1, -1);
        }

        public int Magnitude()
        {
            // Base case: if number is regular
            if (_isRegular)
            {
                return _regular;
            }

            // Recurse case: get values of left and right
            int leftValue = 3 * _left.Magnitude();
            int rightValue = 2 * _right.Magnitude();

            return leftValue + rightValue;
        }
    }
}

