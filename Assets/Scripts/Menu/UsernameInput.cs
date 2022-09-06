using TMPro;
using UnityEngine;

namespace Menu {
    public class UsernameInput : MonoBehaviour {
        #region Private Variables
        private TMP_InputField _inputField;
        #endregion

        #region Behaviour Callbacks
        private void Awake() {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void Start() {
            _inputField.text = PlayerPrefs.GetString("user", string.Empty);
        }

        private void OnEnable() {
            _inputField.onEndEdit.AddListener(SaveUsername);
        }

        private void OnDisable() {
            _inputField.onEndEdit.RemoveListener(SaveUsername);
        }
        #endregion

        #region Private Methods
        private void SaveUsername(string username) {
            if(!string.IsNullOrWhiteSpace(username)) {
                PlayerPrefs.SetString("user", username);
                PlayerPrefs.Save();
            }
        }
        #endregion
    }
}