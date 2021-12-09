using System.Text.RegularExpressions;

namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");

            // Clean input
            int[] input = Array.ConvertAll(lines[0].Split(','), int.Parse);
            List<Board> boards = new List<Board>();
            
            int index = 2;
            while (index < lines.Length)
            {
                List<int> numbers = new List<int>();

                for (int i = 0; i < 5; i++)
                {
                    numbers.AddRange(
                        Array.ConvertAll(
                            Regex.Split(lines[index + i].Trim(), @"\D+"),
                            int.Parse
                        )
                    );
                }

                boards.Add(new Board(numbers.ToArray()));
                index += 6;
            }

            // Part 1
            Console.WriteLine(PartOne(input, boards));

            foreach(Board board in boards)
            {
                board.reset();
            }

            // Part 2
            Console.WriteLine(PartTwo(input, boards));
        }

        static string PartOne(int[] input, List<Board> boards)
        {
            foreach(int value in input)
            {
                foreach(Board board in boards)
                {
                    board.add(value);
                    
                    if (board.checkComplete())
                    {
                        return board.score().ToString();
                    }
                }
            }

            return "-1";
        }

        static string PartTwo(int[] input, List<Board> boards)
        {
            List<Board> won = new List<Board>();

            foreach (int value in input)
            {
                if (boards.Count == won.Count)
                {
                    break;
                }

                foreach (Board board in boards)
                {
                    board.add(value);

                    if (!won.Contains(board))
                    {
                        if (board.checkComplete())
                        {
                            won.Add(board);
                        }
                    }
                }
            }

            return won.Last().score().ToString();
        }
    }

    class Board
    {
        int length;
        int[] board;

        List<int> history;
        int[] verticalCounts;
        int[] horizontalCounts;

        public Board(int[] ns)
        {
            length = (int) Math.Sqrt(ns.Length);
            board = (int[]) ns.Clone();

            history = new List<int>();
            verticalCounts = new int[5];
            horizontalCounts = new int[5];
        }

        public void reset()
        {
            history.Clear();
            verticalCounts = new int[5];
            horizontalCounts = new int[5];
        }

        public bool add(int n)
        {
            if (board.Contains(n))
            {
                // Calculate x y position in board
                int index = Array.IndexOf(board, n);
                int y = index / length;
                int x = index % length;

                // Add to history and horz and vert checks
                history.Add(n);
                verticalCounts[y]++;
                horizontalCounts[x]++;

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkComplete()
        {
            return verticalCounts.Contains(length) || horizontalCounts.Contains(length);
        }

        public int score()
        {
            List<int> unseen = new List<int>();

            foreach (int n in board)
            {
                if (!history.Contains(n))
                {
                    unseen.Add(n);
                }
            }

            return unseen.Sum() * history.Last();
        }
    }
}