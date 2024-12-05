/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class TooltipSystem : MonoBehaviour {

        #region VARIABLES - USE SUB-REGIONS 
        #region INSTANCING
        private static TooltipSystem _instance;
        public static TooltipSystem Instance { get { return _instance; } }
        #endregion
        #region CACHE
        public Tooltip tooltip;
        #endregion
        #endregion


        #region SETUP
        protected void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
            } else {
                _instance = this;
            }
        }
        #endregion

        #region PUBLIC METHODS
        public static void Show(string content, string header = "") {
            Instance.tooltip.SetText(content, header);
            Instance.tooltip.gameObject.SetActive(true);
        }
        public static void Hide() {
            Instance.tooltip.gameObject.SetActive(false);
        }
        #endregion

    }

}