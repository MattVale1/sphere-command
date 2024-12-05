/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPSCore {

    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        private static LTDescr delay;
        public string header;
        [Multiline()]
        public string content;

        public void OnPointerEnter(PointerEventData eventData) {
            delay = LeanTween.delayedCall(0.25f, () => {
                TooltipSystem.Show(content, header);
            }).setIgnoreTimeScale(true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            LeanTween.cancel(delay.uniqueId);
            TooltipSystem.Hide();
        }

    }

}