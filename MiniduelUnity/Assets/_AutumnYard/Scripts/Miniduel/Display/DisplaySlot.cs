using AutumnYard.Miniduel.Unity.Handler;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplaySlot : MonoBehaviour,
        IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int _player;
        [SerializeField] private int _fight;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _label;

        [Header("Animation")]
        [SerializeField] private RectTransform _animationContainer;
        [SerializeField] private float _hoverAnimationScale = 1.2f;
        [SerializeField] private float _hoverAnimationDuration = .4f;

        private void OnValidate()
        {
            //name = $"Slot {_player}-{_fight}";

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
            //Debug.Log($" ---- OnDrop");

            if (eventData.pointerDrag == null)
                return;

            var dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();
            if (dragPiece == null)
                return;

            StopAnimation();
            GameHandler.Instance.SetPiece(_player, _fight, dragPiece.Piece);

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();
            if (dragPiece == null)
                return;

            PlayAnimationHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();
            if (dragPiece == null)
                return;

            StopAnimation();

        }

        private void PlayAnimationHover()
        {
            _animationContainer.DOKill();
            _animationContainer.localScale = Vector3.one;
            _animationContainer.DOScale(Vector3.one * _hoverAnimationScale, _hoverAnimationDuration)
                .SetEase(Ease.OutQuad);
        }

        private void StopAnimation()
        {
            _animationContainer.DOKill();
            _animationContainer.DOScale(Vector3.one, _hoverAnimationDuration)
                .SetEase(Ease.OutQuad);
        }
    }
}
