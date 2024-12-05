/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class CurrencyHandler : MonoBehaviour {

        #region Public Propertiies
        public static CurrencyHandler Instance { get; private set; }

        public CurrencySO GalacticCredits;
        public CurrencySO BitShards;
        public CurrencySO Quantumite;
        #endregion


        #region Unity Flow
        protected void Awake() {
            Instance = this;
        }
        #endregion

    }

}