/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using UnityEngine;

namespace RPSCore {

    [DefaultExecutionOrder(0)]
    public class UIStyleManager : MonoBehaviour {

        #region Static Properties

        public static UIStyleManager Instance { get; private set; }

        #endregion

        #region Events

        public static event Action OnUIStyleChanged; // This can be used for player-driven changes in future

        #endregion


        #region Unity Flow   

        private void Awake() {
            Instance = this;
        }

        #endregion

        #region Public Methods

        public void UpdateUIStyle() {
            OnUIStyleChanged?.Invoke();
        }

        #endregion

    }

}