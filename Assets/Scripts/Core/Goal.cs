using Menu;
using UnityEngine;

namespace Core {
    public class Goal : MonoBehaviour {
        #region Public Variables
        [SerializeField] private TeamSelector.Team team;
        #endregion

        #region Behaviour Callbacks
        private void OnTriggerEnter(Collider other) {
            Debug.Log($"Goal for the {team} team!");
        }
        #endregion
    }
}