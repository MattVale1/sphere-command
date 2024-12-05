/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine.UI;

namespace RPSCore {

    public class ToggleListStyleBinding : UIStyleBindingBase {

        #region Private Properties
        private Toggle _toggle;
        private UIButtonStyleSheet _styleSheet;
        #endregion


        #region Private Methods
        public override void ApplyStyling() {
            if (_toggle == null) {
                if (TryGetComponent(out Toggle toggle)) {
                    _toggle = toggle;
                } else {
                    RPSLib.Debug.Log("[" + name + "] :: No Toggle component found on GameObject: " + gameObject.name, RPSLib.Debug.Style.Warning);
                    return;
                }
            }

            _styleSheet = styleSheet as UIButtonStyleSheet;

            if (_styleSheet == null) {
                RPSLib.Debug.Log("[" + name + "] Failed to get Style Sheet of type: UIButtonStyleSheet on: " + gameObject.name, RPSLib.Debug.Style.Warning);
                return;
            }

            _toggle.colors = _styleSheet.style;
            _toggle.image.sprite = _styleSheet.primaryImage;
        }
        #endregion

    }

}