using UnityEngine;

namespace Core {
    public class Ball : MonoBehaviour {
        #region Private Variables
        private Rigidbody _rigidbody;
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }
        #endregion

        #region Public Methods
        public void ResetBall(Vector3 position) {
            _rigidbody.isKinematic = true;
            transform.position = position;
            _rigidbody.isKinematic = false;
        }
        #endregion
    }
}