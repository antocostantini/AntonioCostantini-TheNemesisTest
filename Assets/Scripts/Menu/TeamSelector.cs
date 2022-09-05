using Network;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu {
    public class TeamSelector : UIBehaviour {
        public enum Team {
            Neutral, Blue, Red
        }

        #region Public Variables
        [SerializeField] private Team team;
        #endregion

        #region Public Methods
        public void SelectTeam() {
            MenuNetworkManager.Instance.SelectTeam(team);
        }
        #endregion
    }
}