using AutumnYard.Unity.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayResults : Displayable
    {
        public class DTO
        {
            public int round;
            public int points1;
            public int points2;
            public bool offense;
            public int winner;
            public bool hasFinished;

            public string PointsPlayer1 => $"Player1: <b>{points1}</b>";
            public string PointsPlayer2 => $"Player2: <b>{points2}</b>";
            public string Offense => (!offense ? "Player 1" : "Player 2") + " has offense";
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
                StartTweenWinner();
            }
            else
            {
                _winnerLabel.gameObject.SetActive(false);
                StopTween();
            }
        }

        private void StartTweenWinner()
        {
            Vector3 scale = Vector3.one * 1.1f;
            _winnerLabel.rectTransform.localScale = Vector3.one;
            _winnerLabel.rectTransform.DOScale(scale, .3f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);

            Vector3 rotation = new Vector3(0, 0, 2);
            _winnerLabel.rectTransform.localEulerAngles = -rotation;
            _winnerLabel.rectTransform.DOLocalRotate(rotation, .4f)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void StopTween()
        {
            _winnerLabel.rectTransform.DOKill();
            _winnerLabel.rectTransform.localScale = Vector3.one;
            _winnerLabel.rectTransform.localEulerAngles = Vector3.zero;
        }
    }
}
