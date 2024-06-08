using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayResults : MonoBehaviour
    {
        public class DTO
        {
            public string points1;
            public string points2;
            public string offense;
            public string winner = string.Empty;
        }

        [SerializeField] private TextMeshProUGUI _point1Label;
        [SerializeField] private TextMeshProUGUI _point2Label;
        [SerializeField] private TextMeshProUGUI _offenseLabel;
        [SerializeField] private TextMeshProUGUI _winnerLabel;

        private CanvasGroup _canvasGroup;
        private DTO _dto;

        private void Awake()
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = GetComponentInChildren<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
        }

        public void Set(DTO dto)
        {
            _dto = dto;
            Refresh();
        }

        public void Refresh()
        {
            if (_dto == null)
                return;

            _point1Label.text = _dto.points1;
            _point2Label.text = _dto.points2;
            _offenseLabel.text = _dto.offense;

            if (_dto.winner.Equals(string.Empty))
            {
                _winnerLabel.gameObject.SetActive(false);
            }
            else
            {
                _winnerLabel.text = _dto.winner;
                _winnerLabel.gameObject.SetActive(true);
            }
        }

    }
}
