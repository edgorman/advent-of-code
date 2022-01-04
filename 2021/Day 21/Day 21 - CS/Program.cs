
namespace AdventOfCode2021
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get input from txt file
            string[] lines = System.IO.File.ReadAllLines("test.txt");
            int a = lines[0].Last() - '0';
            int b = lines[1].Last() - '0';

            // Part 1
            Console.WriteLine(PartOne(a, b));

            // Part 2
            Console.WriteLine(PartTwo(a, b));
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

        static string PartTwo(int playerOneStartPosition, int playerTwoStartPosition)
        {
            // We will use a dictionary to store the possible game states and their counts
            // The key is a tuple <p1 score, p1 position, p2 score, p2 position, p1 turn>
            // And value is how many universes exist with that game state
            var gameStateDict = new Dictionary<Tuple<int, int, int, int, bool>, long>();
            gameStateDict.Add(Tuple.Create(0, playerOneStartPosition, 0, playerTwoStartPosition, true), 1);

            // These are the possible rolls you may get after rolling the quantum die three times
            var possibleRolls = new List<Tuple<int, int>>()
            {
                Tuple.Create(3, 1),
                Tuple.Create(4, 3),
                Tuple.Create(5, 6),
                Tuple.Create(6, 7),
                Tuple.Create(7, 6),
                Tuple.Create(8, 3),
                Tuple.Create(9, 1),
            };

            // Store how many have been won by each player
            long playerOneWins = 0;
            long playerTwoWins = 0;

            // Iterate until no game states remain
            while (gameStateDict.Count > 0)
            {
                // Pop next game state
                var state = gameStateDict.Keys.Last();
                var count = gameStateDict[state];
                gameStateDict.Remove(state);

                // Extract game state variables
                int playerOneScore = state.Item1;
                int playerOnePosition = state.Item2;
                int playerTwoScore = state.Item3;
                int playerTwoPosition = state.Item4;
                bool playerOneRoll = state.Item5;

                // Check for end game state
                if (playerOneScore >= 21)
                {
                    playerOneWins += count;
                    continue;
                }
                else if (playerTwoScore >= 21)
                {
                    playerTwoWins += count;
                    continue;
                }

                // Generate children game states
                // <p1 score, p1 position, p2 score, p2 position, p1 turn>
                var childStatesDict = new Dictionary<Tuple<int, int, int, int, bool>, long>();
                if (playerOneRoll)
                {
                    foreach (var roll in possibleRolls)
                    {
                        int newPosition = playerOnePosition + roll.Item1 % 10 == 0 ? 10 : playerOnePosition + roll.Item1;
                        childStatesDict.Add(
                            Tuple.Create(
                                playerOneScore + newPosition, 
                                newPosition, 
                                playerTwoScore, 
                                playerTwoPosition, 
                                !playerOneRoll
                            ), 
                            roll.Item2 * count
                        );
                    }
                }
                else
                {
                    foreach (var roll in possibleRolls)
                    {
                        int newPosition = playerTwoPosition + roll.Item1 % 10 == 0 ? 10 : playerTwoPosition + roll.Item1;
                        childStatesDict.Add(
                            Tuple.Create(
                                playerOneScore, 
                                playerOnePosition, 
                                playerTwoScore + newPosition, 
                                newPosition, 
                                !playerOneRoll
                            ), 
                            roll.Item2 * count
                        );
                    }
                }

                // Add child state to list of possible game states
                foreach (var childState in childStatesDict.Keys)
                {
                    if (gameStateDict.ContainsKey(childState))
                    {
                        gameStateDict[childState] *= childStatesDict[childState];
                    }
                    else
                    {
                        gameStateDict.Add(childState, childStatesDict[childState]);
                    }
                }

            }

            // Return win count from player than won
            long winningCount = playerOneWins > playerTwoWins ? playerOneWins : playerTwoWins;
            return winningCount.ToString();
        }
    }

}
