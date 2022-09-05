using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Menu {
    public class MenuManager : Singleton<MenuManager> {
        #region Private Variables
        private Page _currentPage;
        #endregion
        
        #region Public Variables
        [SerializeField] private Page connectionPage;
        [SerializeField] private Page menuPage;

        [Space] [SerializeField] private Button matchmakingButton;

        #endregion
        
        #region Properties
        public Page ConnectionPage => connectionPage;
        public Page MenuPage => menuPage;
        #endregion
        
        #region Private Methods
        private void CloseCurrentPage() {
            if (_currentPage == null) return;
            _currentPage.Close();
            _currentPage = null;
        }
        #endregion
        
        #region Public Methods
        public void OpenPage(Page page) {
            CloseCurrentPage();
            page.Open();
            _currentPage = page;
        }

        public void CloseMenu(Page page) {
            page.Close();
            _currentPage = null;
        }

        public void OnUsernameChanged(string username) {
            matchmakingButton.interactable = !string.IsNullOrWhiteSpace(username);
        }
        
        public void QuitGame() {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
        #endregion
    }
}