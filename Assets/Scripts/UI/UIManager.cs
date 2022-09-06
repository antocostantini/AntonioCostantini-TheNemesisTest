using Menu;
using TMPro;
using UnityEngine;
using Utility;

namespace UI {
    /// <summary>
    /// Manages the ui in the main scene
    /// </summary>
    public class UIManager : Singleton<UIManager> {
        #region Public Variables
        [SerializeField] private TMP_Text bluePointsText;
        [SerializeField] private TMP_Text redPointsText;
        [Space]
        [SerializeField] private GameObject endPanel;
        [SerializeField] private TMP_Text winText;
        [SerializeField] private TMP_Text disconnectionText;
        [SerializeField] private Color blueColor;
        [SerializeField] private Color redColor;
        #endregion

        #region Public Methods
        /// <summary>
        /// Updates the points on screen
        /// </summary>
        public void SetPoints(int bluePoints, int redPoints) {
            bluePointsText.text = bluePoints.ToString();
            redPointsText.text = redPoints.ToString();
        }

        /// <summary>
        /// Activates the end panel with the necessary information
        /// </summary>
        /// <param name="winner">The team that won the game</param>
        /// <param name="username">The player that won the game</param>
        public void EndPanel(TeamSelector.Team winner, string username) {
            winText.color = winner switch {
                TeamSelector.Team.Blue => blueColor,
                TeamSelector.Team.Red => redColor,
                TeamSelector.Team.Neutral => Color.white,
                _ => Color.white
            };
            winText.SetText($"{username} wins!");
            endPanel.SetActive(true);
        }

        /// <summary>
        /// Activates the end panel with the necessary information when the other player disconnects
        /// </summary>
        /// <param name="winner">The team that won the game</param>
        /// <param name="username">The player that won the game</param>
        /// <param name="disconnectedPlayer">The player that disconnected</param>
        public void EndPanelForDisconnection(TeamSelector.Team winner, string username, string disconnectedPlayer) {
            var color = winner switch {
                TeamSelector.Team.Blue => blueColor,
                TeamSelector.Team.Red => redColor,
                TeamSelector.Team.Neutral => Color.white,
                _ => Color.white
            };
            winText.color = color;
            winText.SetText($"{username} wins!");
            disconnectionText.color = color;
            disconnectionText.SetText($"{disconnectedPlayer} disconnected");
            disconnectionText.gameObject.SetActive(true);
            endPanel.SetActive(true);
        }
        #endregion
    }
}