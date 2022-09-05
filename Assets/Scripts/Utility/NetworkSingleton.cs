using Photon.Pun;

namespace Utility {
    public class NetworkSingleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks {
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

            _instance = this as T;
        }
        #endregion
    }
}