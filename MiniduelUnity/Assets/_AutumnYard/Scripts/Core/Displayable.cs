using UnityEngine;

namespace AutumnYard.Unity.Core
{
    public abstract class Displayable : MonoBehaviour
    {
        protected CanvasGroup _canvasGroup;

        private void Awake()
        {
            if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = GetComponentInChildren<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            ShowInternal();
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            HideInternal();
        }

        protected virtual void ShowInternal() { }
        protected virtual void HideInternal() { }
    }
}
