using System.Collections;
using Menu;
using Network;
using Photon.Pun;
using Player;
using UI;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Core {
    /// <summary>
    /// Manages the main scene of the game
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : NetworkSingleton<GameManager> {
        #region Public Variables
        [SerializeField] private Transform blueSpawnPoint;
        [SerializeField] private Transform redSpawnPoint;
        [Space]
        [SerializeField] private Ball ball;
        [SerializeField] private Transform ballSpawnPoint;
        [Space]
        [SerializeField] private Transform blueGoal;
        [SerializeField] private Transform redGoal;
        [Space]
        [SerializeField] private MeshRenderer blueGoalSpawnZone;
        [SerializeField] private MeshRenderer redGoalSpawnZone;
        [Space]
        [SerializeField] private GameObject rematchUtility;
        
        #endregion

        #region Private Variables
        private UIManager _uiManager;
        private int _bluePoints = 0;
        private int _redPoints = 0;
        private bool _isGameFinished = false;

        private Coroutine _countdownCo;
        #endregion

        #region Properties
        public PlayerStatus BluePlayer {private get; set; }
        public PlayerStatus RedPlayer {private get; set; }
        #endregion

        #region Behaviour Callbacks
        private void Start() {
            _uiManager = UIManager.Instance;
            if(PhotonNetwork.IsMasterClient)
                ResetGame();
        }

        public override void OnLeftRoom() {
            base.OnLeftRoom();
            Destroy(RoomManager.Instance.gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer) {
            base.OnPlayerLeftRoom(otherPlayer);
            if(_isGameFinished) return;
            if(_countdownCo != null) StopCoroutine(_countdownCo);
            _uiManager.DeactivateCountDown();
            if (Equals(BluePlayer.PhotonView.Owner, otherPlayer)) {
                EndGameForDisconnection(TeamSelector.Team.Red, RedPlayer.Username, otherPlayer.NickName);
            }
            else {
                EndGameForDisconnection(TeamSelector.Team.Blue, BluePlayer.Username, otherPlayer.NickName);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the spawn point of the respective team
        /// </summary>
        /// <param name="team">The desired team</param>
        /// <returns>The transform of the spawn point</returns>
        public Transform GetSpawn(TeamSelector.Team team) {
            return team switch {
                TeamSelector.Team.Neutral => blueSpawnPoint,
                TeamSelector.Team.Blue => blueSpawnPoint,
                TeamSelector.Team.Red => redSpawnPoint,
                _ => blueSpawnPoint
            };
        }

        /// <summary>
        /// Scores a goal for the selected team, and checks if the game is over
        /// </summary>
        /// <param name="team">The team that scored</param>
        public void Goal(TeamSelector.Team team) {
            if(_isGameFinished) return;
            switch (team) {
                case TeamSelector.Team.Blue:
                    _bluePoints++;
                    photonView.RPC(nameof(SetPointsRPC), RpcTarget.All, _bluePoints, team);
                    break;
                case TeamSelector.Team.Red:
                    _redPoints++;
                    photonView.RPC(nameof(SetPointsRPC), RpcTarget.All, _redPoints, team);
                    break;
            }
            if (_bluePoints == 3) {
                Debug.Log("Blue wins");
                photonView.RPC(nameof(IsGameFinishedRPC), RpcTarget.All, true);
            }else if (_redPoints == 3) {
                Debug.Log("Red wins");
                photonView.RPC(nameof(IsGameFinishedRPC), RpcTarget.All, true);
            }
            
            if (_isGameFinished) {
                var winner = _bluePoints == 3 ? TeamSelector.Team.Blue : TeamSelector.Team.Red;
#pragma warning disable CS8509
                string username = winner switch {
                    TeamSelector.Team.Blue => BluePlayer.Username,
                    TeamSelector.Team.Red => RedPlayer.Username 
                };
#pragma warning restore CS8509
                photonView.RPC(nameof(EndGameRPC), RpcTarget.All, winner, username);
            }
            else {
                photonView.RPC(nameof(CountdownRPC), RpcTarget.All, team);
                ResetGame();
            }
        }

        /// <summary>
        /// Leaves the room to return back to the menu
        /// </summary>
        public void BackToMenu() {
            PhotonNetwork.LeaveRoom();
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Matchmaking() {
            Instantiate(rematchUtility);
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets a random position inside of a game object
        /// </summary>
        /// <param name="zone">The zone</param>
        /// <returns>The random position</returns>
        private Vector3 GetRandomPositionInside(Renderer zone) {
            var bounds = zone.bounds;
            var min = bounds.min;
            var max = bounds.max;
            return min + Random.Range(0f, 1f) * (max - min);
        }

        /// <summary>
        /// RPC to update the teams' points
        /// </summary>
        [PunRPC]
        private void SetPointsRPC(int points, TeamSelector.Team team) {
            _uiManager.SetPoints(points, team);
        }

        /// <summary>
        /// Ends the game for disconnection and deactivate player controls
        /// </summary>
        private void EndGameForDisconnection(TeamSelector.Team winner, string username, string disconnectedPlayer) {
            _uiManager.EndPanelForDisconnection(winner, username, disconnectedPlayer);
            DeactivatePlayersControls();
        }
        
        /// <summary>
        /// RPC to end the game and deactivate player controls
        /// </summary>
        [PunRPC] 
        private void EndGameRPC(TeamSelector.Team winner, string username) {
            _uiManager.EndPanel(winner, username);
            DeactivatePlayersControls();
        }
        
        /// <summary>
        /// Resets the positions for the next round
        /// </summary>
        private void ResetGame() {
            var blueGoalPos = GetRandomPositionInside(blueGoalSpawnZone);
            var redGoalPos = GetRandomPositionInside(redGoalSpawnZone);
            
            photonView.RPC(nameof(ResetGameRPC), RpcTarget.All, blueGoalPos, redGoalPos);
        }
        
        /// <summary>
        /// RPC to reset the positions over the network
        /// </summary>
        /// <param name="blueGoalPos"></param>
        /// <param name="redGoalPos"></param>
        [PunRPC]
        private void ResetGameRPC(Vector3 blueGoalPos, Vector3 redGoalPos) {
            ball.ResetBall(ballSpawnPoint.position);
            if (BluePlayer != null) BluePlayer.SetPosition(blueSpawnPoint.position);
            if (RedPlayer != null) RedPlayer.SetPosition(redSpawnPoint.position);

            blueGoal.position = blueGoalPos;
            redGoal.position = redGoalPos;
        }

        /// <summary>
        /// Sets the value of <see cref="_isGameFinished"/> for both clients
        /// </summary>
        /// <param name="value">The new value</param>
        [PunRPC]
        private void IsGameFinishedRPC(bool value) {
            _isGameFinished = value;
        }
        
        /// <summary>
        /// RPC to start the countdown over the network
        /// </summary>
        /// <param name="team">The team that scored</param>
        [PunRPC]
        private void CountdownRPC(TeamSelector.Team team) {
            _countdownCo = StartCoroutine(nameof(CountdownCo), team);
        }

        /// <summary>
        /// The coroutine for the countdown
        /// </summary>
        /// <param name="team">The team that scored</param>
        private IEnumerator CountdownCo(TeamSelector.Team team) {
            DeactivatePlayersControls();
            _uiManager.ActivateCountDown(team);
            for (int i = 3; i >= 0; i--) {
                if(i != 0 )
                    _uiManager.Countdown(i.ToString());
                else
                    _uiManager.Countdown("Go!");
                yield return new WaitForSeconds(1f);
            }
            _uiManager.DeactivateCountDown();
            ActivatePlayersControls();
        }

        /// <summary>
        /// Activates the controls for the players
        /// </summary>
        private void ActivatePlayersControls() {
            if (BluePlayer != null) BluePlayer.ActivateControls();
            if (RedPlayer != null) RedPlayer.ActivateControls();
        }
        
        /// <summary>
        /// Deactivates the controls for the players
        /// </summary>
        private void DeactivatePlayersControls() {
            if (BluePlayer != null) BluePlayer.DeactivateControls();
            if (RedPlayer != null) RedPlayer.DeactivateControls();
        }
        #endregion
    }
}
