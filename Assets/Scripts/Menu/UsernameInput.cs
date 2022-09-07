using TMPro;
using UnityEngine;

namespace Menu {
    /// <summary>
    /// Components used to memorize the local username
    /// </summary>
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
            _inputField.onEndEdit.Invoke(_inputField.text);
        }

        private void OnEnable() {
            _inputField.onEndEdit.AddListener(SaveUsername);
        }

        private void OnDisable() {
            _inputField.onEndEdit.RemoveListener(SaveUsername);
        }
        #endregion

        #region Private Methods.
        /// <summary>
        /// Saves the username to the playerprefs
        /// </summary>
        private void SaveUsername(string username) {
            if(!string.IsNullOrWhiteSpace(username)) {
                PlayerPrefs.SetString("user", username);
                PlayerPrefs.Save();
            }
        }
        #endregion
    }
}