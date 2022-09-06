using Menu;
using Network;
using Photon.Pun;
using Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        
        #endregion

        #region Private Variables
        private UIManager _uiManager;
        private int _bluePoints = 0;
        private int _redPoints = 0;
        private bool _isGameFinished = false;
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
            SceneManager.LoadScene(0);
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

        public void Goal(TeamSelector.Team team) {
            switch (team) {
                case TeamSelector.Team.Blue:
                    _bluePoints++;
                    break;
                case TeamSelector.Team.Red:
                    _redPoints++;
                    break;
            }
            photonView.RPC(nameof(SetPointsRPC), RpcTarget.All, _bluePoints, _redPoints);
            if (_bluePoints == 3) {
                Debug.Log("Blue wins");
                _isGameFinished = true;
            }else if (_redPoints == 3) {
                Debug.Log("Red wins");
                _isGameFinished = true;
            }
            ResetGame();

            if (_isGameFinished) {
                var winner = _bluePoints == 3 ? TeamSelector.Team.Blue : TeamSelector.Team.Red;
                string username = BluePlayer.GetComponent<PhotonView>().Owner.NickName;
                photonView.RPC(nameof(EndGameRPC), RpcTarget.All, winner, username);
            }
        }

        public void BackToMenu() {
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods
        private Vector3 GetRandomPositionInside(Renderer zone) {
            var bounds = zone.bounds;
            var min = bounds.min;
            var max = bounds.max;
            return min + Random.Range(0f, 1f) * (max - min);
        }

        [PunRPC]
        private void SetPointsRPC(int bluePoints, int redPoints) {
            _uiManager.SetPoints(bluePoints, redPoints);
        }

        [PunRPC] 
        private void EndGameRPC(TeamSelector.Team winner, string username) {
            _uiManager.EndPanel(winner, username);
            if (BluePlayer != null) BluePlayer.DeactivateControls();
            if (RedPlayer != null) RedPlayer.DeactivateControls();
        }
        
        private void ResetGame() {
            var blueGoalPos = GetRandomPositionInside(blueGoalSpawnZone);
            var redGoalPos = GetRandomPositionInside(redGoalSpawnZone);
            
            photonView.RPC(nameof(ResetGameRPC), RpcTarget.All, blueGoalPos, redGoalPos);
        }
        
        [PunRPC]
        private void ResetGameRPC(Vector3 blueGoalPos, Vector3 redGoalPos) {
            ball.ResetBall(ballSpawnPoint.position);
            if (BluePlayer != null) BluePlayer.SetPosition(blueSpawnPoint.position);
            if (RedPlayer != null) RedPlayer.SetPosition(redSpawnPoint.position);

            blueGoal.position = blueGoalPos;
            redGoal.position = redGoalPos;
        }
        #endregion
    }
}
