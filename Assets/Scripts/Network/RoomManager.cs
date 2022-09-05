using Menu;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network {
    [RequireComponent(typeof(PhotonView))]
    public class RoomManager : MonoBehaviourPunCallbacks {
        #region Properties
        public TeamSelector.Team Team { get; set; }
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            DontDestroyOnLoad(this);
        }

        public override void OnEnable() {
            base.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable() {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endregion

        #region Private Methods
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            // load on main scene
        }
        #endregion
    }
}