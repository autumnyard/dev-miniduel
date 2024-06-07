namespace AutumnYard.Miniduel
{
    public class Game
    {
        public Round _round;

        public Game()
        {
            _round = new Round();
        }

        public bool SetPiece(int player, int location, EPiece piece)
        {
            bool result = _round.SetPiece(player, location, piece);
            if (!result)
                return false;

            return true;
        }

        public bool StartDuel()
        {
            bool result = _round.StartDuel();
            if (!result)
                return false;

            return true;
        }

        public bool PlayNextFight()
        {
            bool result = _round.PlayNextFight(out bool hasFinished);
            if (!result)
                return false;

            return true;
        }
    }
}
