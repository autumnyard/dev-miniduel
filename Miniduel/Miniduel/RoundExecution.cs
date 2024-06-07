using System;

namespace AutumnYard.Miniduel
{
    public sealed class RoundExecution
    {
        private int points1;
        private int points2;
        private bool offense; // false is player1

        internal static void Fight(RoundExecution execution, EPiece piece1, EPiece piece2)
        {
            FightState fight = new FightState(piece1, piece2, !execution.offense);
            FightResult fightResult = fight.Calculate();

            execution.Refresh(fightResult);

            Console.WriteLine(fightResult);
            Console.WriteLine(execution);
            Console.WriteLine();
        }

        private void Refresh(FightResult results)
        {
            points1 += results.player1;
            points2 += results.player2;
            offense ^= results.offenseChange;
        }

        public static int GetWinner(RoundExecution execution)
        {
            if (execution.points1 == execution.points2)
                return execution.offense ? 1 : 0;

            return execution.points1 < execution.points2 ? 1 : 0;
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
            private bool opposite = false;

            public FightState(EPiece player1, EPiece player2, bool isOffenseInP1)
            {
                if (isOffenseInP1)
                {
                    offense = player1;
                    reaction = player2;
                    opposite = false;
                }
                else
                {
                    offense = player2;
                    reaction = player1;
                    opposite = true;
                }
            }

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
                                case EPiece.Attack: return new FightResult(1, 1, true, opposite);
                                case EPiece.Defense: return new FightResult(0, 1, false, opposite);
                                case EPiece.Parry: return new FightResult(0, 2, true, opposite);
                            }
                        }
                        break;

                    case EPiece.Defense:
                        {
                            switch (reaction)
                            {
                                case EPiece.Attack: return new FightResult(1, 0, true, opposite);
                                case EPiece.Defense: return new FightResult(0, 0, true, opposite);
                                case EPiece.Parry: return new FightResult(0, 0, false, opposite);
                            }
                        }
                        break;

                    case EPiece.Parry:
                        {
                            switch (reaction)
                            {
                                case EPiece.Attack: return new FightResult(2, 0, false, opposite);
                                case EPiece.Defense: return new FightResult(0, 0, true, opposite);
                                case EPiece.Parry: return new FightResult(0, 0, false, opposite);
                            }
                        }
                        break;
                }

                return FightResult.Error;
            }
        }

        private class FightResult
        {
            public int player1;
            public int player2;
            public bool offenseChange;

            public static FightResult Error => new FightResult(0, 0, false, false);

            public FightResult(int sA, int sB, bool offenseChange, bool opposite)
            {
                if (opposite)
                {
                    this.player2 = sA;
                    this.player1 = sB;
                }
                else
                {
                    this.player1 = sA;
                    this.player2 = sB;
                }
                this.offenseChange = offenseChange;
            }

            public FightResult(int player1, int player2, bool offenseChange)
            {
                this.player1 = player1;
                this.player2 = player2;
                this.offenseChange = offenseChange;
            }

            public override string ToString()
            {
                if (offenseChange)
                    return $"  (Fight result: Player1 earns {player1} points, and Player2 {player2}. The offense changes side!)";
                else
                    return $"  (Fight result: Player1 earns {player2} points, and Player2 {player2}.)";
            }
        }
    }
}
