/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class QuestTracker : MonoBehaviour {

        #region Private Properties
        private QuestSO _questData;
        private List<GameObject> _objectiveContainer = new();
        #endregion

        #region Public Properties
        public Canvas QuestTrackerCanvas;
        public RectTransform ObjectiveContainerParent;
        public TextMeshProUGUI TrackedQuestTitle;
        public GameObject TrackedQuestObjectiveContainer;
        #endregion


        #region Unity Flow
        protected void Awake() {
            QuestTrackerCanvas.enabled = false;
        }

        protected void Start() {
            SubscribeListeners();
        }

        protected void OnDestroy() {
            UnsubscribeListeners();
        }
        #endregion

        #region Private Methods
        private void SubscribeListeners() {
            QuestManager.Instance.TrackedQuestChanged += UpdateTrackedQuest;
        }

        private void UnsubscribeListeners() {
            QuestManager.Instance.TrackedQuestChanged -= UpdateTrackedQuest;
        }

        private void RemoveTrackedQuest() {
            if (_objectiveContainer.Count > 0) {
                foreach (var objective in _objectiveContainer) {
                    Destroy(objective);
                }
                _objectiveContainer.Clear();
            }

            TrackedQuestTitle.text = string.Empty;

            QuestTrackerCanvas.enabled = false;
        }

        private void UpdateTrackedQuest(QuestSO questToTrack) {
            // If we select the same quest to track, untrack/hide it
            if (_questData != null) {
                if (questToTrack.QUEST_ID == _questData.QUEST_ID) {
                    _questData = null;
                    RemoveTrackedQuest();
                    return;
                }
            }

            _questData = questToTrack;

            // Delete any existing objective data objects
            RemoveTrackedQuest();

            // Set tracked quest title
            TrackedQuestTitle.text = questToTrack.title;

            // Spawn all objectives
            if (questToTrack.Objectives is { Count: > 0 }) {
                foreach (var objective in questToTrack.Objectives) {
                    GameObject newObjectiveContainer = Instantiate(TrackedQuestObjectiveContainer, ObjectiveContainerParent);
                    newObjectiveContainer.GetComponent<QuestObjectiveItemUI>().SetData(objective);
                    _objectiveContainer.Add(newObjectiveContainer);
                }
            }

            QuestTrackerCanvas.enabled = true;
            LayoutRebuilder.ForceRebuildLayoutImmediate(ObjectiveContainerParent);
        }
        #endregion

        #region Public Methods

        #endregion

    }

}