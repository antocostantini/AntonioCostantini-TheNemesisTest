using System;
using Core;
using Menu;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    /// <summary>
    /// Component used to change the player controller status
    /// </summary>
    public class PlayerStatus : MonoBehaviour
    {
        #region Public Variables
        [SerializeField] private MeshRenderer meshRenderer;
        [Space]
        [SerializeField] private Material blueMaterial;
        [SerializeField] private Material redMaterial;
        #endregion

        #region Private Variables
        private TeamSelector.Team _team;
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            var photonView = GetComponent<PhotonView>();
            _team = (TeamSelector.Team)photonView.InstantiationData[0];
            meshRenderer.material = _team == TeamSelector.Team.Blue ? blueMaterial : redMaterial;
#if UNITY_EDITOR
            gameObject.name = $"{photonView.Owner.NickName} Player Controller";
#endif
        }

        private void Start() {
            switch (_team) {
                case TeamSelector.Team.Blue:
                    GameManager.Instance.BluePlayer = this;
                    break;
                case TeamSelector.Team.Red:
                    GameManager.Instance.RedPlayer = this;
                    break;
            }
        }
        #endregion

        #region Public Methods
        public void SetPosition(Vector3 pos) {
            transform.position = pos;
        }

        public void DeactivateControls() {
            if (TryGetComponent<PlayerInput>(out var playerInput)) {
                playerInput.enabled = false;
            }
        }
        #endregion
    }
}
