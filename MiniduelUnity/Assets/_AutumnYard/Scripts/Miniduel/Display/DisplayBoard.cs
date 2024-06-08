using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayBoard : MonoBehaviour
    {
        [SerializeField] private DisplayPanel[] _panels;
        [SerializeField] private DisplaySlot[] _slotsPlayer1;
        [SerializeField] private DisplaySlot[] _slotsPlayer2;

        private DisplaySlot GetSlot(int player, int location)
        {
            if (player == 0)
                return _slotsPlayer1[location];
            else
                return _slotsPlayer2[location];
        }

        //public void OnStartedRound()
        //{
        //}

        public void OnSettedPiece(Round round, int player, int location, EPiece piece)
        {
            GetSlot(player, location).Set(piece);
        }

        //public void OnStartedDuel(Round round)
        //{
        //}

        //public void OnPlayedNextFight(Round round, List<FightResult> fightResult)
        //{
        //}

        //public void OnFinished(Round round)
        //{
        //}
    }
}
