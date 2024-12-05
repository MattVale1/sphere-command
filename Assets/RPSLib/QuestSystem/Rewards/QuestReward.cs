/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
namespace RPSCore {

    [System.Serializable]
    public class QuestReward {

        public RewardTypeSO RewardType;
        public int RewardAmount;


        // Get rarity of the reward, for sorting.
        public RarityLevel GetRarity() {
            if (RewardType is CurrencyRewardSO currencyReward) {
                return currencyReward.CurrencyType.ItemRarity;
            }

            if (RewardType is ItemRewardSO itemReward) {
                return itemReward.itemReward.ItemRarity;
            }

            return RarityLevel.COMMON;
        }

        // Rewards may come in, currently, 2 types of ScriptableObjects, let's cover both types...
        public void Reward() {
            if (RewardType is CurrencyRewardSO currencyReward) {
                PlayerInventoryService.Instance.AddCurrency(currencyReward.CurrencyType, TransactionType.ADD, RewardAmount);
            }

            if (RewardType is ItemRewardSO itemReward) {
                PlayerInventoryService.Instance.AddItem(itemReward.itemReward, TransactionType.ADD, RewardAmount);
            }
        }

    }

}