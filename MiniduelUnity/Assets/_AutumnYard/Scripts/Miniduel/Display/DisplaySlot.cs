using AutumnYard.Miniduel.Unity.Handler;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplaySlot : MonoBehaviour,
        IDropHandler
    {
        [SerializeField] private int _player;
        [SerializeField] private int _fight;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _label;

        private void OnValidate()
        {
            name = $"Slot {_player}-{_fight}";

            if (_label == null) _label = GetComponentInChildren<TextMeshProUGUI>();

            if (_image == null) _image = GetComponent<Image>();
            if (_image == null) _image = GetComponentInChildren<Image>();
        }

        public void Set(EPiece piece)
        {
            if (piece == EPiece.None)
            {
                _label.alpha = 0f;
                _image.color = Color.gray;
            }
            else
            {
                _label.text = DisplayUtils.GetPieceWithColor(piece);
                _label.alpha = 1f;
                _image.color = Color.white;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            DragPiece dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();

            if (dragPiece != null)
            {
                GameHandler.Instance.SetPiece(_player, _fight, dragPiece.Piece);
            }

            //Debug.Log($" ---- OnDrop {eventData}");
        }
    }
}
