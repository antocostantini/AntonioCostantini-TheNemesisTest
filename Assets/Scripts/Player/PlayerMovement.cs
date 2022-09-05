using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    [RequireComponent(typeof(PhotonView))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Public Variables
        [SerializeField] private float movementSpeed = 10f;
        #endregion

        #region Private Variables
        private PhotonView _photonView;
        private Rigidbody _rigidbody;

        private Vector3 _velocity;
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            _photonView = GetComponent<PhotonView>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start() {
            if(!_photonView.IsMine)
                RemoveComponents();
        }

        private void Update() {
            if(_photonView.IsMine)
                _rigidbody.velocity = new Vector3(_velocity.x, 0, _velocity.z);
        }

        public void OnMovement(InputValue value) {
            var mov = value.Get<Vector2>();
            _velocity = (Vector3.forward * mov.y + Vector3.right * mov.x) * movementSpeed;
        }
        #endregion

        #region Private Methods
        private void RemoveComponents() {
            Destroy(GetComponent<PlayerInput>());
            Destroy((PlayerMovement)this);
            //_rigidbody.isKinematic = true;
        }
        #endregion
    }
}
