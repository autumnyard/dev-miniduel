using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayOffense : MonoBehaviour
    {
        [SerializeField] private GameObject _offenseIndicator;
        [SerializeField] private GameObject _offenseChangeIndicator;

        public void Set(bool hasOffense, bool offenseChanged)
        {
            _offenseIndicator.SetActive(hasOffense);
            _offenseChangeIndicator.SetActive(hasOffense && offenseChanged);
        }

        public void Set()
        {
            Set(true, false);
        }

        public void Unset()
        {
            Set(false, false);
        }
    }
}
