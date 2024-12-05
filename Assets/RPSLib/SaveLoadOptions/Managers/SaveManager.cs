/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPSCore {

    [RequireComponent(typeof(LoadManager))]
    [RequireComponent(typeof(SaveDataContainer))]
    public class SaveManager : MonoBehaviour {

        #region VARIABLES

        #region Instancing
        public static SaveManager Instance;
        #endregion

        #region Required Classes
        public LoadManager loadManager;
        #endregion

        #region Save Data
        private string JSONData; // Save data in string format
        [Header("Save Data Components")]
        public SaveDataContainer saveDataContainer;
        //public GameSaveData gameSaveData; // Container for all game data
        //public GameSettingsData gameSettingsData; // Container for all settings data
        #endregion

        #region Filepaths
        public readonly string gameSettingsFolderName = "/gamesettings"; // The subfolder the game settings will live in
        public readonly string gameSaveFolderName = "/saves/"; // The subfolder the saved games will live in
        #endregion

        #region Public States
        public static bool isLoadingFromSave;
        #endregion

        #region Events
        public delegate void OnGameSave();
        public static OnGameSave onGameSave;
        #endregion

        #region Save Game String
        [HideInInspector]
        public string selectedSaveGameFileName; // Used for loading a game transferring between scenes.
        #endregion
        #endregion


        #region Setup
        protected void Awake() {
            Instance = this;
        }

        protected void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// Have we just loaded a scene? If so this method will run, starting the load process if needed.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        protected void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            SaveManagerUI.Instance.ToggleSaveButton(false);
            // Check here to see what scene we have loaded, we should only run this on the main game scene(s).
            if (SceneManager.GetActiveScene().name == SceneNameManager.GALAXY) {
                SaveManagerUI.Instance.ToggleSaveButton(true);
                CheckIfLoadingFromSaveFile();
            }
        }

        /// <summary>
        /// If we are loading a scene here, run the Load method after waiting for n amount of seconds, this is to give other scripts the chance to Init.
        /// </summary>
        private void CheckIfLoadingFromSaveFile() {
            if (isLoadingFromSave)
                Invoke(nameof(LoadGameFile), 0.2f);
        }
        #endregion

        #region File Checks
        /// <summary>
        /// Check if the chosen save file already exists.
        /// </summary>
        public bool SaveFileExists(string fileName) {
            return File.Exists(Application.persistentDataPath + gameSaveFolderName + fileName + ".json");
        }
        #endregion

        #region Button Handlers
        public void SaveGame(string saveGameID) {
            SaveGameFile(saveDataContainer.GameData, saveGameID);
        }

        public void LoadGame(string saveGameID) {
            if (!SaveFileExists(saveGameID)) {
                RPSLib.Debug.Log("SaveManager :: No save file exists, not loading anything.", RPSLib.Debug.Style.Warning);
                return;
            }
            isLoadingFromSave = true;
            selectedSaveGameFileName = saveGameID;

            RPSLib.SceneManagement.LoadScene(SceneNameManager.GALAXY, LoadSceneMode.Additive, true);
        }

        public void SaveOptions() {
            SaveGameSettings(saveDataContainer.SettingsData);
        }

        public void LoadOptions() {
            LoadGameSettings();
        }
        #endregion

        #region Save & Load Methods
        /// <summary>
        /// Save the game data to a file.
        /// </summary>
        private void SaveGameFile(GameSaveData data, string saveGameID) {
            onGameSave();
            try {
                JSONData = JsonUtility.ToJson(data);
                File.WriteAllText(Application.persistentDataPath + gameSaveFolderName + saveGameID + ".json", JSONData);
            } catch (Exception e) {
                RPSLib.Debug.Log("SaveManager :: Failed to save game data! Error:\n" + e.Message, RPSLib.Debug.Style.Error);
            }
        }

        /// <summary>
        /// Load our game save data and place it in the SaveDataContainer class. 
        /// </summary>
        private void LoadGameFile() {
            isLoadingFromSave = false; // Reset isLoading state so we don't accidentally load again from elsewhere
            try {
                if (File.Exists(Application.persistentDataPath + gameSaveFolderName + selectedSaveGameFileName + ".json")) {
                    JSONData = File.ReadAllText(Application.persistentDataPath + gameSaveFolderName + selectedSaveGameFileName + ".json");
                    JsonUtility.FromJsonOverwrite(JSONData, saveDataContainer.GameData); // Use FromJsonOverwrite so we can have MonoBehaviour.
                                                                                         // Begin the load process for different systems
                    loadManager.LoadGameData(saveDataContainer.GameData);
                } else {
                    RPSLib.Debug.Log("SaveManager :: No Save Data found!", RPSLib.Debug.Style.Error);
                }
            } catch (Exception e) {
                RPSLib.Debug.Log("SaveManager :: We failed to load game data. Error:\n" + e.Message, RPSLib.Debug.Style.Error);
            }
        }

        /// <summary>
        /// Save the game settings to a file.
        /// </summary>
        private void SaveGameSettings(GameSettingsData data) {
            try {
                JSONData = JsonUtility.ToJson(data.settingsData); // Serialize to JSON
                File.WriteAllText(Application.persistentDataPath + gameSettingsFolderName + ".json", JSONData); // Save to file
            } catch (Exception e) {
                RPSLib.Debug.Log("SaveManager :: Failed to save game settings! Error:\n" + e.Message, RPSLib.Debug.Style.Error);
            }
        }

        /// <summary>
        /// Load our game save data and place it in the SaveDataContainer class. 
        /// </summary>
        private void LoadGameSettings() {
            try {
                if (File.Exists(Application.persistentDataPath + gameSettingsFolderName + ".json")) {
                    JSONData = File.ReadAllText(Application.persistentDataPath + gameSettingsFolderName + ".json");
                    JsonUtility.FromJsonOverwrite(JSONData, saveDataContainer.SettingsData);  // Use FromJsonOverwrite so we can have MonoBehaviour.
                                                                                              // Begin the load process for different systems
                    loadManager.LoadSettingsData(saveDataContainer.SettingsData);
                } else {
                    RPSLib.Debug.Log("SaveManager :: No game settings file found!", RPSLib.Debug.Style.Error);
                }
            } catch (Exception e) {
                RPSLib.Debug.Log("SaveManager :: Failed to load game settings! Error:\n" + e.Message, RPSLib.Debug.Style.Error);
            }
        }
        #endregion

        #region Save Game Deletion
        public void DeleteSaveGame(string saveGameID) {
            try {
                if (File.Exists(Application.persistentDataPath + gameSaveFolderName + saveGameID + ".json")) {
                    File.Delete(Application.persistentDataPath + gameSaveFolderName + saveGameID + ".json");
                } else {
                    RPSLib.Debug.Log("SaveManager :: Save file does not exist to delete.", RPSLib.Debug.Style.Error);
                }
            } catch (Exception e) {
                RPSLib.Debug.Log("SaveManager :: Failed to delete the save file. Error:\n" + e.Message, RPSLib.Debug.Style.Error);
            }
        }
        #endregion

    }

}