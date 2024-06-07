using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Display
{
    public class SlotDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        public void Set(EPiece piece)
        {
            _label.text = piece.ToString();
        }
    }
}
