namespace AutumnYard.Miniduel
{
    public sealed class RoundResults
    {
        public int points1;
        public int points2;
        public bool offense; // false is player1

        public void AddFightResults(FightResult results)
        {
            points1 += results.player1;
            points2 += results.player2;
            offense ^= results.offenseChange;
        }

        public static int GetWinner(RoundResults execution)
        {
            if (execution.points1 == execution.points2)
                return execution.offense ? 1 : 0;

            return execution.points1 < execution.points2 ? 1 : 0;
        }

        public override string ToString()
        {
            return $"Round state: [{points1} - {points2}], offense in " + (!offense ? "Player 1" : "Player 2");
        }
    }
}
