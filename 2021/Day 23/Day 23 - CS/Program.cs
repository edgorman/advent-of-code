
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            var amphipods = new List<Amphipod>();
            for (int y = 1; y < lines.Length; y++)
            {
                for (int x = 1; x < lines[y].Length; x++)
                {
                    if (new char[] { 'A', 'B', 'C', 'D' }.Contains(lines[y][x]))
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

            var newLines = new string[lines.Length + 2];
            newLines[0] = lines[0];
            newLines[1] = lines[1];
            newLines[2] = lines[2];
            newLines[3] = "  #D#C#B#A#";
            newLines[4] = "  #D#B#A#C#";
            newLines[5] = lines[3];
            newLines[6] = lines[4];

            var newAphipods = new List<Amphipod>();
            for (int y = 1; y < newLines.Length; y++)
            {
                for (int x = 1; x < newLines[y].Length; x++)
                {
                    if (new char[] { 'A', 'B', 'C', 'D' }.Contains(newLines[y][x]))
                    {
                        newAphipods.Add(
                            new Amphipod(
                                newLines[y][x],
                                x,
                                y
                            )
                        );
                    }
                }
            }
            var newStart = new State(newLines, 0, newAphipods);

            // Part 2
            Console.WriteLine(PartOne(newStart));
        }

        static string PartOne(State start)
        {
            // Initialise priority queue of states and energy estimates
            var states = new PriorityQueue<State, int>();
            states.Enqueue(start, 0);

            // Initialise hashset of previously visited states by hashcode
            var history = new HashSet<int>() { start.GetHashCode() };

            // While there are states remaining
            while (states.TryDequeue(out State nextState, out int nextPriority))
            {
                // Check if next state is end state
                if (nextState.IsDone())
                {
                    return nextState.energy.ToString();
                }

                // For each possible state/priority reachable from next state
                foreach (var statePriority in nextState.GenerateNewStates())
                {
                    var newState = statePriority.Item1;
                    var newPriority = statePriority.Item2;

                    // If this new state has been seen before
                    if (newState.IsDone() || !history.Contains(newState.GetHashCode()))
                    {
                        // Insert state into priority queue and hashset
                        states.Enqueue(newState, newPriority);
                        history.Add(newState.GetHashCode());
                    }
                }    
            }
            
            // Return start state energy
            return start.energy.ToString();
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
            return ToString().GetHashCode();
        }
    }

    class State
    {
        public string[] map;
        public int energy;
        public List<Amphipod> amphipods;
        public State previous;

        public State()
        {
            map = new string[0];
            energy = 0;
            amphipods = new List<Amphipod>();
        }

        public State(string[] m, int e, List<Amphipod> a)
        {
            map = m;
            energy = e;
            amphipods = a;
        }

        public bool IsDone()
        {
            foreach (var a in amphipods)
            {
                if (!IsAmphipodDone(a))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsAmphipodDone(Amphipod a)
        {
            // If amphipod is not home, return false
            if (a.x != a.home)
            {
                return false;
            }

            // If blocking amphipod of another type beneathm return false
            for (int y = a.y; y < map.Length - 1; y++)
            {
                if (map[y][a.x] != a.type)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsAmphipodStuck(Amphipod a)
        {
            // If amphipod in hallway, return false
            if (a.y == 1)
            {
                return false;
            }

            // If amphipod has anything other than empty above it, return true
            for (int y = a.y - 1; y >= 1; y--)
            {
                if (map[y][a.x] != '.')
                {
                    return true;
                }
            }

            return false;
        }

        public List<Tuple<State, int>> GenerateNewStates()
        {
            var newStates = new List<Tuple<State, int>>();
            var roomEntrance = new int[] { 3, 5, 7, 9 };

            // For each amphipod
            foreach (var a in amphipods)
            {
                // Check if it's home and not blocking another type
                if (IsAmphipodDone(a))
                {
                    continue;
                }

                // Check if it's stuck in a room behing another type
                if (IsAmphipodStuck(a))
                {
                    continue;
                }

                // For each possible position in map
                for (int y = 1; y < map.Length - 1; y++)
                {
                    for (int x = 1; x < map[y].Length; x++)
                    {
                        // Filter positions that are:
                        //  empty,
                        //  not outside a room,
                        //  rooms that are not home
                        if (map[y][x] == '.' && (
                            (y == 1 && !roomEntrance.Contains(x)) ||
                            (y > 1 && x == a.home)
                           ))
                        {
                            var statePriority = TryMove(a, x, y);
                            var state = statePriority.Item1;
                            var priority = statePriority.Item2;

                            // if priority is -1, skip
                            if (priority == -1)
                            {
                                continue;
                            }
                            // Else add state priority to list
                            else
                            {
                                newStates.Add(statePriority);
                            }
                        }
                    }
                }
            }

            return newStates;
        }

        public Tuple<State, int> TryMove(Amphipod a, int destX, int destY)
        {
            int count = 0;
            int startX = a.x;
            int startY = a.y;

            // If amphipod is in room that is not the destination, move amphipod up
            if (startY > 1 && startX != destX)
            {
                for (int y = startY; y > 0; y--)
                {
                    if (y != startY)
                    {
                        if (map[y][startX] != '.')
                        {
                            return Tuple.Create(new State(), -1);
                        }

                        count++;
                    }
                }
            }
            startY = 1;

            // If amphipod is not at their destination x, move amphipod along
            if (startX != destX)
            {
                for (int x = Math.Min(startX, destX); x <= Math.Max(startX, destX); x++)
                {
                    if (x != startX)
                    {
                        if (map[1][x] != '.')
                        {
                            return Tuple.Create(new State(), -1);
                        }

                        count++;
                    }
                }
            }
            startX = destX;

            // If destination is in a room and currently above the room, move amphipod down
            if (destY > 1 && startX == destX)
            {
                for (int y = startY; y <= destY; y++)
                {
                    if (y != startY)
                    {
                        if (map[y][destX] != '.')
                        {
                            return Tuple.Create(new State(), -1);
                        }

                        count++;
                    }
                }
            }
            startY = destY;

            // Amphipod is able to move to destination, create new state
            var newMap = new string[map.Length];
            var newEnergy = energy + count * a.GetCost();
            var newAmphipods = amphipods.ConvertAll(a => new Amphipod(a));

            Array.Copy(map, newMap, map.Length);
            newMap[a.y] = newMap[a.y].Remove(a.x, 1).Insert(a.x, ".");
            newMap[destY] = newMap[destY].Remove(destX, 1).Insert(destX, a.type.ToString());

            newAmphipods.Remove(a);
            newAmphipods.Add(new Amphipod(a.type, destX, destY));

            var newState = new State(newMap, newEnergy, newAmphipods);
            newState.previous = this;
            return Tuple.Create(newState, newEnergy);
        }

        public override string ToString()
        {
            string str = "";

            foreach(var a in amphipods)
            {
                if (!IsAmphipodDone(a))
                {
                    str += a.ToString();
                }
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
            var hashCode = 13;

            foreach (var a in amphipods)
            {
                hashCode *= 13 + a.GetHashCode();
            }

            return hashCode;
        }
    }

}
