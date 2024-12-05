/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using Steamworks;
using UnityEngine;

namespace RPSCore.Steamworks {

    public class SteamScript : MonoBehaviour {

        #region Instancing
        public static SteamScript Instance { get; private set; }
        #endregion

        #region Private Properties

        #endregion

        #region Public Properties

        public PlatformDependantSDKManager platformSDKMan;

        /// <summary>Get or set the MSteamName. The Steam username for the account logged in...</summary>
        public string MSteamName {
            get { return SteamFriends.GetPersonaName(); }
            set { SteamFriends.GetPersonaName(); }
        }
        /// <summary>Get or set the MSteamID. The SteamID for the account logged in...</summary>
        public CSteamID MSteamID {
            get { return SteamUser.GetSteamID(); }
            set { SteamUser.GetSteamID(); }
        }
        #endregion

        #region Callbacks
        protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
        protected Callback<FriendRichPresenceUpdate_t> m_FriendRichPresenceUpdate;
        protected Callback<GameRichPresenceJoinRequested_t> m_GameRichPresenceJoinRequested;
        #endregion


        #region Unity Flow
        protected void Awake() {
            Instance = this;
        }

        protected void OnEnable() {
            if (platformSDKMan.platform == PlatformDependantSDKManager.Platform.Steam) {
                if (SteamManager.Initialized) {
                    InitCallbacks();
                }
            }
        }
        #endregion

        #region Private Methods
        private void InitCallbacks() {
            m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            m_FriendRichPresenceUpdate = Callback<FriendRichPresenceUpdate_t>.Create(OnFriendRichPresenceUpdate);
            m_GameRichPresenceJoinRequested = Callback<GameRichPresenceJoinRequested_t>.Create(OnGameRichPresenceJoinRequested);
        }

        private void OnGameOverlayActivated(GameOverlayActivated_t pCallback) {
            if (pCallback.m_bActive != 0) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1;
            }
        }

        private void OnFriendRichPresenceUpdate(FriendRichPresenceUpdate_t pCallback) {
            Debug.Log("[" + FriendRichPresenceUpdate_t.k_iCallback + " - FriendRichPresenceUpdate] - " + pCallback.m_steamIDFriend + " -- " + pCallback.m_nAppID);
        }

        private void OnGameRichPresenceJoinRequested(GameRichPresenceJoinRequested_t pCallback) {
            Debug.Log("[" + GameRichPresenceJoinRequested_t.k_iCallback + " - GameRichPresenceJoinRequested] - " + pCallback.m_steamIDFriend + " -- " + pCallback.m_rgchConnect);
        }
        #endregion

        #region Public Methods
        public void SetRichFriendPresence(string hKey) {
            if (SteamManager.Initialized) {
                SteamFriends.SetRichPresence("steam_display", hKey);
            }
        }
        #endregion

    }

}