using Menu;
using Photon.Pun;
using Photon.Realtime;
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
            _menuManager.OpenPage(_menuManager.MenuPage);
        }
        
        public override void OnJoinedRoom() {
            base.OnJoinedRoom();
            Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Players.Count);
            if (PhotonNetwork.IsMasterClient) {
                Debug.Log("Master client!");
            }
            else {
                _menuManager.OpenPage(_menuManager.PreselectionPage);
            }
        }

        public override void OnCreatedRoom() {
            base.OnCreatedRoom();
            Debug.Log("Created room");
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.LogError("Joined random failed\n"+message);
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.LogError("Create room failed\n"+message);
        }
        
        public override void OnPlayerEnteredRoom(Player newPlayer) {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log(newPlayer.NickName + " has joined the room: " + PhotonNetwork.CurrentRoom.Players.Count);
            _menuManager.OpenPage(_menuManager.PreselectionPage);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log(otherPlayer.NickName + " has left the room: " + PhotonNetwork.CurrentRoom.Players.Count);
            BackToMenu();
        }
        
        public override void OnLeftRoom() {
            base.OnLeftRoom();
            
            _menuManager.OpenPage(_menuManager.MenuPage);
        }
        #endregion
        
        #region Public Methods
        public void StartMatchmaking() {
            var roomOptions = new RoomOptions {
                MaxPlayers = 2
            };
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
            _menuManager.OpenPage(_menuManager.SearchingPage);
        }
        
        public void BackToMenu() {
            PhotonNetwork.LeaveRoom();
        }

        public void ChangeUsername(string username) {
            PhotonNetwork.NickName = username;
        }
        #endregion
        
        #region Private Methods
        private void Connect() {
            PhotonNetwork.ConnectUsingSettings();
        }
        #endregion
    }
}