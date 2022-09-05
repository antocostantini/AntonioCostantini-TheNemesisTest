using Menu;
using Photon.Pun;
using UnityEngine;

namespace Player {
    public class PlayerStatus : MonoBehaviour
    {
        #region Public Variables
        [SerializeField] private MeshRenderer meshRenderer;
        [Space]
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            var photonView = GetComponent<PhotonView>();
            var team = (TeamSelector.Team)photonView.InstantiationData[0];
            meshRenderer.material = team == TeamSelector.Team.Blue ? blueMaterial : redMaterial;
        }
        #endregion
    }
}
