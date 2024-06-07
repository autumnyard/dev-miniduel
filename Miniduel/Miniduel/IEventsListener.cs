namespace AutumnYard.Miniduel
{
    public interface IEventsListener
    {
        void OnAddedPiece(int player, int location, EPiece piece);
        void OnStartDuel();
        void OnPlayedFight(RoundState round);
        void OnFinishedRound(RoundState round);
    }
}
