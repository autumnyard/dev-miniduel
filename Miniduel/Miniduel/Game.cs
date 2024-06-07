namespace AutumnYard.Miniduel
{
    public class Game
    {
        public RoundState _round;
        public IEventsListener _listener;

        public Game()
        {
            _round = new RoundState();
            _round.StartGame();
        }

        public void SetListener(IEventsListener listener)
        {
            _listener = listener;
        }

        public bool SetPiece(int player, int location, EPiece piece)
        {
            bool result = _round.SetPiece(player, location, piece);
            if (!result)
                return false;

            _listener?.OnAddedPiece(player, location, piece);
            return true;
        }

        public bool StartDuel()
        {
            bool result = _round.StartDuel();
            if (!result)
                return false;

            _listener?.OnStartDuel();
            return true;
        }

        public bool PlayNextFight()
        {
            bool result = _round.PlayNextFight(out bool hasFinished);
            if (!result)
                return false;

            _listener?.OnPlayedFight(_round);

            if (hasFinished)
            {
                _listener?.OnFinishedRound(_round);
            }

            return true;
        }
    }
}
