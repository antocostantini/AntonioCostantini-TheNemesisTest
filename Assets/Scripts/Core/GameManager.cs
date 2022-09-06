using Menu;
using Photon.Pun;
using TMPro;
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
        [SerializeField] private TMP_Text bluePointsText;
        [SerializeField] private TMP_Text redPointsText;
        #endregion

        #region Private Variables
        private int _bluePoints = 0;
        private int _redPoints = 0;
        #endregion

        #region Properties
        public Transform BluePlayer {private get; set; }
        public Transform RedPlayer {private get; set; }
        #endregion

        #region Behaviour Callbacks
        private void Start() {
            if(PhotonNetwork.IsMasterClient)
                ResetGame();
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
            }else if (_redPoints == 3) {
                Debug.Log("Red wins");
            }
            ResetGame();
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
            bluePointsText.text = bluePoints.ToString();
            redPointsText.text = redPoints.ToString();
        }
        
        private void ResetGame() {
            var blueGoalPos = GetRandomPositionInside(blueGoalSpawnZone);
            var redGoalPos = GetRandomPositionInside(redGoalSpawnZone);
            
            photonView.RPC(nameof(ResetGameRPC), RpcTarget.All, blueGoalPos, redGoalPos);
        }
        
        [PunRPC]
        private void ResetGameRPC(Vector3 blueGoalPos, Vector3 redGoalPos) {
            ball.ResetBall(ballSpawnPoint.position);
            if (BluePlayer != null) BluePlayer.position = blueSpawnPoint.position;
            if (RedPlayer != null) RedPlayer.position = redSpawnPoint.position;

            blueGoal.position = blueGoalPos;
            redGoal.position = redGoalPos;
        }
        #endregion
    }
}
