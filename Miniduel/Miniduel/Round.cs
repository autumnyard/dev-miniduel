using System;
using System.Collections.Generic;

namespace AutumnYard.Miniduel
{
    public static class RoundConstants
    {
        public const int PLAYERS = 2;
        public const int FIGHTS = 3;
        private const string PLAYER_1 = "Player 1";
        private const string PLAYER_2 = "Player 2";

        public static string GetPlayer(int integer) => (integer == 0 ? PLAYER_1 : PLAYER_2);
        public static string GetPlayer(bool boolean) => (!boolean ? PLAYER_1 : PLAYER_2);
    }

    public class Round
    {
        private ERoundState state;
        private EPiece[,] board;
        private List<FightResult> results;

        public Round()
        {
            board = new EPiece[RoundConstants.PLAYERS, RoundConstants.FIGHTS];
            results = null;
            state = ERoundState.Preparation;
        }

        public bool SetPiece(int player, int location, EPiece piece)
        {
            // Check requirements
            {
                if (state != ERoundState.Preparation)
                    return false;

                bool isValidPosition = RoundOperations.IsValidPosition(in board, player, location);
                if (isValidPosition == false)
                    return false;
            }

            // Apply operation
            {
                RoundOperations.SetPiece(piece, player, location, ref board);
            }

            // Consequences
            {
            }

            return true;
        }

        public bool ChangeStateToDueling()
        {
            // Check requirements
            {
                bool correctState = state == ERoundState.Preparation;
                if (!correctState)
                    return false;

                bool canDuel = RoundOperations.CanDuel(in board);
                if (!canDuel)
                    return false;
            }

            // Apply operation
            {
                state = ERoundState.Dueling;
            }

            // Consequences
            {
                results = new List<FightResult>(RoundConstants.FIGHTS);
            }

            return true;
        }

        public bool PlayNextFight()
        {
            // Check requirements
            {
                bool correctState = state != ERoundState.Dueling;
                if (correctState)
                    return false;

                bool hasFinished = RoundOperations.CanFinish(in results);
                if (hasFinished)
                    return false;
            }

            // Apply operation
            {
                RoundOperations.PlayNextRound(in board, ref results);
            }

            // Consequences
            {
                Console.WriteLine(RoundOperations.GetLastRoundResult(results));
            }

            return true;
        }

        public bool ChangeStateToFinished()
        {
            // Check requirements
            {
                bool correctState = state == ERoundState.Dueling;
                if (!correctState)
                    return false;

                bool canFinish = RoundOperations.CanFinish(in results);
                if (!canFinish)
                    return false;
            }

            // Apply operation
            {
                state = ERoundState.Finished;
            }

            // Consequences
            {
                Console.WriteLine(RoundOperations.GetFinishResults(results));
            }

            return true;
        }

    }

    public static class RoundOperations
    {
        public static void SetPiece(in EPiece piece, int player, int location, ref EPiece[,] board)
        {
            board[player, location] = piece;
        }

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

        public static bool CanFinish(in List<FightResult> results)
        {
            return results.Count >= RoundConstants.FIGHTS;
        }

        public static RoundResult GetRoundResult(in List<FightResult> results)
        {
            RoundResult roundResults = new RoundResult();
            foreach (var result in results)
            {
                roundResults.AddFightResults(result);
            }
            return roundResults;
        }

        public static string GetLastRoundResult(in List<FightResult> results)
        {
            return results[results.Count - 1].ToString();
        }

        public static string GetFinishResults(in List<FightResult> results)
        {
            RoundResult roundResults = GetRoundResult(results);
            int winner = RoundResult.GetWinner(roundResults);
            return
                $"\n" +
                $"\n-------------- FINISHED!! --------------" +
                $"\n  And the winner was " + RoundConstants.GetPlayer(winner) +
                $"\n  Results: {roundResults}" +
                $"\n----------------------------------------";
        }
    }

    public sealed class RoundResult
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

        public static int GetWinner(RoundResult execution)
        {
            if (execution.points1 == execution.points2)
                return execution.offense ? 1 : 0;

            return execution.points1 < execution.points2 ? 1 : 0;
        }

        public override string ToString()
        {
            return $"Round state: [{points1} - {points2}], offense in " + RoundConstants.GetPlayer(offense);
        }
    }
}
