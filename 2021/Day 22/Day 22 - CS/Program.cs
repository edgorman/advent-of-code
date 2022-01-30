
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            var instructions = new Dictionary<Cuboid, int>();

            for (int i = 0; i < lines.Length; i++)
            {
                var info = lines[i].Split(' ');

                var ranges = info[1].Split(',');
                var xr = ranges[0][2..].Split("..").Select(int.Parse).ToArray();
                var yr = ranges[1][2..].Split("..").Select(int.Parse).ToArray();
                var zr = ranges[2][2..].Split("..").Select(int.Parse).ToArray();
                var start = new Position(xr[0], yr[0], zr[0]);
                var end = new Position(xr[1], yr[1], zr[1]);

                var cube = new Cuboid(start, end);
                var value = info[0] == "on" ? 1 : 0;

                instructions.Add(cube, value);
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

        static string PartOne(Dictionary<Cuboid, int> instructions, Cuboid validRange)
        {
            var reactorCubes = new Dictionary<Cuboid, int>();

            // For each instruction
            foreach (var instruction in instructions)
            {
                var cube = instruction.Key;
                var value = instruction.Value;
                var newCubes = new Dictionary<Cuboid, int>();

                // Check that cube is contained by valid range
                if (!validRange.Contains(cube))
                {
                    continue;
                }

                // If state is on, add to dict of new cubes
                if (value == 1)
                {
                    newCubes.Add(cube, 1);
                }

                // For each cube in reactor
                foreach (var rCube in reactorCubes)
                {
                    // Calculate intersection of instruciton's cube and reactor's cube
                    var iCube = cube.Intersect(rCube.Key);

                    // If intersection cube has volume greater than zero
                    if (iCube.Volume() > 0)
                    {
                        // Add cube to dict of new cubes
                        if (newCubes.ContainsKey(iCube))
                        {
                            newCubes[iCube] -= rCube.Value;
                        }
                        else
                        {
                            newCubes.Add(iCube, -rCube.Value);
                        }
                    }
                }

                // Add each new cube to reactor
                foreach (var nCube in newCubes)
                {
                    // Add cube to dict of reactor cubes
                    if (reactorCubes.ContainsKey(nCube.Key))
                    {
                        reactorCubes[nCube.Key] += nCube.Value;
                    }
                    else
                    {
                        reactorCubes.Add(nCube.Key, nCube.Value);
                    }
                }
            }

            // Calculate volume of reactor that is on
            long onCount = 0;
            foreach (var rCube in reactorCubes)
            {
                if (rCube.Value != 0 && rCube.Key.Volume() != 1)
                {
                    onCount += rCube.Key.Volume() * rCube.Value;
                }
            }

            return onCount.ToString();
        }
    }

    class Position
    {
        public long x;
        public long y;
        public long z;

        public Position()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Position(long i, long j, long k)
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

        public override int GetHashCode()
        {
            return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
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

        public override bool Equals(object obj)
        {
            return Equals(obj as Cuboid);
        }

        public bool Equals(Cuboid o)
        {
            if (o is null)
                return false;

            if (ReferenceEquals(this, o))
                return true;

            if (GetType() != o.GetType())
                return false;

            return start.Equals(o.start) && end.Equals(o.end) ||
                start.Equals(o.end) && end.Equals(o.start);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
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
