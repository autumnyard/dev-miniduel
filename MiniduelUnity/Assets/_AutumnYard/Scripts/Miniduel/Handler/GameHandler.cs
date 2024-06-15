using AutumnYard.Unity.Core;

namespace AutumnYard.Miniduel.Unity.Handler
{
    public class GameHandler : Singleton<GameHandler>
    {
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
