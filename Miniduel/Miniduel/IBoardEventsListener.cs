using System.Collections.Generic;

namespace AutumnYard.Miniduel
{
    public interface IBoardEventsListener
    {
        void OnSettedPiece(int player, int location, EPiece piece);
        void OnStartedDuel();
        void OnPlayedNextFight(List<FightResult> fightResult);
        void OnFinished();
    }
}
