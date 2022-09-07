using Network;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu {
    /// <summary>
    /// The ready button used when confirming the team on the preselection page
    /// </summary>
    public class ReadyButton : UIBehaviour {
        #region Public Variables
        [SerializeField] private GameObject checkmark;
        #endregion
        
        #region Private Variables
        private Button _button;
        private bool _ready;
        #endregion

        #region Behaviour Callbacks
        protected override void Awake() {
            base.Awake();
            _button = GetComponent<Button>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the local player as ready
        /// </summary>
        public void SetReady() {
            _ready = !_ready;
            checkmark.SetActive(_ready);
            MenuNetworkManager.Instance.SetReady(_ready);
        }

        /// <summary>
        /// Activates/deactivates the checkmark img on the button
        /// </summary>
        public void CheckAs(bool ready) {
            _ready = ready;
            checkmark.SetActive(ready);
        }

        /// <summary>
        /// Sets the ready button as interactable or not
        /// </summary>
        public void SetAsInteractable(bool interactable) {
            if(_button) _button.interactable = interactable;
        }
        #endregion
    }
}