/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using Steamworks;
using UnityEngine;

namespace RPSCore.Steamworks {

    public class SteamAchievements : MonoBehaviour {

        #region Instancing
        public static SteamAchievements Instance { get; protected set; }
        #endregion

        #region Private Properties
        private CGameID m_GameID;
        #endregion

        #region Public Properties
        public enum Achievement : int {
            TEST_01,
            TEST_02
        }

        public Achievement_t[] m_Achievements = new Achievement_t[] {
            new (Achievement.TEST_01, "Achievement Title 01", "Achievement description 01."),
            new (Achievement.TEST_02, "Achievement Title 02", "Achievement description 02.")
        };
        #endregion

        #region Callbacks
        protected Callback<UserStatsReceived_t> m_UserStatsReceived;
        protected Callback<UserStatsStored_t> m_UserStatsStored;
        protected Callback<UserAchievementStored_t> m_UserAchievementStored;
        #endregion

        #region Sub-classes
        public class Achievement_t {
            public Achievement m_eAchievementID;
            public string m_strName;
            public string m_strDescription;
            public bool m_bAchieved;
            /// < summary>Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/628650/// </summary>
            /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
            /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
            /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
            public Achievement_t(Achievement achievementID, string name, string desc) {
                m_eAchievementID = achievementID;
                m_strName = name;
                m_strDescription = desc;
                m_bAchieved = false;
            }
        }
        #endregion

        #region Unity Flow
        protected void Awake() {
            Instance = this;
        }

        protected void OnEnable() {
            if (!SteamManager.Initialized)
                return;

            m_GameID = new CGameID(SteamUtils.GetAppID());

            m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
            m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

            GetPlayerStats();
        }
        #endregion

        #region Private Methods
        private void OnUserStatsReceived(UserStatsReceived_t pCalblack) {
            if (!SteamManager.Initialized)
                return;

            if ((ulong)m_GameID == pCalblack.m_nGameID) {
                if (EResult.k_EResultOK == pCalblack.m_eResult) {
                    // Load achievements
                    foreach (Achievement_t ach in m_Achievements) {
                        bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
                        if (ret) {
                            ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
                            ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
                        } else {
                            Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
                        }
                    }
                }
            }
        }

        private void OnUserStatsStored(UserStatsStored_t pCallback) {
            // we may get callbacks for other games' stats arriving, ignore them
            if ((ulong)m_GameID == pCallback.m_nGameID) {
                if (EResult.k_EResultOK == pCallback.m_eResult) {
                    Debug.Log("StoreStats - success");
                } else if (EResult.k_EResultInvalidParam == pCallback.m_eResult) {
                    // One or more stats we set broke a constraint. They've been reverted,
                    // and we should re-iterate the values now to keep in sync.
                    Debug.Log("StoreStats - some failed to validate");
                    // Fake up a callback here so that we re-load the values.
                    UserStatsReceived_t callback = new() {
                        m_eResult = EResult.k_EResultOK,
                        m_nGameID = (ulong)m_GameID
                    };
                    OnUserStatsReceived(callback);
                } else {
                    Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
                }
            }
        }

        private void OnAchievementStored(UserAchievementStored_t pCallback) {
            // We may get callbacks for other games' stats arriving, ignore them
            if ((ulong)m_GameID == pCallback.m_nGameID) {
                if (0 == pCallback.m_nMaxProgress) {
                    Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
                } else {
                    Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
                }
            }
        }

        private void StorePlayerStats() {
            SteamUserStats.StoreStats(); // Make sure Steam stores the new achievement after we get one
            SteamAPI.RunCallbacks(); // Make sure steam shows that we have new achievements
        }

        private void GetPlayerStats() {
            SteamUserStats.RequestCurrentStats();
        }
        #endregion

        #region Public Methods
        public Achievement_t GetAchievement(Achievement achievementID) {
            Achievement_t desiredAchievement = m_Achievements[(int)achievementID];
            return desiredAchievement;
        }

        public void UnlockAchievement(Achievement_t achievement) {
            if (!SteamManager.Initialized) {
                Debug.LogWarning("<color=orange>Steam not initialized, trying to unlock ACH: </color>" + achievement.m_strName);
                return;
            }
            if (achievement.m_bAchieved) {
                // Already unlocked this, do nothing
                Debug.Log("<color=green>ACHIEVEMENTS:</color> Already got: " + achievement.m_strName);
                return;
            }
            achievement.m_bAchieved = true;
            Debug.Log("<color=green>ACHIEVEMENTS:</color> Unlocking: " + achievement.m_strName);
            SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
            StorePlayerStats(); // Make sure we are updating the achievement status on Steam
        }

        public void ResetAchievement(Achievement achievement) {
            SteamUserStats.ClearAchievement(m_Achievements[(int)achievement].m_strName);
            StorePlayerStats(); // Make sure we are updating the achievement status on Steam
        }

        public void ResetAllStatsAndAchievements() {
            SteamUserStats.ResetAllStats(true);
            StorePlayerStats(); // Make sure we are updating the achievement status on Steam
        }
        #endregion

    }

}