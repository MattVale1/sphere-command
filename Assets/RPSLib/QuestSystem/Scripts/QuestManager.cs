/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPSCore {

    public class QuestManager : MonoBehaviour {

        #region Variables - Use sub regions
        #region Instancing
        public static QuestManager Instance;
        #endregion
        #region Events/Actions
        public event Action<QuestSO> TrackedQuestChanged;
        #endregion
        #region Quests
        [Header("QUESTS LISTS")]
        public List<QuestSO> questList = new();
        private List<QuestSO> possibleQuests = new();
        private List<QuestSO> activeQuests = new();
        private List<QuestSO> completedQuests = new();
        #endregion
        #region Quest Categories
        [Header("QUEST CATEGORIES")]
        public List<QuestCategory> questCategories;
        public GameObject questCategoryPrefab;
        public Transform questCategoryParent;
        #endregion
        #region UI
        [Header("----- UI")]
        public Canvas questCanvas;
        public GameObject questItemPrefab;
        public Transform questListParent;
        private Dictionary<QuestSO, GameObject> questUIItems = new();
        public QuestItemFullUI questItemFullUI;
        #endregion
        #endregion


        #region Setup
        protected void Awake() {
            Instance = this;

            CloseQuestUI();

            PopulateQuestCategories();

            Init();

            //if (!SaveManager.isLoadingFromSave) {
            //    //Debug.Log("Not loading from save, Init quests...");
            //    Init();
            //}
        }

        private void Init() {
            // Init our quest scriptable objects as instances so we don't modify the original
            for (int i = 0; i < questList.Count; i++) {
                if (questList[i].mustHaveCompleted.Count > 0)
                    continue;
                CreateInstanceOfQuest(questList[i]);
            }
            // Add initial quest to kick things off!
            AddQuestToActive(possibleQuests[0]);
            AddQuestToActive(possibleQuests[1]); // TODO remove me
            AddQuestToActive(possibleQuests[2]); // TODO remove me
            AddProgressToQuest(GetActiveQuestByID("TEST_CLAIMABLE_QUEST"));
        }

        private void CreateInstanceOfQuest(QuestSO quest, bool activateNow = false) {
            QuestSO questSO = CreateInstanceOfQuestAndReturn(quest);
            possibleQuests.Add(questSO);
            if (activateNow) {
                AddQuestToActive(questSO);
            }
        }

        private QuestSO CreateInstanceOfQuestAndReturn(QuestSO quest) {
            QuestSO questSO = ScriptableObject.CreateInstance(typeof(QuestSO)) as QuestSO;
            questSO.QUEST_ID = quest.QUEST_ID;
            questSO.IsObjective = quest.IsObjective;
            questSO.title = quest.title;
            questSO.sourceOfQuest = quest.sourceOfQuest;
            questSO.description = quest.description;
            questSO.Objectives = quest.Objectives;
            questSO.rewards = quest.rewards;
            questSO.bonusRewards = quest.bonusRewards;
            questSO.bonusRewardsEarned = quest.bonusRewardsEarned;
            questSO.requiredQuestSteps = quest.requiredQuestSteps;
            questSO.questProgress = quest.questProgress;
            questSO.isActive = quest.isActive;
            questSO.isCompleted = quest.isCompleted;
            questSO.mustHaveCompleted = quest.mustHaveCompleted;
            questSO.questsToProgress = quest.questsToProgress;
            questSO.triggersNextQuest = quest.triggersNextQuest;
            // Data for saving
            questSO.data.ID = quest.data.ID;
            questSO.data.PROGRESS = quest.data.PROGRESS;
            questSO.data.IS_ACTIVE = quest.data.IS_ACTIVE;
            questSO.data.IS_COMPLETE = quest.data.IS_COMPLETE;
            return questSO;
        }
        #endregion

        #region Internal Quest Methods
        private void AddQuestToActive(QuestSO questToAdd) {
            questToAdd.data.ID = questToAdd.QUEST_ID;
            questToAdd.data.IS_ACTIVE = true;
            activeQuests.Add(questToAdd);
            CreateQuestUIItem(questToAdd);
            SetSaveableData();
        }

        private void CheckQuestStatus(QuestSO questToQuery) {
            if (questToQuery.questProgress >= questToQuery.requiredQuestSteps) {
                CompleteQuest(questToQuery);
            }
        }

        private void CompleteQuest(QuestSO questToComplete) {
            // Update quest data...
            questToComplete.isCompleted = true;
            questToComplete.data.IS_COMPLETE = true;
            questToComplete.data.IS_ACTIVE = false;

            // Update lists
            completedQuests.Add(questToComplete);
            activeQuests.Remove(questToComplete);
            // Update save data
            SetSaveableData();

            // UI updates        
            UpdateQuestItem(questToComplete);

            // Check if the quest leads to another...
            if (questToComplete.triggersNextQuest.Count > 0) {
                for (int i = 0; i < questToComplete.triggersNextQuest.Count; i++) {
                    CreateInstanceOfQuest(questToComplete.triggersNextQuest[i], true);
                }
            }

            // Check if this adds progress to another quest (objective)
            if (questToComplete.questsToProgress.Count > 0) {
                for (int i = 0; i < questToComplete.questsToProgress.Count; i++) {
                    AddProgressToQuest(questToComplete.questsToProgress[i]);
                }
            }
        }
        #endregion

        #region UI + Button Handlers
        protected void Update() {
            if (Input.GetKeyDown(KeyCode.J)) {
                questCanvas.enabled = !questCanvas.enabled;
            }
        }

        public void OpenQuestUI() {
            questCanvas.enabled = true;
        }

        public void CloseQuestUI() {
            questCanvas.enabled = false;
        }

        public void CreateQuestUIItem(QuestSO questToAdd) {
            GameObject questItem = Instantiate(questItemPrefab, questListParent);
            // Create the pairing of the UI element and the Quest data in a dictionary, using QuestSO as key
            questUIItems.Add(questToAdd, questItem);
            // Update the UI with the Quest data
            CreateQuestItem(questToAdd);
        }

        private void CreateQuestItem(QuestSO quest) {
            questUIItems[quest].GetComponent<QuestItemUI>().InitUI(quest);
        }

        private void UpdateQuestItem(QuestSO quest) {
            questUIItems[quest].GetComponent<QuestItemUI>().UpdateUI(quest);
        }

        public void RemoveQuestItem(QuestSO quest) {
            // Delete UI object
            Destroy(questUIItems[quest]);
            // Remove Quest from Dictionary
            questUIItems.Remove(quest);
        }

        public void PopulateQuestCategories() {
            foreach (var questType in questCategories) {
                QuestCategoryItemUI questCategoryItem = Instantiate(questCategoryPrefab, questCategoryParent).GetComponent<QuestCategoryItemUI>();
                questCategoryItem.SetData(questType.categoryIcon, questType.category.ToString());
            }
        }

        public void SelectQuest(QuestSO quest) {
            questItemFullUI.SetData(quest);
        }

        public void TrackQuest(QuestSO quest) {
            TrackedQuestChanged?.Invoke(quest);
        }
        #endregion

        #region Claim Rewards
        public void ClaimRewards(QuestSO questData) {
            // Normal reward claim
            if (questData.rewards is { Count: > 0 }) {
                for (int i = 0; i < questData.rewards.Count; i++) {
                    questData.rewards[i].Reward();
                }
            }

            // Bonus reward claim
            if (questData.bonusRewardsEarned) {
                if (questData.bonusRewards is { Count: > 0 }) {
                    for (int i = 0; i < questData.bonusRewards.Count; i++) {
                        questData.bonusRewards[i].Reward();
                    }
                }
            }

            // Remove claimed quest from UI
            RemoveQuestItem(questData);
            // Remove Quest Full Info from UI
            questItemFullUI.Hide();
        }
        #endregion

        #region Public Quest Methods
        public void CreateNewQuest(QuestSO newQuestSO, bool activateNow = true) {
            CreateInstanceOfQuest(newQuestSO, activateNow);
        }

        public void AddProgressToQuest(QuestSO questToProgress) {
            if (activeQuests.Contains(questToProgress)) {
                if (questToProgress.questProgress < questToProgress.requiredQuestSteps) {
                    //Debug.Log("<PrimaryColor=orange>Adding progress to quest: </PrimaryColor>" + questToProgress.title);
                    questToProgress.questProgress++;
                    questToProgress.data.PROGRESS = questToProgress.questProgress;
                    CheckQuestStatus(questToProgress);
                    SetSaveableData();
                }
            }
        }

        public void ActivateQuestOfTypeToBeCompleted(string ID, bool allowDuplicate = false) {
            if (allowDuplicate == false) {
                if (HasQuestCompleted(ID) || IsQuestActive(ID))
                    return;
            }
            CreateInstanceOfQuest(GetPossibleQuestByID(ID), true);
        }

        public QuestSO GetPossibleQuestByID(string ID) {
            for (int i = 0; i < possibleQuests.Count; i++) {
                if (possibleQuests[i].QUEST_ID == ID)
                    return possibleQuests[i];
            }
            return null;
        }

        public QuestSO GetActiveQuestByID(string ID) {
            if (activeQuests.Count == 0)
                return null;

            for (int i = 0; i < activeQuests.Count; i++) {
                if (activeQuests[i].QUEST_ID == ID) {
                    return activeQuests[i];
                }
            }

            return null;
        }

        public bool HasQuestCompleted(string ID) {
            if (completedQuests.Count == 0)
                return false;
            for (int i = 0; i < completedQuests.Count; i++) {
                if (completedQuests[i].QUEST_ID == ID) {
                    return true;
                }
            }
            return false;
        }

        public bool IsQuestActive(string ID) {
            if (activeQuests.Count == 0)
                return false;

            for (int i = 0; i < activeQuests.Count; i++) {
                if (activeQuests[i].QUEST_ID == ID) {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Saving/Loading
        private void SetSaveableData() {
            //GameSaveData.Instance.ActiveQuests.Clear();
            //for (int i = 0; i < activeQuests.Count; i++) {
            //    GameSaveData.Instance.ActiveQuests.Add(activeQuests[i].data);
            //}
            //GameSaveData.Instance.CompletedQuests.Clear();
            //for (int i = 0; i < completedQuests.Count; i++) {            
            //    GameSaveData.Instance.CompletedQuests.Add(completedQuests[i].data);
            //}
        }

        public void LoadActiveQuests(List<QuestData> data) {
            // Loop through all possible quests, if we get a match of ID, recreate it and set to active
            for (int i = 0; i < questList.Count; i++) {
                //Debug.Log("Possible quest 1 query...");
                for (int n = 0; n < data.Count; n++) {
                    if (questList[i].QUEST_ID == data[n].ID) {
                        //Debug.Log("Loading Active Quest :: " + questList[i].QUEST_ID);
                        CreateInstanceOfQuest(questList[i], true);
                    }
                }
            }
        }

        public void LoadCompletedQuests(List<QuestData> data) {
            for (int i = 0; i < questList.Count; i++) {
                for (int n = 0; n < data.Count; n++) {
                    //Debug.Log("LoadCompletedQuests :: Data :: " + data[n].ID);
                    if (questList[i].QUEST_ID == data[n].ID) {
                        completedQuests.Add(questList[i]);
                    }
                }
            }
        }
        #endregion
    }

}