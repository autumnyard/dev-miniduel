using System.Collections.Generic;

namespace AutumnYard.Miniduel
{
    public interface IBoardEventsListener
    {
        void OnStartedRound(Round round);
        void OnSettedPiece(Round round, int player, int location, EPiece piece);
        void OnUnsettedPiece(Round round, int player, int location);
        void OnStartedDuel(Round round);
        void OnPlayedNextFight(Round round, List<FightResult> fightResult);
        void OnFinished(Round round);
    }
}
