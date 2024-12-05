/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RPSEditor {

    public class SceneLoader : Editor {

        /// <summary>
        /// Paths of all important scenes.
        /// </summary>
        private static readonly string RPSIntroScene = "Assets/RPSLib/Scenes/RPSIntro";
        private static readonly string GlobalManagersScene = "Assets/RPSLib/Scenes/GlobalManagers";

        private static readonly string MenuScene = "Assets/Game/SCENES/MENUS/MainMenu";
        private static readonly string GalaxyScene = "Assets/Game/SCENES/GAMEPLAY/GalaxyScene";
        private static readonly string PlanetarySystemScene = "Assets/Game/SCENES/GAMEPLAY/PlanetarySystemScene";

        private static readonly string ShipControllerScene = "Assets/Game/SCENES/PROTOTYPING/ShipControllerTesting";


        // ***** RPS Scenes ***** //
        // ---------------------- //
        [MenuItem("RPSTools/0_RPSIntro (boot_0)", false, 0)]
        public static void OpenRPSIntroScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(RPSIntroScene + ".unity");
        }

        [MenuItem("RPSTools/1_GlobalManagers (boot_1)", false, 0)]
        public static void OpenGlobalManagersScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(GlobalManagersScene + ".unity");
        }

        // ***** Game Scenes ***** //
        // ----------------------- //
        [MenuItem("RPSTools/MainMenu", false, 20)]
        public static void OpenMainMenuScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(MenuScene + ".unity");
        }

        [MenuItem("RPSTools/GalaxyScene", false, 21)]
        public static void OpenGalaxyScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(GalaxyScene + ".unity");
        }

        [MenuItem("RPSTools/PlanetarySystemScene", false, 22)]
        public static void OpenPlanetaryScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(PlanetarySystemScene + ".unity");
        }

        // ***** Game Scenes ***** //
        // ----------------------- //
        [MenuItem("RPSTools/ShipControllerTesting", false, 40)]
        public static void OpenShipControllerScene() {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(ShipControllerScene + ".unity");
        }

    }

}