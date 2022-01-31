
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");

            // Clean input
            var amphipods = new List<Amphipod>();
            for (int x = 3; x < 10; x += 2)
            {
                for (int y = 2; y < 4; y++)
                {
                    if (lines[y][x] != '.')
                    {
                        amphipods.Add(
                            new Amphipod(
                                lines[y][x],
                                x,
                                y
                            )
                        );
                    }
                }
            }
            var start = new State(lines, 0, amphipods);

            // Part 1
            Console.WriteLine(PartOne(start));

            // Part 2
            Console.WriteLine(PartTwo(start));
        }

        static string PartOne(State start)
        {
            // Store states in priority queue sorted by energy heuristic
            var states = new PriorityQueue<State, int>();
            states.Enqueue(start, start.energy);

            // Store list of visited states in set of hashcodes
            var history = new HashSet<int>();
            int lastEstimate = 0;

            // Try the next state with lowest energy cost
            while (states.TryDequeue(out State next, out int estimate))
            {
                // Check if state is complete
                if (next.isDone())
                {
                    return next.energy.ToString();
                }

                // Add next state to list of visited states
                history.Add(next.GetHashCode());

                if (lastEstimate != estimate)
                {
                    Console.WriteLine(next.energy.ToString() + " - " + estimate.ToString());
                    lastEstimate = estimate;
                }

                // Generate possible states
                foreach (var pair in next.GenerateStates())
                {
                    // If state has not been visited before
                    if (!history.Contains(pair.Item1.GetHashCode()))
                    {
                        states.Enqueue(pair.Item1, pair.Item2);
                    }
                }
            }

            // Couldn't find a state that finished
            return "-1";
        }

        static string PartTwo(State start)
        {
            return "";
        }
    }

    class Amphipod
    {
        public char type;
        public int home;
        public int x;
        public int y;

        public Amphipod(char t, int x, int y)
        {
            type = t;
            home = (t - 'A') * 2 + 3;
            this.x = x;
            this.y = y;
        }

        public Amphipod(Amphipod a)
        {
            type = a.type;
            home = a.home;
            x = a.x;
            y = a.y;
        }

        public int GetCost()
        {
            if (type == 'A') { return 1; }
            if (type == 'B') { return 10; }
            if (type == 'C') { return 100; }
            if (type == 'D') { return 1000; }
            return -1;
        }

        public override string ToString()
        {
            return "(" + type + ":" + x + "," + y + ")";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Amphipod);
        }

        public bool Equals(Amphipod a)
        {
            if (a is null)
                return false;

            if (ReferenceEquals(this, a))
                return true;

            return type == a.type && x == a.x && y == a.y;
        }

        public override int GetHashCode()
        {
            return type ^ x.GetHashCode() ^ y.GetHashCode();
        }
    }

    class State
    {
        public string[] map;
        public int energy;
        public List<Amphipod> amphipods;

        public State(string[] m, int e, List<Amphipod> a)
        {
            map = m;
            energy = e;
            amphipods = a;
        }

        public bool isDone()
        {
            // For each amphipod
            foreach (var a in amphipods)
            {
                // If they are not home
                if (a.x != a.home)
                {
                    return false;
                }
            }

            // All are home
            return true;
        }

        public List<Tuple<State, int>> GenerateStates()
        {
            var newStates = new List<Tuple<State, int>>();

            // For each amphipod
            foreach (var a in amphipods)
            {
                // Generate possible moves for this amphipod
                var newPositions = new List<Tuple<int, int>>();
                
                // If amphipod is home
                if (IsHome(a))
                {
                    // If amphipod is blocking another type
                    if (IsBlockingAnotherType(a))
                    {
                        // Generate all valid moves to hallway
                        for (int x = 1; x < 12; x++)
                        {
                            newPositions.Add(Tuple.Create(x, 1));
                        }
                    }
                    // Else amphipod has no move to make this round
                    else
                    {
                        continue;
                    }
                }
                // Else if amphipod is in hallway or can move to hallway
                else if (IsInHallway(a) || CanReachHallway(a))
                {
                    // If amphipod can reach their home and it is not occupied by another type
                    if (CanReachX(a, a.home) && IsHomeAvailable(a))
                    {
                        // If same type is already home
                        if (IsLowerHomeAvailable(a))
                        {
                            // Move to lower position
                            newPositions.Add(Tuple.Create(a.home, 3));
                        }
                        else
                        {
                            // Move to upper position
                            newPositions.Add(Tuple.Create(a.home, 2));
                        }
                    }
                    // Else move to other available positions in hallway
                    else
                    {
                        // Generate all valid moves to hallway
                        for (int x = 1; x < 12; x++)
                        {
                            newPositions.Add(Tuple.Create(x, 1));
                        }
                    }
                }
                // Else this amphipod is blocked and has no valid moves
                else
                {
                    continue;
                }

                // Generate states from possible moves
                foreach (var p in newPositions)
                {
                    var count = CanMove(a, p.Item1, p.Item2);

                    if (count != -1)
                    {
                        newStates.Add(Move(a, p.Item1, p.Item2, count));
                    }
                }
            }

            return newStates;
        }

        public bool IsHome(Amphipod a)
        {
            return a.home == a.x;
        }

        public bool IsBlockingAnotherType(Amphipod a)
        {
            return a.y == 2 || a.type != map[3][a.x];
        }

        public bool IsInHallway(Amphipod a)
        {
            return a.y == 1;
        }

        public bool CanReachHallway(Amphipod a)
        {
            return a.y != 3 || map[2][a.x] == '.';
        }

        public bool CanReachX(Amphipod a, int destX)
        {
            int minX = Math.Min(a.x, destX);
            int maxX = Math.Min(a.x, destX);

            for (int x = minX; x < maxX; x++)
            {
                if (map[a.y][x] != '.')
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsHomeAvailable(Amphipod a)
        {
            if (!new char[] {'.', a.type}.Contains(map[3][a.home])) { return false; }
            if (!new char[] { '.', a.type }.Contains(map[2][a.home])) { return false; }
            return true;
        }

        public bool IsLowerHomeAvailable(Amphipod a)
        {
            return map[3][a.home] == '.';
        }

        public int CanMove(Amphipod a, int destX, int destY)
        {
            int count = 0;
            int startX = a.x;
            int startY = a.y;

            // If destination is outside room in hallway
            if (destY == 1 && new int[] {3, 5, 7, 9 }.Contains(destX)) { return -1; }

            // If amphipod is in room but room is not destination
            if (startY > 1 && startX != destX)
            {
                // Move amphipod up to hallway
                for (int y = startY - 1; y >= 1; y--)
                {
                    if (map[y][startX] != '.') { return -1; }
                    count++;
                }

                // Amphipod is now in hallway
                startY = 1;
            }

            // If amphipod is not at the same x as destination
            if (startX != destX)
            {
                // Move amphipod along to destination x
                int minX = Math.Min(startX, destX);
                int maxX = Math.Max(startX, destX);
                for (int x = minX; x <= maxX; x++)
                {
                    if (x == startX) { continue; }
                    if (map[startY][x] != '.') { return -1; }
                    count++;
                }
                startX = destX;
            }

            // If amphipod is moving into room
            if (startY != destY)
            {
                // Move amphipod along to destination y
                int minY = Math.Min(startY, destY);
                int maxY = Math.Max(startY, destY);
                for (int y = minY + 1; y <= maxY; y++)
                {
                    if (map[y][startX] != '.') { return -1; }
                    count++;
                }
                startY = destY;
            }

            // Sanity check, start should now equal end
            if (startX != destX || startY != destY) { return -1; }

            return count;
        }

        public Tuple<State, int> Move(Amphipod a, int x, int y, int count)
        {
            // Deep copy map and amphipods
            var newMap = new string[map.Length];
            Array.Copy(map, newMap, map.Length);
            var newAmphipods = amphipods.ConvertAll(a => new Amphipod(a));

            // Move amphipod to new position
            newMap[a.y] = newMap[a.y].Remove(a.x, 1).Insert(a.x, ".");
            newMap[y] = newMap[y].Remove(x, 1).Insert(x, a.type.ToString());
            newAmphipods.Remove(a);
            newAmphipods.Add(new Amphipod(a.type, x, y));

            // Calculate new cost and estimate to end
            var newEnergy = energy + a.GetCost() * count;
            var energyEstimate = newEnergy;
            //foreach (var b in newAmphipods)
            //{
            //    var countb = CanMove(b, b.home, 3);
            //    energyEstimate += (countb == -1 ? 10 : countb) * b.GetCost();
            //}

            // Return new state and new energy
            return Tuple.Create(new State(newMap, newEnergy, newAmphipods), energyEstimate);
        }

        public override string ToString()
        {
            string str = "";

            foreach(var a in amphipods)
            {
                str += a.ToString() + ", ";
            }

            return str;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as State);
        }

        public bool Equals(State s)
        {
            if (s is null)
                return false;

            if (ReferenceEquals(this, s))
                return true;

            foreach (Amphipod a in s.amphipods)
            {
                if (!amphipods.Contains(a))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

}
