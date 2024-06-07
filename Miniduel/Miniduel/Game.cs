namespace AutumnYard.Miniduel
{
    public class Game
    {
        public Round _round;

        public Game()
        {
            _round = new Round();
        }

        public Game(IBoardEventsListener listener)
        {
            _round = new Round(listener);
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
            bool result = _round.ChangeStateToDueling();
            if (!result)
                return false;

            return true;
        }

        public bool PlayNextFight()
        {
            bool result = _round.PlayNextFight();
            if (!result)
                return false;

            return true;
        }

        public bool FinishRound()
        {
            bool result = _round.ChangeStateToFinished();
            if (!result)
                return false;

            return true;
        }
    }
}
