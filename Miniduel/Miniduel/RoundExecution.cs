using System;

namespace AutumnYard.Miniduel
{
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
}
