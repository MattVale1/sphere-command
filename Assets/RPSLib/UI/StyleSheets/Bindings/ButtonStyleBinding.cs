/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine.UI;

namespace RPSCore {

    public class ButtonStyleBinding : UIStyleBindingBase {

        #region Private Properties
        private Button _button;
        private UIButtonStyleSheet _styleSheet;
        #endregion


        #region Private Methods
        public override void ApplyStyling() {
            if (_button == null) {
                if (TryGetComponent(out Button btn)) {
                    _button = btn;
                } else {
                    RPSLib.Debug.Log("[" + name + "] :: No Button component found on GameObject: " + gameObject.name, RPSLib.Debug.Style.Warning);
                    return;
                }
            }

            _styleSheet = styleSheet as UIButtonStyleSheet;

            if (_styleSheet == null) {
                RPSLib.Debug.Log("[" + name + "] Failed to get Style Sheet of type: UIButtonStyleSheet on: " + gameObject.name, RPSLib.Debug.Style.Warning);
                return;
            }

            _button.colors = _styleSheet.style;
            _button.image.sprite = _styleSheet.primaryImage;
        }
        #endregion

    }

}