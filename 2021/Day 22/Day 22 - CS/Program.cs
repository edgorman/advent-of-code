
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");

            // Clean input
            var instructions = new List<Tuple<Cuboid, bool>>();

            for (int i = 0; i < lines.Length; i++)
            {
                var info = lines[i].Split(' ');

                var ranges = info[1].Split(',');
                var xr = ranges[0][2..].Split("..");
                var yr = ranges[1][2..].Split("..");
                var zr = ranges[2][2..].Split("..");
                var cube = new Cuboid(xr, yr, zr);

                instructions.Add(Tuple.Create(cube, info[0] == "on"));
            }

            // Part 1
            Console.WriteLine(PartOne(instructions));

            // Part 2
            Console.WriteLine(PartTwo(instructions));
        }

        static string PartOne(List<Tuple<Cuboid, bool>> instructions)
        {
            return "";
        }

        static string PartTwo(List<Tuple<Cuboid, bool>> instructions)
        {
            return "";
        }
    }

    class Position
    {
        public int x;
        public int y;
        public int z;

        public Position(int i, int j, int k)
        {
            x = i;
            y = j;
            z = k;
        }

        public override bool Equals(object obj) => this.Equals(obj as Position);

        public bool Equals(Position p)
        {
            if (p is null)
                return false;

            if (Object.ReferenceEquals(this, p))
                return true;

            if (this.GetType() != p.GetType())
                return false;

            return x == p.x && y == p.y && z == p.z;
        }

    }

    class Cuboid
    {
        public Tuple<int, int> x;
        public Tuple<int, int> y;
        public Tuple<int, int> z;

        public Cuboid(string[] xr, string[] yr, string[] zr)
        {
            int xa = int.Parse(xr[0]);
            int xb = int.Parse(xr[1]);
            int ya = int.Parse(yr[0]);
            int yb = int.Parse(yr[1]);
            int za = int.Parse(zr[0]);
            int zb = int.Parse(zr[1]);

            x = new Tuple<int, int>(xa, xb);
            y = new Tuple<int, int>(ya, yb);
            z = new Tuple<int, int>(za, zb);
        }
    }
}
