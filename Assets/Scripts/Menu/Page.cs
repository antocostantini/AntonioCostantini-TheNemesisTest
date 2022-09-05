using UnityEngine.EventSystems;

namespace Menu {
    /// <summary>
    /// A single page of the menu
    /// </summary>
    public class Page : UIBehaviour {
        #region Private Variables
        private bool _isOpen;
        #endregion
        
        #region Behaviour Callbacks
        protected override void Awake() {
            base.Awake();
            _isOpen = gameObject.activeSelf;
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Activates the page
        /// </summary>
        public void Open() {
            if (!_isOpen) {
                gameObject.SetActive(true);
                _isOpen = true;
            }
        }

        /// <summary>
        /// Deactivates the page
        /// </summary>
        public void Close() {
            if (_isOpen) {
                gameObject.SetActive(false);
                _isOpen = false;
            }
        }
        #endregion
    }
}