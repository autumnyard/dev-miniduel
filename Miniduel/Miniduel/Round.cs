using System;
using System.Collections.Generic;

namespace AutumnYard.Miniduel
{
    public static class RoundConstants
    {
        public const int PLAYERS = 2;
        public const int FIGHTS = 3;
    }

    public class Round
    {
        // Round State
        private ERoundState state;
        private EPiece[,] board;

        // Round Execution
        private List<FightResult> results;

        public Round()
        {
            state = default;
            board = new EPiece[RoundConstants.PLAYERS, RoundConstants.FIGHTS];

            state = ERoundState.Preparation;
        }

        public bool SetPiece(int player, int location, EPiece piece)
        {
            if (state != ERoundState.Preparation)
                return false;

            bool isValidPosition = RoundOperations.IsValidPosition(in board, player, location);
            if (isValidPosition == false)
                return false;

            board[player, location] = piece;
            return true;
        }

        public bool StartDuel()
        {
            bool correctState = state != ERoundState.Preparation;
            if (correctState)
                return false;

            bool canDuel = RoundOperations.CanDuel(in board);
            if (!canDuel)
                return false;

            state = ERoundState.Dueling;
            results = new List<FightResult>(RoundConstants.FIGHTS);
            return true;
        }

        public bool PlayNextFight(out bool hasFinished)
        {
            int currentRound = results.Count;
            hasFinished = currentRound >= RoundConstants.FIGHTS;

            bool correctState = state != ERoundState.Dueling;
            if (correctState)
                return false;

            if (hasFinished)
                return false;

            RoundOperations.PlayNextRound(in board, ref results);

            // Refresh information
            {
                currentRound = results.Count;
                hasFinished = currentRound >= RoundConstants.FIGHTS;
            }

            // Print
            {
                Console.WriteLine(results[currentRound - 1]);
                Console.WriteLine();
            }

            if (hasFinished)
            {
                state = ERoundState.Finished;
                RoundResults roundResults = FightOperations.CalculateResults(results);

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

    public static class RoundOperations
    {
        public static bool IsValidPosition(in EPiece[,] board, int player, int location)
        {
            if (board.Rank <= player)
                return false;

            int locations = board.Length / board.Rank;
            if (locations <= location)
                return false;

            return true;
        }

        public static bool CanDuel(in EPiece[,] board)
        {
            for (int i = 0; i < RoundConstants.PLAYERS; i++)
            {
                for (int j = 0; j < RoundConstants.FIGHTS; j++)
                {
                    if (board[i, j] == EPiece.None)
                        return false;
                }
            }

            return true;
        }

        public static void PlayNextRound(in EPiece[,] board, ref List<FightResult> results)
        {
            int currentRound = results.Count;

            FightData data = new FightData()
            {
                offense = FightOperations.CalculateOffense(results),
                piecePlayer1 = board[0, currentRound],
                piecePlayer2 = board[1, currentRound],
            };

            results.Add(FightOperations.Fight(in data));
        }
    }

    public sealed class RoundResults
    {
        public int points1;
        public int points2;
        public bool offense; // false is player1

        public void AddFightResults(FightResult results)
        {
            points1 += results.player1;
            points2 += results.player2;
            offense ^= results.offenseChange;
        }

        public static int GetWinner(RoundResults execution)
        {
            if (execution.points1 == execution.points2)
                return execution.offense ? 1 : 0;

            return execution.points1 < execution.points2 ? 1 : 0;
        }

        public override string ToString()
        {
            return $"Round state: [{points1} - {points2}], offense in " + (!offense ? "Player 1" : "Player 2");
        }
    }
}
