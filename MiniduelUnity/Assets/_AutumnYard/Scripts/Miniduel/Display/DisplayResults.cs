using AutumnYard.Unity.Core;
using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayResults : Displayable
    {
        public class DTO
        {
            public int points1;
            public int points2;
            public bool offense;
            public int winner;
            public bool hasFinished;

            public string PointsPlayer1 => $"Player1: {points1} points";
            public string PointsPlayer2 => $"Player1: {points2} points";
            public string Offense => "Offense in " + (!offense ? "Player 1" : "Player 2");
            public string Winner => "Winner is" + (winner == 0 ? "Player 1" : "Player 2") + "!!";
        }

        [SerializeField] private TextMeshProUGUI _point1Label;
        [SerializeField] private TextMeshProUGUI _point2Label;
        [SerializeField] private TextMeshProUGUI _offenseLabel;
        [SerializeField] private TextMeshProUGUI _winnerLabel;

        private DTO _dto;

        public void Set(DTO dto)
        {
            _dto = dto;
            Refresh();
        }

        public void Refresh()
        {
            if (_dto == null)
                return;

            _point1Label.text = _dto.PointsPlayer1;
            _point2Label.text = _dto.PointsPlayer2;
            _offenseLabel.text = _dto.Offense;

            if (_dto.hasFinished)
            {
                _winnerLabel.text = _dto.Winner;
                _winnerLabel.gameObject.SetActive(true);
            }
            else
            {
                _winnerLabel.gameObject.SetActive(false);
            }
        }
    }
}
