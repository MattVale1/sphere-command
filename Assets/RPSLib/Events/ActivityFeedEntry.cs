/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPSCore {

    public class ActivityFeedEntry : MonoBehaviour {

        public CanvasGroup CanvasGroup;
        public Image EntryBackground;
        public Image EntryIcon;
        public TextMeshProUGUI EntryText;


        public void SetData(Sprite sprite, Color color, string text, float startHide, float hide) {
            CanvasGroup.alpha = 1f;
            color.a = 0.1f;
            EntryBackground.color = color;
            EntryIcon.sprite = sprite;
            EntryText.text = text;
            LeanTween.cancel(gameObject);
            LeanTween.alphaCanvas(CanvasGroup, 0f, hide - startHide).setDelay(startHide);
        }

    }

}