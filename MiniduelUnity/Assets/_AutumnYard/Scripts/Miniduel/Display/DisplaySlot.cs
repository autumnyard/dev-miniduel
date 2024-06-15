using AutumnYard.Miniduel.Unity.Handler;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AutumnYard.Miniduel.Unity.Display
{
    [RequireComponent(typeof(Animator))]
    public sealed class DisplaySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public interface IEventsListener
        {
            void OnSettedPiece(DisplaySlot slot, EPiece piece);
            void OnFinishedFightAnimations(DisplaySlot slot);
        }

        [Header("Configuration")]
        [SerializeField] private Settings _settings;
        [SerializeField] private int _player;
        [SerializeField] private int _fight;

        [Header("References")]
        [SerializeField] private Image _background;
        [SerializeField] private Image _sprite;
        [SerializeField] private TextMeshProUGUI _label;

        [Header("Animations")]
        [SerializeField] private Animator _animator;
        [SerializeField] private RectTransform _tweenContainer;
        [SerializeField] private float _hoverTweenScale = 1.2f;
        [SerializeField] private float _hoverTweenDuration = .4f;

        private EPiece _piece;
        private IEventsListener _listener;

        private void OnValidate()
        {
            //name = $"Slot {_player}-{_fight}";

            if (_label == null) _label = GetComponentInChildren<TextMeshProUGUI>();

            if (_background == null) _background = GetComponent<Image>();
            if (_background == null) _background = GetComponentInChildren<Image>();

            if (_animator == null) _animator = GetComponent<Animator>();
        }

        public void Clear()
        {
            _piece = EPiece.None;
            Refresh();
        }

        public void Set(EPiece piece)
        {
            if (piece == _piece)
                return;

            _piece = piece;
            Refresh();
            _listener?.OnSettedPiece(this, _piece);
        }

        public void ForceSet(EPiece piece)
        {
            _piece = piece;
            Refresh();
        }

        private void Refresh()
        {
            var setting = _settings.GetPieceSetting(_piece);

            if (_piece == EPiece.None)
            {
                _label.alpha = 0f;

                _sprite.color = Color.clear;
            }
            else
            {
                _label.text = setting.text;
                _label.color = setting.color;
                _label.alpha = 1f;

                if (setting.sprite == null)
                {
                    _sprite.color = Color.clear;
                }
                else
                {
                    _sprite.sprite = setting.sprite;
                    _sprite.color = Color.white;
                    //_sprite.SetAllDirty();
                    //_sprite.color = setting.color;
                }
            }
        }

        #region Event Listening

        public void SetListener(IEventsListener listener)
        {
            _listener = listener;
        }

        public void UnsetListener()
        {
            _listener = null;
        }


        public void OnFinishedAnimation()
        {
            _listener?.OnFinishedFightAnimations(this);
        }

        #endregion // Event Listening


        public void OnDrop(PointerEventData eventData)
        {
            //Debug.Log($" ---- OnDrop");

            if (eventData.pointerDrag == null)
                return;

            var dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();
            if (dragPiece == null)
                return;

            StopTween();
            GameHandler.Instance.SetPiece(_player, _fight, dragPiece.Piece);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;

            var dragPiece = eventData.pointerDrag.GetComponent<DragPiece>();
            if (dragPiece == null)
                return;

            StopTweenInstant();
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
        private void StopTweenInstant()
        {
            _tweenContainer.DOKill();
            _tweenContainer.localScale = Vector3.one;
        }

        public override string ToString()
        {
            return $"DisplaySlot {_player} {_fight}: {_piece}";
        }
    }
}
