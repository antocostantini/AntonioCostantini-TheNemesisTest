using Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility {
    /// <summary>
    /// Utility component used in case we want to get in the matchmaking right after a game has ended
    /// </summary>
    public class RematchUtility : MonoBehaviour {
        #region Behaviour Callbacks
        private void Start() {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endregion

        #region Private Methods
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            if (scene.buildIndex == 0) {
                MenuNetworkManager.Instance.AutomaticMatchmaking = true;
                Destroy(gameObject);
            }
        }
        #endregion
    }
}