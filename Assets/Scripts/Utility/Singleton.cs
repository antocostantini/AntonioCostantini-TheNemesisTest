using UnityEngine;

namespace Utility {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        #region Private Variables
        private static T _instance;
        #endregion

        #region Properties
        public static T Instance => _instance;
        #endregion
        
        #region Behaviour Callbacks
        protected virtual void Awake() {
            if (_instance != null) {
                Destroy(this.gameObject);
                return;
            }

            //DontDestroyOnLoad(_instance.gameObject);
            _instance = this as T;
        }
        #endregion
    }
}