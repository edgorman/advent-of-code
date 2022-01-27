
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            var instructions = new List<Tuple<Cuboid, bool>>();

            for (int i = 0; i < lines.Length; i++)
            {
                var info = lines[i].Split(' ');

                var ranges = info[1].Split(',');
                var xr = ranges[0][2..].Split("..").Select(int.Parse).ToArray();
                var yr = ranges[1][2..].Split("..").Select(int.Parse).ToArray();
                var zr = ranges[2][2..].Split("..").Select(int.Parse).ToArray();
                var start = new Position(xr[0], yr[0], zr[0]);
                var end = new Position(xr[1], yr[1], zr[1]);

                var state = info[0] == "on";
                var cube = new Cuboid(start, end);

                instructions.Add(Tuple.Create(cube, state));
            }

            // Part 1
            var s = new Position(-50, -50, -50);
            var e = new Position(50, 50, 50);
            Console.WriteLine(PartOne(instructions, new Cuboid(s, e)));

            // Part 2
            s = new Position(int.MinValue, int.MinValue, int.MinValue);
            e = new Position(int.MaxValue, int.MaxValue, int.MaxValue);
            Console.WriteLine(PartOne(instructions, new Cuboid(s, e)));
        }

        static string PartOne(List<Tuple<Cuboid, bool>> instructions, Cuboid validRange)
        {
            var reactorCubes = new List<Tuple<Cuboid, int>>();

            // For each instruction
            foreach (var instruction in instructions)
            {
                var cube = instruction.Item1;
                var state = instruction.Item2;
                var newCubes = new List<Tuple<Cuboid, int>>();

                // Check that cube is contained by valid range
                if (!validRange.Contains(cube))
                {
                    continue;
                }

                // If state is on
                if (state)
                {
                    newCubes.Add(Tuple.Create(cube, 1));
                }

                // For each cube in reactor
                foreach (var rCube in reactorCubes)
                {
                    var otherCube = rCube.Item1;
                    var otherState = rCube.Item2;

                    // Calculate intersection of instruciton's cube and reactor's cube
                    var iCube = cube.Intersect(otherCube);

                    // If intersection cube has volume greater than zero
                    if (iCube.Volume() > 0)
                    {
                        newCubes.Add(Tuple.Create(iCube, -otherState));
                    }
                }

                // Add each new cube to reactor
                reactorCubes.AddRange(newCubes);
            }

            // Calculate volume of reactor that is on
            long onCount = 0;
            foreach (var rCube in reactorCubes)
            {
                onCount += rCube.Item1.Volume() * rCube.Item2;
            }

            return onCount.ToString();
        }
    }

    class Position
    {
        public int x;
        public int y;
        public int z;

        public Position()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Position(int i, int j, int k)
        {
            x = i;
            y = j;
            z = k;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Position);
        }

        public bool Equals(Position p)
        {
            if (p is null)
                return false;

            if (ReferenceEquals(this, p))
                return true;

            if (GetType() != p.GetType())
                return false;

            return x == p.x && y == p.y && z == p.z;
        }
    }

    class Cuboid
    {
        public Position start;
        public Position end;

        public Cuboid(Position s, Position e)
        {
            start = s;
            end = e;
        }

        public override String ToString()
        {
            return "x=" + start.x + ".." + end.x + ", " +
                "y=" + start.y + ".." + end.y + ", " +
                "z=" + start.z + ".." + end.z;
        }

        public long Volume()
        {
            return (Math.Abs(start.x - end.x) + 1) * 
                (Math.Abs(start.y - end.y) + 1) * 
                (Math.Abs(start.z - end.z) + 1);
        }

        public bool Contains(Cuboid o)
        {
            return start.x <= o.start.x &&
                start.y <= o.start.y &&
                start.z <= o.start.z &&
                end.x >= o.end.x && 
                end.y >= o.end.y && 
                end.z >= o.end.z;
        }

        public Cuboid Intersect(Cuboid o)
        {
            Position s = new Position(
                Math.Max(start.x, o.start.x),
                Math.Max(start.y, o.start.y),
                Math.Max(start.z, o.start.z)
            );
            Position e = new Position(
                Math.Min(end.x, o.end.x),
                Math.Min(end.y, o.end.y),
                Math.Min(end.z, o.end.z)
            );

            if (s.x > e.x || s.y > e.y || s.z > e.z)
            {
                s = new Position();
                e = new Position();
            }

            return new Cuboid(s, e);
        }
    }
}
