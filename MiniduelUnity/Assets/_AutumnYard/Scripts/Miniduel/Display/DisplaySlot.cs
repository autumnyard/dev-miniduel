using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplaySlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        public void Set(EPiece piece)
        {
            _label.text = piece.ToString();
        }
    }
}
