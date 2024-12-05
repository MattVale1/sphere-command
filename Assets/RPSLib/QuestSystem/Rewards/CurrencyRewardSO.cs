/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [System.Serializable]
    [CreateAssetMenu(fileName = "New Item Reward", menuName = "Quests/New Item Reward")]
    public class CurrencyRewardSO : RewardTypeSO {

        public CurrencySO CurrencyType;

        public override Sprite GetIcon() {
            return CurrencyType.CurrencyIcon;
        }

        public override Color GetRarityBorderColor() {
            return CurrencyType.CurrencyColor;
        }

        public override Color GetRarityColor() {
            return CurrencyType.CurrencyColor;
        }
    }

}