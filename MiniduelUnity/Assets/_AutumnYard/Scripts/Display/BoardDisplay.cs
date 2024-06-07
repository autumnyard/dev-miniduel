using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Display
{
    public class BoardDisplay : MonoBehaviour, IBoardEventsListener
    {
        [SerializeField] private TextMeshProUGUI _roundStateLabel;
        [SerializeField] private PanelDisplay[] _panels;
        [SerializeField] private SlotDisplay[] _slotsPlayer1;
        [SerializeField] private SlotDisplay[] _slotsPlayer2;



        #region Display

        private SlotDisplay GetSlot(int player, int location)
        {
            if (player == 0)
                return _slotsPlayer1[location];
            else
                return _slotsPlayer2[location];
        }

        public void OnSettedPiece(int player, int location, EPiece piece)
        {
            Debug.Log("OnSettedPiece");
            GetSlot(player, location).Set(piece);
            _roundStateLabel.text = "preparing";
        }

        public void OnStartedDuel()
        {
            Debug.Log("OnStartedDuel");
            _roundStateLabel.text = "dueling";
        }

        public void OnPlayedNextFight(List<FightResult> fightResult)
        {
            Debug.Log("OnPlayedNextFight");
        }

        public void OnFinished()
        {
            Debug.Log("OnFinished");
            _roundStateLabel.text = "finished";
        }

        #endregion // Display
    }
}
