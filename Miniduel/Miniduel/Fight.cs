namespace AutumnYard.Miniduel
{
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
