using Menu;
using Photon.Pun;
using UnityEngine;

namespace Core {
    /// <summary>
    /// Component used to score goals
    /// </summary>
    public class Goal : MonoBehaviour {
        #region Public Variables
        [SerializeField] private TeamSelector.Team team;
        [SerializeField] private Collider goalCollider;
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            if (!PhotonNetwork.IsMasterClient) 
                Destroy(goalCollider);  // only the master client needs to get notified on collisions
        }

        private void OnTriggerEnter(Collider other) {
            GameManager.Instance.Goal(team);
        }
        #endregion
    }
}