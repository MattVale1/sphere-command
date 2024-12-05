/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class RewardItemUI : MonoBehaviour {

        public Image RewardImage;
        public Image RarityColorImage;
        public Image RarityBorderImage;
        public Image AmountContainerImage;
        public TextMeshProUGUI rewardAmountText;

        public void SetQuestRewardUI(Sprite spr, Color primaryColor, Color borderColor, int amount) {
            RewardImage.sprite = spr;
            Color mainColor = primaryColor;
            mainColor.a = primaryColor.a;
            RarityColorImage.color = mainColor;
            RarityBorderImage.color = borderColor;
            AmountContainerImage.color = borderColor;
            rewardAmountText.text = $"{amount:n0}"; // 1,234,567
        }

    }

}