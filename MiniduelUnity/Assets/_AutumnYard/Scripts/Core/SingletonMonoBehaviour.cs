using UnityEngine;

namespace AutumnYard.Unity.Core
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : UnityEngine.Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var asd = GameObject.FindFirstObjectByType<T>(FindObjectsInactive.Include);
                    if (asd == null)
                    {
                        //var go = new GameObject("Audio");
                        //asd = go.AddComponent<T>();
                        Debug.LogError($"Couldn't find instance of {typeof(T)} in scene.");
                        return null;
                    }

                    _instance = asd;
                }

                return _instance;
            }
        }
    }
}
