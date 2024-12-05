/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using Steamworks;
using UnityEngine;

namespace RPSCore.Steamworks {

    public class PlatformDependantSDKManager : MonoBehaviour {

        #region Instancing
        public static PlatformDependantSDKManager Instance { get; private set; }
        #endregion
        #region Public Properties
        public Platform platform;
        public enum Platform {
            Standalone,
            Steam
        }
        #endregion

        #region Unity Flow
        protected void Start() {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (platform == Platform.Steam) {

            } else {
                SteamAPI.Shutdown();
            }
        }
        #endregion

    }

}