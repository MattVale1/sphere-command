/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class QuestItemFullUI : MonoBehaviour {

        public TextMeshProUGUI title;
        public TextMeshProUGUI source;
        public TextMeshProUGUI description;
        private QuestSO questData;
        public GameObject questContainer;
        public RectTransform questContainerRectTransform;

        [Header("REWARD DATA")]
        public GameObject PrimaryRewardContainer;
        public TextMeshProUGUI PrimaryCurrencyRewardText;
        public GameObject SecondaryRewardContainer;
        public TextMeshProUGUI SecondaryCurrencyRewardText;
        public GameObject TertiaryRewardContainer;
        public TextMeshProUGUI TertiaryCurrencyRewardText;

        public GameObject normalRewardTitle;
        public GameObject normalRewardUIPanel;
        public GameObject bonusRewardTitle;
        public GameObject bonusRewardUIPanel;
        public GameObject rewardItemPrefab;
        private List<GameObject> _rewardItems = new(); // Reward list, so we can delete when switching quests


        #region UI 
        private void Reset() {
            questContainer.SetActive(false);

            PrimaryRewardContainer.SetActive(false);
            SecondaryRewardContainer.SetActive(false);
            TertiaryRewardContainer.SetActive(false);

            normalRewardTitle.SetActive(false);
            normalRewardUIPanel.SetActive(false);

            bonusRewardTitle.SetActive(false);
            bonusRewardUIPanel.SetActive(false);

            for (int i = 0; i < _rewardItems.Count; i++) {
                Destroy(_rewardItems[i]);
            }
        }

        public void SetData(QuestSO data) {
            // Ignore updates if we select the same quest
            if (questData == data) {
                return;
            }

            // Reset any UI elements to a default state
            Reset();

            // Show basic quest data
            questData = data;
            title.text = questData.title;
            source.text = questData.sourceOfQuest;
            description.text = questData.description;

            // Spawn rewards
            SpawnRewardItems();

            // Show the rewards
            if (data.rewards.Count > 0) {
                normalRewardTitle.SetActive(true);
                normalRewardUIPanel.SetActive(true);
            }

            if (data.bonusRewards.Count > 0) {
                bonusRewardTitle.SetActive(true);
                bonusRewardUIPanel.SetActive(true);
            }

            questContainer.SetActive(true);

            LayoutRebuilder.ForceRebuildLayoutImmediate(questContainerRectTransform);
        }

        public void Hide() {
            Reset();
        }

        private void SpawnRewardItems() {
            // Normal reward items
            if (questData.rewards is { Count: > 0 }) {
                for (int i = 0; i < questData.rewards.Count; i++) {
                    CreateRewardItemsBasedOnType(questData.rewards[i]);
                }
            }

            // Bonus reward items
            if (questData.bonusRewards is { Count: > 0 }) {
                for (int i = 0; i < questData.bonusRewards.Count; i++) {
                    CreateRewardItemsBasedOnType(questData.bonusRewards[i], true);
                }
            }
        }

        private void CreateRewardItemsBasedOnType(QuestReward reward, bool bonusReward = false) {
            // Currency rewards
            if (reward.RewardType is CurrencyRewardSO currencyReward) {
                CreateCurrencyRewardItem(currencyReward.CurrencyType.Currency, reward.RewardAmount);
                return;
            }

            // Item rewards
            if (reward.RewardType is ItemRewardSO itemReward) {
                if (!bonusReward) {
                    CreateRewardItem(reward, normalRewardUIPanel.transform);
                } else {
                    CreateRewardItem(reward, bonusRewardUIPanel.transform);
                }
                return;
            }
        }

        private void CreateCurrencyRewardItem(Currency currencyType, int amount) {
            switch (currencyType) {
                case Currency.GALACTIC_CREDITS:
                    PrimaryCurrencyRewardText.text = $"{amount:n0}";
                    PrimaryRewardContainer.SetActive(true);
                    break;
                case Currency.BIT_SHARDS:
                    SecondaryCurrencyRewardText.text = $"{amount:n0}";
                    SecondaryRewardContainer.SetActive(true);
                    break;
                case Currency.QUANTUMITE:
                    TertiaryCurrencyRewardText.text = $"{amount:n0}";
                    TertiaryRewardContainer.SetActive(true);
                    break;
            }
        }

        private void CreateRewardItem(QuestReward rewardData, Transform parent) {
            // Create the prefab
            GameObject rewardItem = Instantiate(rewardItemPrefab, parent);
            _rewardItems.Add(rewardItem);

            // Get the RewardItemUI component
            RewardItemUI rewardUI = rewardItem.GetComponent<RewardItemUI>();

            // Update the reward UI with the appropriate data
            rewardUI.SetQuestRewardUI(
                rewardData.RewardType.GetIcon(),
                rewardData.RewardType.GetRarityColor(),
                rewardData.RewardType.GetRarityBorderColor(),
                rewardData.RewardAmount);
        }
        #endregion

    }

}