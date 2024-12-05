/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPSCore {

    [Serializable]
    public class GameSaveData : MonoBehaviour {

        #region Instancing
        public static GameSaveData Instance;

        protected void Awake() {
            Instance = this;
        }
        #endregion

        #region Saveable Data Sets
        public List<QuestData> ActiveQuests = new();
        public List<QuestData> CompletedQuests = new();
        //public PlayerInventoryData playerInventoryData;
        #endregion

    }

}