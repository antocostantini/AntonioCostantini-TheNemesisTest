using Network;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu {
    /// <summary>
    /// Component used to switch to the desired team
    /// </summary>
    public class TeamSelector : UIBehaviour {
        public enum Team {
            Neutral, Blue, Red
        }

        #region Public Variables
        [SerializeField] private Team team;
        #endregion

        #region Public Methods
        /// <summary>
        /// Selects this team as the desired one
        /// </summary>
        public void SelectTeam() {
            MenuNetworkManager.Instance.SelectTeam(team);
        }
        #endregion
    }
}