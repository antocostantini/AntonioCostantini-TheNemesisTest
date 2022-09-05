using Menu;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

namespace Core {
    /// <summary>
    /// Manages the main scene of the game
    /// </summary>
    public class GameManager : Singleton<GameManager> {
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
        #endregion

        #region Private Methods
        private Vector3 GetRandomPositionInside(Renderer zone) {
            var bounds = zone.bounds;
            var min = bounds.min;
            var max = bounds.max;
            return min + Random.Range(0f, 1f) * (max - min);
        }

        [ContextMenu("Reset game")]
        private void ResetGame() {
            ball.ResetBall(ballSpawnPoint.position);
            blueGoal.position = GetRandomPositionInside(blueGoalSpawnZone);
            redGoal.position = GetRandomPositionInside(redGoalSpawnZone);
        }
        #endregion
    }
}
