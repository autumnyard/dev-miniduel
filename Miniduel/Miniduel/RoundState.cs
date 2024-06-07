using System;

namespace AutumnYard.Miniduel
{
    public class RoundState
    {
        private const int PLAYERS = 2;
        private const int FIGHTS = 3;
        public EGameState state;
        public EPiece[,] board;
        public RoundExecution execution;
        public int currentRound;

        public RoundState()
        {
            state = default;
            board = new EPiece[PLAYERS, FIGHTS];
            execution = new RoundExecution();
            currentRound = 0;
        }

        public void StartGame()
        {
            state = EGameState.Preparation;
        }

        public bool IsValidPosition(int player, int location)
        {
            if (board.Rank <= player)
                return false;

            int locations = board.Length / board.Rank;
            if (locations <= location)
                return false;

            return true;
        }

        public bool CanDuel()
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

        public void StartDuel()
        {
            state = EGameState.Dueling;

            execution = new RoundExecution();
            currentRound = 0;
        }

        public bool PlayNextRound(out bool hasFinished)
        {
            hasFinished = currentRound >= FIGHTS;
            if (hasFinished)
                return false;

            //var round = new RoundExecution();
            var piecePlayer1 = board[0, currentRound];
            var piecePlayer2 = board[1, currentRound];
            execution.Fight(piecePlayer1, piecePlayer2);

            //results[currentRound] = round;
            currentRound++;

            // TODO: Check if finished????
            hasFinished = currentRound >= FIGHTS;
            if (hasFinished)
            {
                state = EGameState.Results;
                Console.WriteLine($"");
                Console.WriteLine($"----------------------");
                Console.WriteLine($"FINISHED!! ");
                Console.WriteLine($"  And the winner was {execution.GetWinner()}");
                Console.WriteLine($"  Results: {execution.ToString()}");
                Console.WriteLine($"----------------------");
            }

            return true;
        }
    }
}
