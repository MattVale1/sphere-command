/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class ItemRarityHandler : MonoBehaviour {

        #region Public Properties
        public static ItemRarityHandler Instance { get; private set; }
        public RarityColorSO RarityColors;
        #endregion


        #region Unity Flow
        protected void Awake() {
            Instance = this;
        }
        #endregion

    }

}