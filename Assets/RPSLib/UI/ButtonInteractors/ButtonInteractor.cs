/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPSCore {

    public class ButtonInteractor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

        public virtual void OnPointerEnter(PointerEventData eventData) {
            if (AudioManager.Instance == null) {
                RPSLib.Debug.Log("ButtonInteractor :: No AudioManager found, is it in the scene?", RPSLib.Debug.Style.Warning);
                return;
            }

            AudioManager.Instance.PlayUISound(AudioManager.UIClipType.BUTTON_HOVER, true);
        }

        public virtual void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (AudioManager.Instance == null) {
                    RPSLib.Debug.Log("ButtonInteractor :: No AudioManager found, is it in the scene?", RPSLib.Debug.Style.Warning);
                    return;
                }

                AudioManager.Instance.PlayUISound(AudioManager.UIClipType.BUTTON_CLICK, true);
            }
        }

        public virtual void OnPointerExit(PointerEventData eventData) {

        }

    }

}