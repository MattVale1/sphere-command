/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using UnityEngine;

namespace RPSCore {

    [Serializable]
    public class SaveDataContainer : MonoBehaviour {

        #region Instancing
        public static SaveDataContainer Instance;

        protected void Awake() {
            Instance = this;
        }
        #endregion

        #region Saveable Data Sets
        public GameSaveData GameData;
        public GameSettingsData SettingsData;
        #endregion

    }

}