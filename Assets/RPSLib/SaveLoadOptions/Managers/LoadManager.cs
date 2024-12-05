/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using UnityEngine;

namespace RPSCore {

    public class LoadManager : MonoBehaviour {

        #region Load Game Save Data
        /// <summary>
        /// Begin the process of loading all saved data.
        /// </summary>
        /// <param name="saveData">The SaveDataContainer file.</param>
        public void LoadGameData(GameSaveData saveData) {
            // Loading of game "data"
            LoadQuests(saveData);
            // Loading of game "objects"
            // LoadSavedGameObject(saveData);
        }

        /// <summary>
        /// Load all quests in our save file.
        /// </summary>
        private void LoadQuests(GameSaveData saveData) {
            try {
                QuestManager.Instance.LoadCompletedQuests(saveData.CompletedQuests);
                QuestManager.Instance.LoadActiveQuests(saveData.ActiveQuests);
            } catch (Exception e) {
                Debug.Log("<PrimaryColor=red>Loading <PrimaryColor=yellow>QUESTS</PrimaryColor> failed because:</PrimaryColor>\n" + e.Message);
            }
        }
        #endregion

        #region Loading Settings Data
        public void LoadSettingsData(GameSettingsData settingsData) {
            GameOptionsManager.Instance.LoadSettings(settingsData.settingsData);
        }
        #endregion

    }

}