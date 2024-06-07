using System;

namespace AutumnYard.Miniduel
{
    public class RoundState
    {
        private const int PLAYERS = 2;
        private const int FIGHTS = 3;

        // Round State
        private EGameState state;
        private EPiece[,] board;

        // Round Execution
        private int currentRound;
        private FightResult[] results;

        public RoundState()
        {
            state = default;
            board = new EPiece[PLAYERS, FIGHTS];
            currentRound = 0;
        }

        public void StartGame()
        {
            state = EGameState.Preparation;
        }

        public bool SetPiece(int player, int location, EPiece piece)
        {
            if (state != EGameState.Preparation)
                return false;

            bool isValidPosition = IsValidPosition(player, location);
            if (isValidPosition == false)
                return false;

            board[player, location] = piece;
            return true;
        }

        private bool IsValidPosition(int player, int location)
        {
            if (board.Rank <= player)
                return false;

            int locations = board.Length / board.Rank;
            if (locations <= location)
                return false;

            return true;
        }

        public bool StartDuel()
        {
            bool correctState = state != EGameState.Preparation;
            if (correctState)
                return false;

            bool canDuel = CanDuel();
            if (!canDuel)
                return false;

            state = EGameState.Dueling;
            results = new FightResult[FIGHTS];
            currentRound = 0;
            return true;
        }

        private bool CanDuel()
        {
            for (int i = 0; i < PLAYERS; i++)
            {
                for (int j = 0; j < FIGHTS; j++)
                {
                    if (board[i, j] == EPiece.None)
                        return false;
                }
            }

            return true;
        }

        public bool PlayNextFight(out bool hasFinished)
        {
            hasFinished = false;

            bool correctState = state != EGameState.Dueling;
            if (correctState)
                return false;

            bool result = PlayNextRound(out hasFinished);
            return result;
        }


        private bool PlayNextRound(out bool hasFinished)
        {
            hasFinished = currentRound >= FIGHTS;

            bool correctState = state != EGameState.Dueling;
            if (correctState)
                return false;

            if (hasFinished)
                return false;

            FightData data = new FightData()
            {
                offense = FightOperations.CalculateOffense(in results, in currentRound),
                piecePlayer1 = board[0, currentRound],
                piecePlayer2 = board[1, currentRound],
            };

            // Refresh
            {
                results[currentRound] = FightOperations.Fight(in data);
                currentRound++;
            }

            // Print
            {
                Console.WriteLine(results[currentRound - 1]);
                Console.WriteLine();
            }

            hasFinished = currentRound >= FIGHTS;
            if (hasFinished)
            {
                state = EGameState.Results;
                RoundResults roundResults = FightOperations.CalculateResults(in results, in currentRound);

                Console.WriteLine($"");
                Console.WriteLine($"----------------------");
                Console.WriteLine($"FINISHED!! ");
                Console.WriteLine($"  And the winner was {RoundResults.GetWinner(roundResults)}");
                Console.WriteLine($"  Results: {roundResults}");
                Console.WriteLine($"----------------------");
            }

            return true;
        }
    }
}
