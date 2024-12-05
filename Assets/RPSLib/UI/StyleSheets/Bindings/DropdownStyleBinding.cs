/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;

namespace RPSCore {

    public class DropdownStyleBinding : UIStyleBindingBase {

        #region Private Properties
        private TMP_Dropdown _dropdown;
        private UIButtonStyleSheet _styleSheet;
        #endregion


        #region Private Methods
        public override void ApplyStyling() {
            if (_dropdown == null) {
                if (TryGetComponent(out TMP_Dropdown dropdown)) {
                    _dropdown = dropdown;
                } else {
                    RPSLib.Debug.Log("[" + name + "] :: No TMP_Dropdown component found on GameObject: " + gameObject.name, RPSLib.Debug.Style.Warning);
                    return;
                }
            }

            _styleSheet = styleSheet as UIButtonStyleSheet;

            if (_styleSheet == null) {
                RPSLib.Debug.Log("[" + name + "] Failed to get Style Sheet of type: UIButtonStyleSheet on: " + gameObject.name, RPSLib.Debug.Style.Warning);
                return;
            }

            _dropdown.colors = _styleSheet.style;
            _dropdown.image.sprite = _styleSheet.primaryImage;
        }
        #endregion

    }

}