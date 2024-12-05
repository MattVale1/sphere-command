/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine;

namespace RPSCore {

    public static class CameraSyncHandler_Persistent {

        private static Vector3 _storedEulerAngles = Vector3.zero;

        // TODO :: Could we just use an event here instead?
        public static bool FetchExpected = false;


        public static void StoreCameraRotation(Vector3 rot, bool nextEnabledCameraShouldFetch) {
            _storedEulerAngles = rot;
            FetchExpected = nextEnabledCameraShouldFetch;
            //RPS.Debug.Log("CameraSyncHandler :: Stored camera rotation: " + _storedEulerAngles + ". Fetch expected on next camera: " + FetchExpected, RPS.Debug.Style.Success);
        }

        public static Vector3 FetchCameraRotation(bool shouldClear = true) {
            FetchExpected = false;

            //RPS.Debug.Log("CameraSyncHandler :: Fetched camera rotation: " + _storedEulerAngles, RPS.Debug.Style.Normal);

            if (shouldClear) {
                Vector3 returnVal = _storedEulerAngles;
                _storedEulerAngles = default;
                return returnVal;
            }

            return _storedEulerAngles;
        }

    }

}