using AutumnYard.Miniduel.Unity.Handler;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutumnYard.Miniduel.Unity.Display
{
    [RequireComponent(typeof(Animator))]
    public class DisplaySlot : MonoBehaviour,
        IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int _player;
        [SerializeField] private int _fight;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _label;

        [Header("Animator")]
        [SerializeField] private Animator _animator;

        [Header("Tweening")]
        [SerializeField] private RectTransform _tweenContainer;
        [SerializeField] private float _hoverTweenScale = 1.2f;
        [SerializeField] private float _hoverTweenDuration = .4f;

        private EPiece _piece;

        private void OnValidate()
        {
            //name = $"Slot {_player}-{_fight}";

            if (_label == null) _label = GetComponentInChildren<TextMeshProUGUI>();

            if (_image == null) _image = GetComponent<Image>();
            if (_image == null) _image = GetComponentInChildren<Image>();

            if (_animator == null) _animator = GetComponent<Animator>();
        }

        public void Set(EPiece piece)
        {
            _piece = piece;
            if (_piece == EPiece.None)
            {
                _label.alpha = 0f;
                _image.color = Color.gray;
            }
            else
            {
                _label.text = DisplayUtils.GetPieceWithColor(_piece);
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

            StopTween();
            _piece = dragPiece.Piece;
            GameHandler.Instance.SetPiece(_player, _fight, _piece);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();
            if (dragPiece == null)
                return;

            PlayTweenHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();
            if (dragPiece == null)
                return;

            StopTween();

        }

        [ContextMenu("Play animation")]
        public void PlayAnimation()
        {
            _animator.SetTrigger(_piece.ToString());
        }

        [ContextMenu("Reset animation")]
        public void ResetAnimation()
        {
            _animator.SetTrigger("Clear");
        }


        private void PlayTweenHover()
        {
            _tweenContainer.DOKill();
            _tweenContainer.localScale = Vector3.one;
            _tweenContainer.DOScale(Vector3.one * _hoverTweenScale, _hoverTweenDuration)
                .SetEase(Ease.OutQuad);
        }

        private void StopTween()
        {
            _tweenContainer.DOKill();
            _tweenContainer.DOScale(Vector3.one, _hoverTweenDuration)
                .SetEase(Ease.OutQuad);
        }
    }
}
