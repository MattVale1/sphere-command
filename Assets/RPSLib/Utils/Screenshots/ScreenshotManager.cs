/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public class ScreenshotManager : MonoBehaviour {

        public static ScreenshotManager Instance;

        public KeyCode keyForScreenshot;


        private void Awake() {
            Instance = this;
        }

        void Update() {
            GetInput();
        }

        void GetInput() {
            if (Input.GetKeyDown(keyForScreenshot)) {
                RPSLib.ScreenshotHandler.TakeScreenshot("Screenshot", true);
            }
        }

    }

}