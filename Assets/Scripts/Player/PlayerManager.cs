using System.IO;
using Core;
using Menu;
using Photon.Pun;
using UnityEngine;

namespace Player {
    /// <summary>
    /// Component used to manage the player's controller
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class PlayerManager : MonoBehaviour {
        #region Public Variables
        [SerializeField] private GameObject playerControllerPrefab;
        #endregion
        
        #region Private Variables
        private PhotonView _photonView;
        #endregion
        
        #region Properties
        public TeamSelector.Team Team { get; set; }
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            _photonView = GetComponent<PhotonView>();
#if UNITY_EDITOR
            gameObject.name = $"{_photonView.Owner.NickName} Player Manager";
#endif
        }

        private void Start() {
            if(_photonView.IsMine)
                CreateController();
        }
        #endregion

        #region private Methods
        /// <summary>
        /// Spawns the player controller over the network
        /// </summary>
        private void CreateController() {
            var spawn = GameManager.Instance.GetSpawn(Team);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerControllerPrefab.name), spawn.position, spawn.rotation, data: new object[]{Team});
        }
        #endregion
    }
}