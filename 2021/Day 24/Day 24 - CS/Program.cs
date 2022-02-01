// Code is heavily inspired by the reverse engineering performed
// by the folks over in r/adventofcode
// and would likely have taken me far longer without
// https://www.reddit.com/r/adventofcode/comments/rnejv5/2021_day_24_solutions/

using System.Diagnostics;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            Queue<string> instructions = new Queue<string>();
            foreach (string line in lines)
            {
                instructions.Enqueue(line);
            }

            // Create ALU
            var alu = new ALU();

            // Part 1
            var part1 = PartOne(new Queue<string>(instructions));
            alu.Reset(new Queue<string>(instructions));
            alu.Run(part1);

            // If z is 0, number was valid
            Debug.Assert(alu.z == 0);
            Console.WriteLine(part1);

            // Part 2
            var part2 = PartTwo(new Queue<string>(instructions));
            alu.Reset(new Queue<string>(instructions));
            alu.Run(part2);

            // If z is 0, number was valid
            Debug.Assert(alu.z == 0);
            Console.WriteLine(part2);
        }

        static long PartOne(Queue<string> instructions)
        {
            var s = instructions.ToList();
            var p = new Stack<Tuple<long, long>>();
            var v = 99999999999999;

            // MONDAD is split into 14 code chunks, one per input value to the program
            // We will process each in order, having simplified the instructions
            for (long i = 0; i < 14; i++)
            {
                var a = long.Parse(s[(int) (18 * i + 5)].Split(' ').Last());
                var b = long.Parse(s[(int) (18 * i + 15)].Split(' ').Last());

                // If value should be put on stack
                if (a > 0)
                {
                    p.Push(Tuple.Create(i, b));
                }
                // Else pop value from stack
                else
                {
                    var pop = p.Pop();
                    var j = pop.Item1;
                    b = pop.Item2;

                    var exp = a <= -b ? 13 - i : 13 - j;
                    v -= (long) Math.Abs((a + b) * Math.Pow(10, exp));
                }
            }
            return v;
        }

        static long PartTwo(Queue<string> instructions)
        {
            var s = instructions.ToList();
            var p = new Stack<Tuple<long, long>>();
            var v = 11111111111111;

            // MONDAD is split into 14 code chunks, one per input value to the program
            // We will process each in order, having simplified the instructions
            for (long i = 0; i < 14; i++)
            {
                var a = long.Parse(s[(int)(18 * i + 5)].Split(' ').Last());
                var b = long.Parse(s[(int)(18 * i + 15)].Split(' ').Last());

                // If value should be put on stack
                if (a > 0)
                {
                    p.Push(Tuple.Create(i, b));
                }
                // Else pop value from stack
                else
                {
                    var pop = p.Pop();
                    var j = pop.Item1;
                    b = pop.Item2;

                    var exp = a >= -b ? 13 - i : 13 - j;
                    v += (long)Math.Abs((a + b) * Math.Pow(10, exp));
                }
            }
            return v;
        }
    }

    class ALU
    {
        public long w;
        public long x;
        public long y;
        public long z;

        private Queue<string> _instructions;

        public void Reset(Queue<string> s)
        {
            w = 0;
            x = 0;
            y = 0;
            z = 0;
            _instructions = s;
        }

        public void Run(long value)
        {
            // Convert input integer to stack
            var input = new Stack<int>();
            foreach (var v in value.ToString().Reverse())
            {
                input.Push(v - '0');
            }

            // While there are operations remaining
            while (_instructions.TryDequeue(out string instruction))
            {
                var args = instruction.Split(' ');

                switch(args[0])
                {
                    case "inp":
                        Set(args[1], input.Pop());
                        break;
                    case "add":
                        Add(args[1], args[2]);
                        break;
                    case "mul":
                        Mul(args[1], args[2]);
                        break;
                    case "div":
                        Div(args[1], args[2]);
                        break;
                    case "mod":
                        Mod(args[1], args[2]);
                        break;
                    case "eql":
                        Eql(args[1], args[2]);
                        break;
                    default:
                        continue;
                }
            }
        }

        public long Get(string variable)
        {
            switch (variable)
            {
                case "w":
                    return w;
                case "x":
                    return x;
                case "y":
                    return y;
                case "z":
                    return z;
            }

            throw new Exception("Could not find variable: '" + variable + "'.");
        }

        public void Set(string variable, long value)
        {
            switch (variable)
            {
                case "w":
                    w = value;
                    return;
                case "x":
                    x = value;
                    return;
                case "y":
                    y = value;
                    return;
                case "z":
                    z = value;
                    return;
            }

            throw new Exception("Could not set variable: '" + variable + "'.");
        }

        public long Parse(string input)
        {
            return long.TryParse(input, out long _) ? long.Parse(input) : Get(input);
        }

        public void Add(string a, string b)
        {
            var m = Get(a);
            var n = Parse(b);
            Set(a, m + n);
        }

        public void Mul(string a, string b)
        {
            var m = Get(a);
            var n = Parse(b);
            Set(a, m * n);
        }

        public void Div(string a, string b)
        {
            var m = Get(a);
            var n = Parse(b);
            Set(a, (long) Math.Floor((double)m / (double)n));
        }

        public void Mod(string a, string b)
        {
            var m = Get(a);
            var n = Parse(b);
            Set(a, m % n);
        }

        public void Eql(string a, string b)
        {
            var m = Get(a);
            var n = Parse(b);
            Set(a, m == n ? 1 : 0);
        }
    }
}

