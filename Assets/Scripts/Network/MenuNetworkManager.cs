using Menu;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Utility;

namespace Network {
    [RequireComponent(typeof(PhotonView))]
    public class MenuNetworkManager : NetworkSingleton<MenuNetworkManager> {
        #region Public Variables
        [SerializeField] private RoomManager roomManager;
        #endregion
        
        #region Private Variables
        private MenuManager _menuManager;
        private bool _isPlayer1Ready;
        private bool _isPlayer2Ready;
        private TeamSelector.Team _player1Team;
        private TeamSelector.Team _player2Team;
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
            if (!PhotonNetwork.IsMasterClient) 
                _menuManager.OpenPage(_menuManager.PreselectionPage);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.LogError("Joined random failed\n"+message);
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.LogError("Create room failed\n"+message);
        }
        
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log(newPlayer.NickName + " has joined the room: " + PhotonNetwork.CurrentRoom.Players.Count);
            _menuManager.OpenPage(_menuManager.PreselectionPage);
            SetPlayersUsernames();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log(otherPlayer.NickName + " has left the room: " + PhotonNetwork.CurrentRoom.Players.Count);
            BackToMenu();
        }
        
        public override void OnLeftRoom() {
            base.OnLeftRoom();
            _menuManager.ResetPlayerPositions();
            _isPlayer1Ready = _isPlayer2Ready = false;
            _menuManager.CheckReadyButtonAs(false);
            _player1Team = _player2Team = TeamSelector.Team.Neutral;
            _menuManager.SetReadyButtonAsInteractable(false);
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

        private void SetPlayersUsernames() {
            string user1 = PhotonNetwork.PlayerList[0].NickName;
            string user2 = PhotonNetwork.PlayerList[1].NickName;
            photonView.RPC(nameof(SetPlayersUsernamesRPC), RpcTarget.All, user1, user2);
        }

        [PunRPC]
        private void SetPlayersUsernamesRPC(string user1, string user2) {
            _menuManager.SetPlayersUsernames(user1, user2);
        }

        public void SelectTeam(TeamSelector.Team team) {
            photonView.RPC(nameof(SelectTeamRPC), RpcTarget.All, team, PhotonNetwork.IsMasterClient);
            roomManager.Team = team;
            _menuManager.SetReadyButtonAsInteractable(team != TeamSelector.Team.Neutral);
            
            _menuManager.CheckReadyButtonAs(false);
            SetReady(false);
        }

        [PunRPC]
        public void SelectTeamRPC(TeamSelector.Team team, bool isPlayer1) {
            if (isPlayer1)
                _player1Team = team;
            else
                _player2Team = team;
            _menuManager.SelectTeam(team, isPlayer1);
        }
        
        public void SetReady(bool ready) {
            photonView.RPC(nameof(SetReadyRpc), RpcTarget.MasterClient, ready, PhotonNetwork.IsMasterClient);
        }
        
        [PunRPC]
        private void SetReadyRpc(bool ready, bool isPlayer1) {
            if (isPlayer1)
                _isPlayer1Ready = ready;
            else
                _isPlayer2Ready = ready;
            
            if(_isPlayer1Ready && _isPlayer2Ready && _player1Team != _player2Team) 
                PhotonNetwork.LoadLevel(1);
        }
        #endregion
        
        #region Private Methods
        private void Connect() {
            PhotonNetwork.ConnectUsingSettings();
        }
        #endregion
    }
}