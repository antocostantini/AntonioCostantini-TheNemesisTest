using UnityEngine;

namespace Core {
    /// <summary>
    /// Component attached to the ball
    /// </summary>
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
        /// <summary>
        /// Resets the ball position
        /// </summary>
        /// <param name="position">The new position</param>
        public void ResetBall(Vector3 position) {
            _rigidbody.isKinematic = true;
            transform.position = position;
            _rigidbody.isKinematic = false;
        }
        #endregion
    }
}