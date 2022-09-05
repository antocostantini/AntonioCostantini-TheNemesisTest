using TMPro;
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
        [SerializeField] private Page searchingPage;
        [SerializeField] private Page preselectionPage;
        [Space]
        [SerializeField] private Transform neutralTeamSelector;
        [SerializeField] private Transform blueTeamSelector;
        [SerializeField] private Transform redTeamSelector;
        [Space]
        [SerializeField] private Transform player1;
        [SerializeField] private Transform player2;
        [Space] 
        [SerializeField] private Button matchmakingButton;
        [SerializeField] private ReadyButton readyButton;
        #endregion
        
        #region Properties
        public Page ConnectionPage => connectionPage;
        public Page MenuPage => menuPage;
        public Page SearchingPage => searchingPage;
        public Page PreselectionPage => preselectionPage;
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

        public void ClosePage(Page page) {
            page.Close();
            _currentPage = null;
        }

        public void SetPlayersUsername(string username1, string username2) {
            player1.GetComponent<TMP_Text>().SetText(username1);
            player2.GetComponent<TMP_Text>().SetText(username2);
        }
        
        public void ResetPlayerPositions() {
            float x = neutralTeamSelector.position.x;
            player1.position = new Vector3(x, player1.position.y, 0);
            player2.position = new Vector3(x, player2.position.y, 0);
        }
        
        public void SelectTeam(TeamSelector.Team team, bool isPlayer1) {
            float x = team switch {
                TeamSelector.Team.Neutral => neutralTeamSelector.position.x,
                TeamSelector.Team.Blue => blueTeamSelector.position.x,
                TeamSelector.Team.Red => redTeamSelector.position.x,
                _ => neutralTeamSelector.position.x
            };
            if (isPlayer1)
                player1.position = new Vector3(x, player1.position.y, 0);
            else
                player2.position = new Vector3(x, player2.position.y, 0);
        }
        
        public void SetReadyButtonAsInteractable(bool interactable) {
            readyButton.SetAsInteractable(interactable);
        }
    
        public void CheckReadyButtonAs(bool ready) {
            readyButton.CheckAs(ready);
        }

        public void OnUsernameChanged(string username) {
            matchmakingButton.interactable = !string.IsNullOrWhiteSpace(username);
        }
        
        public void OnUsernameEndEdit(string username) {
            if(!string.IsNullOrWhiteSpace(username))
               Network.MenuNetworkManager.Instance.ChangeUsername(username);
        }
        
        public void QuitGame() {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
        #endregion
    }
}