using Menu;
using UnityEngine;
using Utility;

namespace Core {
    public class GameManager : Singleton<GameManager> {
        #region Public Variables
        [SerializeField] private Transform blueSpawnPoint;
        [SerializeField] private Transform redSpawnPoint;
        #endregion

        #region Public Methods
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
