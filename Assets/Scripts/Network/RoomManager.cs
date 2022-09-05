using System.IO;
using Menu;
using Photon.Pun;
using Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network {
    [RequireComponent(typeof(PhotonView))]
    public class RoomManager : MonoBehaviourPunCallbacks {
        #region Public Variables
        [SerializeField] private GameObject playerManagerPrefab;
        #endregion
        
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
            if (scene.buildIndex == 1) {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerManagerPrefab.name), Vector3.zero, quaternion.identity).GetComponent<PlayerManager>().Team = Team;
            }
        }
        #endregion
    }
}