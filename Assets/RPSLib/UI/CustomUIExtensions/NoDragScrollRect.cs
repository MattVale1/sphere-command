/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPSCore {

    public class NoDragScrollRect : ScrollRect {

        public override void OnBeginDrag(PointerEventData eventData) { }
        public override void OnDrag(PointerEventData eventData) { }
        public override void OnEndDrag(PointerEventData eventData) { }

    }

}