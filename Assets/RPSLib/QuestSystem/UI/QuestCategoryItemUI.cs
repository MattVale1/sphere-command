/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class QuestCategoryItemUI : MonoBehaviour {

        public Image categoryImage;
        public TextMeshProUGUI categoryName;

        public void SetData(Sprite icon, string name) {
            categoryImage.sprite = icon;
            categoryName.text = name;
        }

    }

}