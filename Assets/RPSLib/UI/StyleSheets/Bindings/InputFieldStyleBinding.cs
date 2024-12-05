/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;

namespace RPSCore {

    public class InputFieldStyleBinding : UIStyleBindingBase {

        #region Private Properties
        private TMP_InputField _inputField;
        private UIButtonStyleSheet _styleSheet;
        #endregion


        #region Private Methods
        public override void ApplyStyling() {
            if (_inputField == null) {
                if (TryGetComponent(out TMP_InputField inputField)) {
                    _inputField = inputField;
                } else {
                    RPSLib.Debug.Log("[" + name + "] :: No TMP_InputField component found on GameObject: " + gameObject.name, RPSLib.Debug.Style.Warning);
                    return;
                }
            }

            _styleSheet = styleSheet as UIButtonStyleSheet;

            if (_styleSheet == null) {
                RPSLib.Debug.Log("[" + name + "] Failed to get Style Sheet of type: UIButtonStyleSheet on: " + gameObject.name, RPSLib.Debug.Style.Warning);
                return;
            }

            _inputField.colors = _styleSheet.style;
            _inputField.image.sprite = _styleSheet.primaryImage;
        }
        #endregion

    }

}