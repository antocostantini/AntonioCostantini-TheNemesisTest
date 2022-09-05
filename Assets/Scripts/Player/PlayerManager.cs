using System.IO;
using Menu;
using Photon.Pun;
using UnityEngine;

namespace Player {
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
        }

        private void Start() {
            if(_photonView.IsMine)
                CreateController();
        }
        #endregion

        #region private Methods
        private void CreateController() {
            //Transform spawn = 
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", playerControllerPrefab.name), Vector3.zero, Quaternion.identity, data: new object[]{Team});
        }
        #endregion
    }
}