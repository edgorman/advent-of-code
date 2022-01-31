
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

            // Part 1
            Console.WriteLine(PartOne(instructions, 74929995999389));

            // Part 2
            Console.WriteLine(PartOne(instructions, 11118151637112));
        }

        static string PartOne(Queue<string> instructions, long value)
        {
            // Create ALU
            var alu = new ALU();

            // Convert value to input stack
            var input = new Stack<int>();
            foreach (var v in value.ToString().Reverse())
            {
                input.Push(v - '0');
            }

            // Run value through MONAD program
            alu.Reset(instructions);
            alu.Run(input);

            // If z is 0, number was valid
            return alu.z.ToString();
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
            _instructions = new Queue<string>(s);
        }

        public void Run(Stack<int> inputs)
        {
            // While there are operations remaining
            while(_instructions.TryDequeue(out string instruction))
            {
                var args = instruction.Split(' ');

                switch(args[0])
                {
                    case "inp":
                        Set(args[1], inputs.Pop());
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

/**
Python code for finding the min and max values
From https://www.reddit.com/user/4HbQ/

instr, stack = [*open(0)], []

p, q = 99999999999999, 11111111111111

for i in range(14):
    a = int(instr[18*i+5].split()[-1])
    b = int(instr[18*i+15].split()[-1])

    if a > 0: stack+=[(i, b)]; continue
    j, b = stack.pop()

    p -= abs((a+b)*10**(13-[i,j][a>-b]))
    q += abs((a+b)*10**(13-[i,j][a<-b]))

print(p, q)
*/