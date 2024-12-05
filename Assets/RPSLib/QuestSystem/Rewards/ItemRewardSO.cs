/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [CreateAssetMenu(fileName = "New Item Reward", menuName = "Quests/New Item Reward")]
    public class ItemRewardSO : RewardTypeSO {

        public ItemSO itemReward;

        public override Sprite GetIcon() {
            return itemReward.ItemIcon;
        }

        public override Color GetRarityColor() {
            return itemReward.GetRarityPrimaryColor();
        }

        public override Color GetRarityBorderColor() {
            return itemReward.GetRarityColorBorder();
        }

    }

}