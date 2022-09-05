using Menu;
using Photon.Pun;
using UnityEngine;
using Utility;

namespace Network {
    public class MenuNetworkManager : NetworkSingleton<MenuNetworkManager> {
        #region Private Variables
        private MenuManager _menuManager;
        #endregion
        
        #region Behaviour Callbacks
        private void Start() {
            _menuManager = MenuManager.Instance;
            _menuManager.OpenPage(_menuManager.ConnectionPage);
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
            _menuManager.OpenPage(_menuManager.MenuPage);
        }
        #endregion
        
        #region Private Methods
        private void Connect() {
            PhotonNetwork.ConnectUsingSettings();
        }
        #endregion
    }
}