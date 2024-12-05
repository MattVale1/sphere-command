/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPSCore {

    public class HoverDropPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        public GameObject panelToDisplay;

        protected void Start() {
            panelToDisplay.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            panelToDisplay.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            panelToDisplay.SetActive(false);
        }

    }

}