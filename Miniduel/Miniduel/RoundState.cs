using System;

namespace AutumnYard.Miniduel
{
    public class RoundState
    {
        private const int PLAYERS = 2;
        private const int FIGHTS = 3;
        private EGameState state;
        private EPiece[,] board;
        private RoundExecution execution;
        private int currentRound;

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
            execution = new RoundExecution();
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
            if (hasFinished)
                return false;

            var piecePlayer1 = board[0, currentRound];
            var piecePlayer2 = board[1, currentRound];
            RoundExecution.Fight(execution, piecePlayer1, piecePlayer2);

            currentRound++;

            hasFinished = currentRound >= FIGHTS;
            if (hasFinished)
            {
                state = EGameState.Results;
                Console.WriteLine($"");
                Console.WriteLine($"----------------------");
                Console.WriteLine($"FINISHED!! ");
                Console.WriteLine($"  And the winner was {RoundExecution.GetWinner(execution)}");
                Console.WriteLine($"  Results: {execution}");
                Console.WriteLine($"----------------------");
            }

            return true;
        }
    }
}
