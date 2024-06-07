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
            bool correctState = _round.state != EGameState.Preparation;
            if (correctState)
                return false;

            bool isValidPosition = _round.IsValidPosition(player, location);
            if (isValidPosition == false)
                return false;

            _round.board[player, location] = piece;
            _listener?.OnAddedPiece(player, location, piece);
            return true;
        }

        public bool StartDuel()
        {
            bool correctState = _round.state != EGameState.Preparation;
            if (correctState)
                return false;

            bool canDuel = _round.CanDuel();
            if (!canDuel)
                return false;

            // TODO: Duel
            _round.StartDuel();
            _listener?.OnStartDuel();
            return true;
        }

        public bool PlayNextFight()
        {
            bool correctState = _round.state != EGameState.Dueling;
            if (correctState)
                return false;

            _round.PlayNextRound(out bool hasFinished);
            _listener?.OnPlayedFight(_round);

            if (hasFinished)
            {
                _listener?.OnFinishedRound(_round);
            }

            return true;
        }
    }
}
