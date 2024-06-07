using System.Collections.Generic;

namespace AutumnYard.Miniduel
{
    public static class FightOperations
    {
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

            var asd = (offense, reaction);

            if (!rules.ContainsKey(asd))
                return FightResult.Error;

            return rules[asd];
        }


        public static bool CalculateOffense(in FightResult[] results, in int index)
        {
            bool offense = false;
            for (int i = 0; i < index; i++)
            {
                offense ^= results[i].offenseChange;
            }

            return offense;
        }

        public static RoundResults CalculateResults(in FightResult[] results, in int index)
        {
            RoundResults roundResults = new RoundResults();
            for (int i = 0; i < index; i++)
            {
                roundResults.AddFightResults(results[i]);
            }
            return roundResults;
        }
    }


    public sealed class FightData
    {
        public bool offense;
        public EPiece piecePlayer1;
        public EPiece piecePlayer2;
    }
    public class FightResult
    {
        public int player1;
        public int player2;
        public bool offenseChange;

        public static FightResult Error => new FightResult(0, 0, false, false);

        public FightResult(int offencePoints, int defensePoints, bool offenseChange, bool opposite)
        {
            if (opposite)
            {
                this.player1 = defensePoints;
                this.player2 = offencePoints;
            }
            else
            {
                this.player1 = offencePoints;
                this.player2 = defensePoints;
            }
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
