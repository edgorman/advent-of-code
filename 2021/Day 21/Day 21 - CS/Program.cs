
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("input.txt");
            int a = lines[0].Last() - '0';
            int b = lines[1].Last() - '0';

            // Part 1
            Console.WriteLine(PartOne(a, b));

            // Part 2
            Console.WriteLine(PartTwo(lines));
        }

        static string PartOne(int playerOneStartPosition, int playerTwoStartPosition)
        {
            int playerOneScore = 0;
            int playerOnePosition = playerOneStartPosition;
            int playerTwoScore = 0;
            int playerTwoPosition = playerTwoStartPosition;

            bool playerOneRoll = true;
            int lastDiceValue = 1;
            int diceRollCount = 0;

            // Iterate until one player's score passes 1000
            while (playerOneScore < 1000 && playerTwoScore < 1000)
            {
                // Roll dice three times using deterministic die
                int diceRollSum = 0;
                diceRollSum += lastDiceValue++ % 100;
                diceRollSum += lastDiceValue++ % 100;
                diceRollSum += lastDiceValue++ % 100;
                diceRollCount += 3;

                // Increment player positions and scores
                if (playerOneRoll)
                {
                    playerOnePosition += diceRollSum;
                    playerOnePosition = playerOnePosition % 10;
                    playerOneScore += playerOnePosition == 0 ? 10 : playerOnePosition;
                }
                else
                {
                    playerTwoPosition += diceRollSum;
                    playerTwoPosition = playerTwoPosition % 10;
                    playerTwoScore += playerTwoPosition == 0 ? 10 : playerTwoPosition;
                }

                // Let other player take their turn
                playerOneRoll = !playerOneRoll;
            }

            // Determine which player won
            if (playerOneScore >= 1000)
            {
                return (playerTwoScore * diceRollCount).ToString();
            }
            else
            {
                return (playerOneScore * diceRollCount).ToString();
            }
        }

        static string PartTwo(string[] lines)
        {
            return "";
        }
    }

}
