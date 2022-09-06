using Menu;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Utility;

namespace Network {
    [RequireComponent(typeof(PhotonView))]
    public class MenuNetworkManager : NetworkSingleton<MenuNetworkManager> {
        #region Private Variables
        private MenuManager _menuManager;
        private bool _isPlayer1Ready;
        private bool _isPlayer2Ready;
        private TeamSelector.Team _player1Team;
        private TeamSelector.Team _player2Team;

        private bool _otherPlayerDisconnected = false;
        #endregion
        
        #region Behaviour Callbacks
        private void Start() {
            _menuManager = MenuManager.Instance;
            _menuManager.OpenPage(_menuManager.ConnectionPage);
            if(!PhotonNetwork.IsConnected)
                PhotonNetwork.ConnectUsingSettings();
        }
        
        public override void OnConnectedToMaster() {
            base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby() {
            base.OnJoinedLobby();
            if (_otherPlayerDisconnected) { // if we come from a disconnection, we return in the matchmaking
                _otherPlayerDisconnected = false;
                StartMatchmaking();
            }
            else {  // otherwise we simply open the menu
                _menuManager.OpenPage(_menuManager.MenuPage);
            }
        }
        
        public override void OnJoinedRoom() {
            base.OnJoinedRoom();
            Debug.Log("Joined room with " + PhotonNetwork.CurrentRoom.Players.Count + " players");
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
            Debug.Log(newPlayer.NickName + " has joined the room: " + PhotonNetwork.CurrentRoom.Players.Count + " players");
            _menuManager.OpenPage(_menuManager.PreselectionPage);
            SetPlayersUsernames();
            // we close the room when its full to prevent other players from entering if one player leaves
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log(otherPlayer.NickName + " has left the room");
            _otherPlayerDisconnected = true;
            PhotonNetwork.LeaveRoom();
        }
        
        public override void OnLeftRoom() {
            base.OnLeftRoom();
            _menuManager.ResetPlayerPositions();
            _isPlayer1Ready = _isPlayer2Ready = false;
            _menuManager.CheckReadyButtonAs(false);
            _player1Team = _player2Team = TeamSelector.Team.Neutral;
            _menuManager.SetReadyButtonAsInteractable(false);
            if (!_otherPlayerDisconnected) {
                _menuManager.OpenPage(_menuManager.MenuPage);
            }
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Begins searching for an opponent<br/>
        /// If no room is found, it creates a new one
        /// </summary>
        public void StartMatchmaking() {
            var roomOptions = new RoomOptions {
                MaxPlayers = 2
            };
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
            _menuManager.OpenPage(_menuManager.SearchingPage);
        }
        
        /// <summary>
        /// Leaves the current room to go back to the main menu
        /// </summary>
        public void BackToMenu() {
            PhotonNetwork.LeaveRoom();
        }

        /// <summary>
        /// Changes the player's username
        /// </summary>
        /// <param name="username">The new username</param>
        public void ChangeUsername(string username) {
            PhotonNetwork.NickName = username;
        }

        /// <summary>
        /// Sets both players' usernames for the menu
        /// </summary>
        private void SetPlayersUsernames() {
            string user1 = PhotonNetwork.PlayerList[0].NickName;
            string user2 = PhotonNetwork.PlayerList[1].NickName;
            photonView.RPC(nameof(SetPlayersUsernamesRPC), RpcTarget.All, user1, user2);
        }

        /// <summary>
        /// RPC to set the players' usernames on both clients 
        /// </summary>
        /// <param name="user1">Player 1 username</param>
        /// <param name="user2">Player 2 username</param>
        [PunRPC]
        private void SetPlayersUsernamesRPC(string user1, string user2) {
            _menuManager.SetPlayersUsernames(user1, user2);
        }

        /// <summary>
        /// Selects the team for the local player
        /// </summary>
        /// <param name="team"></param>
        public void SelectTeam(TeamSelector.Team team) {
            photonView.RPC(nameof(SelectTeamRPC), RpcTarget.All, team, PhotonNetwork.IsMasterClient);
            RoomManager.Instance.Team = team;
            _menuManager.SetReadyButtonAsInteractable(team != TeamSelector.Team.Neutral);
            
            _menuManager.CheckReadyButtonAs(false);
            SetReady(false);
        }

        /// <summary>
        /// RPC to sync the team selection for both players
        /// </summary>
        /// <param name="team">The selected team</param>
        /// <param name="isPlayer1">The player that has selected it</param>
        [PunRPC]
        public void SelectTeamRPC(TeamSelector.Team team, bool isPlayer1) {
            if (isPlayer1)
                _player1Team = team;
            else
                _player2Team = team;
            _menuManager.SelectTeam(team, isPlayer1);
        }
        
        /// <summary>
        /// Sets the player as ready or not
        /// </summary>
        public void SetReady(bool ready) {
            photonView.RPC(nameof(SetReadyRpc), RpcTarget.MasterClient, ready, PhotonNetwork.IsMasterClient);
        }
        
        /// <summary>
        /// RPC to notify the master client when a player is ready or not, and eventually load the main scene
        /// </summary>
        /// <param name="ready">If the player is ready or not</param>
        /// <param name="isPlayer1">The player that has selected it</param>
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
    }
}