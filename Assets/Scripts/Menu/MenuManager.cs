using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Menu {
    /// <summary>
    /// Manages the main menu
    /// </summary>
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

        #region Public Methods
        /// <summary>
        /// Opens a specific page
        /// </summary>
        /// <param name="page">The desired page</param>
        public void OpenPage(Page page) {
            CloseCurrentPage();
            page.Open();
            _currentPage = page;
        }

        /// <summary>
        /// Closes a specific page
        /// </summary>
        /// <param name="page">The desired page</param>
        public void ClosePage(Page page) {
            page.Close();
            _currentPage = null;
        }

        /// <summary>
        /// Sets the players' usernames in the preselection page
        /// </summary>
        /// <param name="username1">Player 1 username</param>
        /// <param name="username2">Player 2 username</param>
        public void SetPlayersUsernames(string username1, string username2) {
            player1.GetComponent<TMP_Text>().SetText(username1);
            player2.GetComponent<TMP_Text>().SetText(username2);
        }
        
        /// <summary>
        /// Resets the position of the players' usernames in preselection page
        /// </summary>
        public void ResetPlayerPositions() {
            float x = neutralTeamSelector.position.x;
            player1.position = new Vector3(x, player1.position.y, 0);
            player2.position = new Vector3(x, player2.position.y, 0);
        }
        
        /// <summary>
        /// Selects the team in which to move the desired player
        /// </summary>
        /// <param name="team">The selected team</param>
        /// <param name="isPlayer1">True if player 1, false if player 2</param>
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
        
        /// <summary>
        /// Sets the ready button interactable or not
        /// </summary>
        public void SetReadyButtonAsInteractable(bool interactable) {
            readyButton.SetAsInteractable(interactable);
        }
    
        /// <summary>
        /// Sets the ready button as ready or not
        /// </summary>
        public void CheckReadyButtonAs(bool ready) {
            readyButton.CheckAs(ready);
        }

        /// <summary>
        /// Enables/disables the matchmaking button when the user writes its username
        /// </summary>
        /// <param name="username">The username</param>
        public void OnUsernameChanged(string username) {
            matchmakingButton.interactable = !string.IsNullOrWhiteSpace(username);
        }
        
        /// <summary>
        /// Changes the player's username when the player has finished editing it
        /// </summary>
        /// <param name="username">The new username</param>
        public void OnUsernameEndEdit(string username) {
            if(!string.IsNullOrWhiteSpace(username))
               Network.MenuNetworkManager.Instance.ChangeUsername(username);
        }
        
        /// <summary>
        /// Closes the game
        /// </summary>
        public void QuitGame() {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
        #endregion
        
        #region Private Methods
        /// <summary>
        /// Closes the current page
        /// </summary>
        private void CloseCurrentPage() {
            if (_currentPage == null) return;
            _currentPage.Close();
            _currentPage = null;
        }
        #endregion
    }
}