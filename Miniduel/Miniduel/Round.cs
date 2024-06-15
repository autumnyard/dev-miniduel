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
        private IBoardEventsListener _listener;

        public ERoundState State => state;

        public BoardDTO GetBoard
        {
            get
            {
                return new BoardDTO()
                {
                    players = RoundConstants.PLAYERS,
                    fights = RoundConstants.FIGHTS,
                    board = board,
                };
            }
        }

        public FightResultDTO GetFightResult
        {
            get
            {
                return new FightResultDTO()
                {
                    fightResults = results,
                };
            }
        }
        public RoundFinishedDTO GetRoundFinished
        {
            get
            {
                RoundResult roundResults = RoundOperations.GetRoundResult(results);
                int winner = RoundResult.GetWinner(roundResults);
                return new RoundFinishedDTO()
                {
                    points1 = roundResults.points1,
                    points2 = roundResults.points2,
                    offense = roundResults.offense,
                    winner = winner
                };
            }
        }

        public Round()
        {
            board = new EPiece[RoundConstants.PLAYERS, RoundConstants.FIGHTS];
            results = null;
            state = ERoundState.Preparation;
            StartRound();
        }

        public Round(IBoardEventsListener listener)
        {
            board = new EPiece[RoundConstants.PLAYERS, RoundConstants.FIGHTS];
            results = null;
            _listener = listener;
            StartRound();
        }

        private void StartRound()
        {
            state = ERoundState.Preparation;
            _listener?.OnStartedRound(this);
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
                if (piece == EPiece.None)
                    _listener?.OnUnsettedPiece(this, player, location);
                else
                    _listener?.OnSettedPiece(this, player, location, piece);
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
                _listener?.OnStartedDuel(this);
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
                _listener?.OnPlayedNextFight(this, results);
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
                _listener?.OnFinished(this);
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
                offense = CalculateOffense(results),
                piecePlayer1 = board[0, currentRound],
                piecePlayer2 = board[1, currentRound],
            };

            results.Add(Fight(in data));
        }

        public static FightResult Fight(in FightData data)
        {
            EPiece offense;
            EPiece reaction;
            bool opposite = false;

            if (!data.offense)
            {
                offense = data.piecePlayer1;
                reaction = data.piecePlayer2;
                opposite = false;
            }
            else
            {
                offense = data.piecePlayer2;
                reaction = data.piecePlayer1;
                opposite = true;
            }

            // Rules:
            // A vs A -> + 1/1, change offense
            // A vs D -> + 0/1
            // A vs P -> + 0/2, change offense
            // D vs A -> + 1/0, change offense
            // D vs D -> + 0/0, change offense
            // D vs P -> + 0/0
            // P vs A -> + 2/0
            // P vs D -> + 0/0, change offense
            // P vs P -> + 0/0

            // TODO: Is there any way I could read this from a file outside the code, so I don't have to recompile to change them?
            // TODO: The opposite bool is transient, so I have to rebuild this rule dictionary every single time the method is called
            Dictionary<(EPiece, EPiece), FightResult> rules = new Dictionary<(EPiece, EPiece), FightResult>()
            {
                { (EPiece.Attack,   EPiece.Attack),     new FightResult(1, 1, true, opposite) },
                { (EPiece.Attack,   EPiece.Defense),    new FightResult(0, 1, false, opposite) },
                { (EPiece.Attack,   EPiece.Parry),      new FightResult(0, 2, true, opposite) },

                { (EPiece.Defense,  EPiece.Attack),     new FightResult(1, 0, true, opposite) },
                { (EPiece.Defense,  EPiece.Defense),    new FightResult(0, 0, true, opposite) },
                { (EPiece.Defense,  EPiece.Parry),      new FightResult(0, 0, false, opposite) },

                { (EPiece.Parry,    EPiece.Attack),     new FightResult(2, 0, false, opposite) },
                { (EPiece.Parry,    EPiece.Defense),    new FightResult(0, 0, true, opposite) },
                { (EPiece.Parry,    EPiece.Parry),      new FightResult(0, 0, false, opposite) },
            };

            var move = (offense, reaction);

            if (!rules.ContainsKey(move))
                return FightResult.Error;

            return rules[move];
        }

        public static bool CalculateOffense(IEnumerable<FightResult> results)
        {
            bool offense = false;
            foreach (var result in results)
            {
                offense ^= result.offenseChange;
            }

            return offense;
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
