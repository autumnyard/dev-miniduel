using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AutumnYard.Miniduel.Unity.Display
{
    public sealed class DragPiece : MonoBehaviour,
        IPointerDownHandler,
        IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _pieceLabel;
        [SerializeField] private EPiece _piece;
        private RectTransform _transform;
        private Vector2 _defaultPosition;

        [Header("Animation")]
        [SerializeField] private RectTransform _animationContainer;
        [SerializeField] private Vector3 _idleAnimationRotation = new Vector3(0, 0, 5);
        [SerializeField] private float _idleAnimationDuration = .6f;

        public EPiece Piece => _piece;

        private void OnValidate()
        {
            if (_piece == EPiece.None)
                _piece = EPiece.Attack;

            //name = $"DragPiece ({_piece})";

            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            if (_pieceLabel == null) _pieceLabel = GetComponentInChildren<TextMeshProUGUI>();

            if (_pieceLabel != null)
                _pieceLabel.text = DisplayUtils.GetPieceWithColor(_piece);

        }

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
            _defaultPosition = _transform.anchoredPosition;
            if (_pieceLabel == null) _pieceLabel = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _pieceLabel.text = DisplayUtils.GetPieceWithColor(_piece);
            PlayAnimationIdle();
        }

        private void PlayAnimationIdle()
        {
            _animationContainer.localEulerAngles = -_idleAnimationRotation;
            _animationContainer.DORotate(_idleAnimationRotation, _idleAnimationDuration)
                .SetEase(Ease.InOutQuint)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void StopAnimation()
        {
            _animationContainer.DOKill();
            _animationContainer.localEulerAngles = Vector3.zero;
        }

        #region Drag & Drop

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log($" ---- OnBeginDrag {eventData}");
            StopAnimation();
            _canvasGroup.alpha = .7f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log($" ----        OnDrag {eventData}");
            _transform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //Debug.Log($" ---- OnEndDrag {eventData}");
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _transform.anchoredPosition = _defaultPosition;
            PlayAnimationIdle();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log($" ---- OnPointerDown {eventData}");
        }

        #endregion // Drag & Drop

    }
}
