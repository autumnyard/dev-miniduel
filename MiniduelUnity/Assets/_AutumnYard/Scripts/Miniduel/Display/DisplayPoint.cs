using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private TextMeshProUGUI _pointsLabel;

        public void Set(int points)
        {
            _container.SetActive(true);
            _pointsLabel.text = $"+{points}";
        }

        public void Unset()
        {
            _container.SetActive(false);
            _pointsLabel.text = $"";
        }
    }
}
