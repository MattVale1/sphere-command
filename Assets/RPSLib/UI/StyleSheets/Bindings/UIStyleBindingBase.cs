/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    [DefaultExecutionOrder(1)]
    public class UIStyleBindingBase : MonoBehaviour {

        #region Public Properties

        public StyleSheetBase styleSheet;

        #endregion


        #region Unity Flow

        public virtual void Awake() {
            ApplyStyling();
            UIStyleManager.OnUIStyleChanged += ApplyStyling;
        }

        public virtual void OnDestroy() {
            UIStyleManager.OnUIStyleChanged -= ApplyStyling;
        }

        #endregion


        #region Public Methods

        public virtual void ApplyStyling() { }

        #endregion

    }

}