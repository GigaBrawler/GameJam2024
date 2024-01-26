using UnityEngine;

namespace Core
{
    public class Singleton<T> : MonoBehaviour where T:Component
    {
        private static T _instance;
        public static T Instance
        {
            get {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance != null) return _instance;
                var gameObject = new GameObject();
                _instance = gameObject.AddComponent<T>();
                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null) {
                _instance = this as T;
            } else {
                if(_instance != this)
                    Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}
