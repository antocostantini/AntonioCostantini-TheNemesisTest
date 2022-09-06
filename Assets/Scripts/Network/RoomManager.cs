using System.IO;
using Menu;
using Photon.Pun;
using Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Network {
    /// <summary>
    /// Component used to carry information about the players from the menu to the main scene
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class RoomManager : Singleton<RoomManager> {
        #region Public Variables
        [SerializeField] private GameObject playerManagerPrefab;
        #endregion
        
        #region Properties
        public TeamSelector.Team Team { get; set; }
        #endregion

        #region Behaviour Callbacks

        protected override void Awake() {
            base.Awake();
            DontDestroyOnLoad(this);
        }

        public void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks if the loaded scene is the main scene, and spawns the local player's manager over the network
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            if (scene.buildIndex == 1) {
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerManagerPrefab.name), Vector3.zero, quaternion.identity).GetComponent<PlayerManager>().Team = Team;
            }
        }
        #endregion
    }
}