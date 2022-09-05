using Photon.Pun;
using UnityEngine;
using Utility;

namespace Network {
    public class MenuNetworkManager : NetworkSingleton<MenuNetworkManager> {
        #region Behaviour Callbacks
        private void Start() {
            Connect();
        }
        
        public override void OnConnectedToMaster() {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby() {
            base.OnJoinedLobby();
            Debug.Log("Connected to lobby!");
        }
        #endregion
        
        #region Private Methods
        private void Connect() {
            PhotonNetwork.ConnectUsingSettings();
        }
        #endregion
    }
}