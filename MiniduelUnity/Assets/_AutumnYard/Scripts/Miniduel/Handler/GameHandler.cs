namespace AutumnYard.Miniduel.Unity.Handler
{
    public class GameHandler
    {
        private static GameHandler _instance;

        public static GameHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameHandler();

                return _instance;
            }
        }

        private Game _game;

        public Game Game => _game;


        public void SetGame(IBoardEventsListener listener)
        {
            _game = new Game(listener);
        }

        public void SetPiece(int player, int location, EPiece piece)
        {
            _game.SetPiece(player, location, piece);
        }
    }
}
