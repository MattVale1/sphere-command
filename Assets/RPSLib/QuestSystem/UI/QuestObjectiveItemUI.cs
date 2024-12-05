/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class QuestObjectiveItemUI : MonoBehaviour {

        #region Private Properties

        #endregion

        #region Public Propertiies
        public TextMeshProUGUI ObjectiveText;
        public Image ObjectiveStatusImage;
        public Color InProgressColor;
        public Color CompletedColor;
        public Color FailedColor;

        public GameObject StatusIcon_Completed;
        public GameObject StatusIcon_Failed;
        #endregion


        #region Unity Flow
        protected void Awake() {
            CacheData();
        }
        #endregion

        #region Private Methods
        private void CacheData() {

        }
        #endregion

        #region Public Methods
        public void SetData(QuestSO objective) {
            // Set the objective description text
            ObjectiveText.text = objective.description;

            // Set status image color
            ObjectiveStatusImage.color = InProgressColor;

            // Disable status icons for now
            StatusIcon_Completed.SetActive(false);
            StatusIcon_Failed.SetActive(false);
        }

        public void UpdateObjective(QuestSO objective) {
            // Set status image color
            ObjectiveStatusImage.color = objective.isCompleted ? CompletedColor : InProgressColor;
            // TODO Fail condition in QuestSO?

            // Disable status icons for now
            StatusIcon_Completed.SetActive(objective.isCompleted);
            //StatusIcon_Failed.SetActive(objective.isCompleted);
        }
        #endregion

    }

}