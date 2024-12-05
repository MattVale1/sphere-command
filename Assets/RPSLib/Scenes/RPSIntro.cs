/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPSCore {

    public class RPSIntro : MonoBehaviour {

        private string gameName;
        private string year;
        private float loadMainMenuAfter = 5f;

        public TextMeshProUGUI copyrightText;


        #region SETUP
        private void Start() {
            gameName = GameVersion.GameName;
            year = DateTime.Now.Year.ToString();

            copyrightText.text =
                "Copyright Red Phoenix Studios "
                + year
                + ". Red Phoenix Studios and "
                + gameName
                + " are protected under copyright law.";

            Invoke(nameof(LoadMainMenu), loadMainMenuAfter);
            // Load our GlobalManagers scene...
            RPSLib.SceneManagement.LoadScene(SceneNameManager.GLOBAL_MANAGERS, LoadSceneMode.Additive, false);
        }
        #endregion

        #region INPUT
        private void Update() {
            if (Input.anyKeyDown) {
                LoadMainMenu();
            }
        }
        #endregion

        #region SCENE MANAGEMENT
        private void LoadMainMenu() {
            CancelInvoke();
            RPSLib.SceneManagement.LoadScene(SceneNameManager.MAIN_MENU, LoadSceneMode.Additive, true);
            RPSLib.SceneManagement.UnloadScene(SceneNameManager.RPS_INTRO);
        }
        #endregion

    }

}