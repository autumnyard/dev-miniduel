using TMPro;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplaySlot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        public void Set(EPiece piece)
        {
            Color color;
            switch (piece)
            {
                default:
                case EPiece.None:
                    color = Color.gray;
                    break;

                case EPiece.Attack:
                    color = Color.red;
                    break;

                case EPiece.Defense:
                    color = Color.green;
                    break;

                case EPiece.Parry:
                    color = Color.blue;
                    break;
            }
            string hex = ColorUtility.ToHtmlStringRGB(color);
            _label.text = $"<color=#{hex}>{piece}</color>";
        }
    }
}
