using Network;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu {
    public class ReadyButton : UIBehaviour {
        #region Public Variables
        [SerializeField] private GameObject checkmark;
        #endregion
        
        #region Private Variables
        private Button _button;
        private bool _ready;
        #endregion

        #region Behaviour Callbacks
        protected override void Start() {
            base.Start();
            _button = GetComponent<Button>();
        }
        #endregion

        #region Public Methods
        public void SetReady() {
            _ready = !_ready;
            checkmark.SetActive(_ready);
            MenuNetworkManager.Instance.SetReady(_ready);
        }

        public void CheckAs(bool ready) {
            _ready = ready;
            checkmark.SetActive(ready);
        }

        public void SetAsInteractable(bool interactable) {
            _button.interactable = interactable;
        }
        #endregion
    }
}