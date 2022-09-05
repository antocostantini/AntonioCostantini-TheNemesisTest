using Menu;
using UnityEngine;
using Utility;

namespace Core {
    /// <summary>
    /// Manages the main scene of the game
    /// </summary>
    public class GameManager : Singleton<GameManager> {
        #region Public Variables
        [SerializeField] private Transform blueSpawnPoint;
        [SerializeField] private Transform redSpawnPoint;
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
    }
}
