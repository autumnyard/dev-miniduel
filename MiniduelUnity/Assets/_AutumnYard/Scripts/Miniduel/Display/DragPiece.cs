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
        [Header("Configuration")]
        [SerializeField] private Settings _settings;
        [SerializeField] private EPiece _piece;

        [Header("External references")]
        [SerializeField] private Canvas _canvas;

        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _pieceLabel;

        [Header("Animation")]
        [SerializeField] private RectTransform _tweenContainer;
        [SerializeField] private Vector3 _idleTweenRotation = new Vector3(0, 0, 4);
        [SerializeField] private float _idleTweenDuration = .4f;


        private RectTransform _transform;
        private Vector2 _defaultPosition;

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
            PlayTweenIdle();
        }

        private void PlayTweenIdle()
        {
            _tweenContainer.localEulerAngles = -_idleTweenRotation;
            _tweenContainer.DORotate(_idleTweenRotation, _idleTweenDuration)
                .SetEase(Ease.InOutQuint)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void StopTween()
        {
            _tweenContainer.DOKill();
            _tweenContainer.localEulerAngles = Vector3.zero;
        }

        #region Drag & Drop

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log($" ---- OnBeginDrag {eventData}");
            StopTween();
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
            PlayTweenIdle();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log($" ---- OnPointerDown {eventData}");
        }

        #endregion // Drag & Drop

    }
}
