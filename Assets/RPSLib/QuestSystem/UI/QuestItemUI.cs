/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;

namespace RPSCore {

    public class QuestItemUI : MonoBehaviour {

        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        private QuestSO questData;

        [Header("BUTTONS")]
        public GameObject claimButton;
        public GameObject trackButton;


        #region UI 
        public void InitUI(QuestSO data) {
            // Show basic quest data
            questData = data;
            title.text = questData.title;
            description.text = questData.description;

            // Update certain UI elements
            UpdateUI(questData);
        }

        public void UpdateUI(QuestSO data) {
            // Show basic quest data
            questData = data;

            // Quest complete state
            claimButton.SetActive(questData.isCompleted);
            trackButton.SetActive(!questData.isCompleted);
        }
        #endregion

        #region Button Handlers
        public void ClaimReward() {
            QuestManager.Instance.ClaimRewards(questData);
        }

        public void SelectQuestItem() {
            QuestManager.Instance.SelectQuest(questData);
        }

        public void TrackQuest() {
            QuestManager.Instance.TrackQuest(questData);
        }
        #endregion

    }

}