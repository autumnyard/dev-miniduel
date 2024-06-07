using System;

namespace AutumnYard.Miniduel
{
    public enum EGameState
    {
        Initialization,
        Preparation, Dueling, Results,
        Dispose
    }
    public enum EPiece
    {
        None, Attack, Defense, Parry
    }

    public sealed class RoundExecution
    {
        public int points1;
        public int points2;
        public bool offense; // false is player1

        public void Fight(EPiece piece1, EPiece piece2)
        {
            FightState fight;
            FightResult fightResult;

            if (!offense) // player1
            {
                fight = new FightState(piece1, piece2);
                fightResult = fight.Calculate();
                points1 += fightResult.offensePoints;
                points2 += fightResult.reactionPoints;
            }
            else // player2
            {
                fight = new FightState(piece2, piece1);
                fightResult = fight.Calculate();
                points2 += fightResult.offensePoints;
                points1 += fightResult.reactionPoints;
            }

            offense ^= fightResult.offenseChange;

            string result = fightResult.ToString();
            string result2 = ToString();
            Console.WriteLine(result);
            Console.WriteLine(result2);
            Console.WriteLine();
        }

        public int GetWinner()
        {
            if (points1 == points2)
                return offense ? 1 : 0;

            return points1 < points2 ? 1 : 0;
        }

        public override string ToString()
        {
            if (!offense)
                return $"Round state: [{points1} - {points2}], offense in Player 1";
            else
                return $"Round state: [{points1} - {points2}], offense in Player 2";
        }

        private class FightState
        {
            public EPiece offense;
            public EPiece reaction;

            public FightState(EPiece offense, EPiece reaction)
            {
                this.offense = offense;
                this.reaction = reaction;
            }

            public FightResult Calculate()
            {
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

                switch (offense)
                {
                    case EPiece.Attack:
                        {
                            switch (reaction)
                            {
                                case EPiece.Attack: return new FightResult(1, 1, true);
                                case EPiece.Defense: return new FightResult(0, 1, false);
                                case EPiece.Parry: return new FightResult(0, 2, true);
                                default: return FightResult.Error;
                            }
                        }

                    case EPiece.Defense:
                        {
                            switch (reaction)
                            {
                                case EPiece.Attack: return new FightResult(1, 0, true);
                                case EPiece.Defense: return new FightResult(0, 0, true);
                                case EPiece.Parry: return new FightResult(0, 0, false);
                                default: return FightResult.Error;
                            }
                        }

                    case EPiece.Parry:
                        {
                            switch (reaction)
                            {
                                case EPiece.Attack: return new FightResult(2, 0, false);
                                case EPiece.Defense: return new FightResult(0, 0, true);
                                case EPiece.Parry: return new FightResult(0, 0, false);
                                default: return FightResult.Error;
                            }
                        }

                    default:
                        return FightResult.Error;
                }
            }
        }

        private class FightResult
        {
            public int offensePoints;
            public int reactionPoints;
            public bool offenseChange;

            public static FightResult Error => new FightResult(0, 0, false);

            public FightResult(int sA, int sB, bool offenseChange)
            {
                this.offensePoints = sA;
                this.reactionPoints = sB;
                this.offenseChange = offenseChange;
            }

            public override string ToString()
            {
                if (offenseChange)
                    return $"  (Fight result: the offense earns {offensePoints} points, and the reaction {reactionPoints}. The offense changes side!)";
                else
                    return $"  (Fight result: the offense earns {offensePoints} points, and the reaction {reactionPoints}.)";
            }
        }
    }

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
            //results = new RoundExecution[ROUNDS];
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

    public interface IEventsListener
    {
        void OnAddedPiece(int player, int location, EPiece piece);
        void OnStartDuel();
    }

    public class Game
    {
        public RoundState _state;
        public IEventsListener _listener;

        public Game()
        {
            _state = new RoundState();
            _state.StartGame();
        }

        public void SetListener(IEventsListener listener)
        {
            _listener = listener;
        }

        public bool SetPiece(int player, int location, EPiece piece)
        {
            bool correctState = _state.state != EGameState.Preparation;
            if (correctState)
                return false;

            bool isValidPosition = _state.IsValidPosition(player, location);
            if (isValidPosition == false)
                return false;

            _state.board[player, location] = piece;
            _listener?.OnAddedPiece(player, location, piece);
            return true;
        }

        public bool StartDuel()
        {
            bool correctState = _state.state != EGameState.Preparation;
            if (correctState)
                return false;

            bool canDuel = _state.CanDuel();
            if (!canDuel)
                return false;

            // TODO: Duel
            _state.StartDuel();
            _listener?.OnStartDuel();
            return true;
        }

        public bool PlayNextRound()
        {
            bool correctState = _state.state != EGameState.Dueling;
            if (correctState)
                return false;

            _state.PlayNextRound(out bool hasFinished);
            return true;
        }
    }
}
