/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using UnityEngine;

namespace RPSCore {

    [Serializable]
    public class GameSettingsData : MonoBehaviour {

        #region Instancing
        public static GameSettingsData Instance;

        protected void Awake() {
            Instance = this;
        }
        #endregion

        #region Saveable Data Sets
        public SettingsData settingsData = new();
        #endregion

    }

}