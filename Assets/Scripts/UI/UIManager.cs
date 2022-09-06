using Menu;
using TMPro;
using UnityEngine;
using Utility;

namespace UI {
    public class UIManager : Singleton<UIManager> {
        #region Public Variables
        [SerializeField] private TMP_Text bluePointsText;
        [SerializeField] private TMP_Text redPointsText;
        [Space]
        [SerializeField] private GameObject endPanel;
        [SerializeField] private TMP_Text winText;
        [SerializeField] private Color blueColor;
        [SerializeField] private Color redColor;
        #endregion

        #region Public Methods
        public void SetPoints(int bluePoints, int redPoints) {
            bluePointsText.text = bluePoints.ToString();
            redPointsText.text = redPoints.ToString();
        }

        public void EndPanel(TeamSelector.Team team, string username) {
            winText.color = team switch {
                TeamSelector.Team.Blue => blueColor,
                TeamSelector.Team.Red => redColor,
                TeamSelector.Team.Neutral => Color.white,
                _ => Color.white
            };
            winText.SetText($"{username} wins!");
            endPanel.SetActive(true);
        }
        #endregion
    }
}