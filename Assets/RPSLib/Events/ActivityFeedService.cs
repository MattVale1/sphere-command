/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections.Generic;
using UnityEngine;

namespace RPSCore {

    public class ActivityFeedService : MonoBehaviour {

        public static ActivityFeedService Instance { get; private set; }

        #region Private Properties
        private Dictionary<GameObject, ActivityFeedEntry> _entries = new();
        #endregion

        #region Public Properties
        public GameObject EntryPrefab;
        public int MaxEntries = 10;
        public Transform ActivityFeedTransform;
        public float TimeUntilHide = 5f;
        public float TimeUntilStartFade = 3f;
        #endregion


        #region Unity Flow
        protected void Awake() {
            Instance = this;

            for (int i = 0; i < MaxEntries; i++) {
                GameObject entryGameObject = Instantiate(EntryPrefab, ActivityFeedTransform);
                ActivityFeedEntry entry = entryGameObject.GetComponent<ActivityFeedEntry>();
                _entries.Add(entryGameObject, entry);
                entryGameObject.SetActive(false);
            }
        }
        #endregion

        #region Private Methods
        private void DisplayEntry(KeyValuePair<GameObject, ActivityFeedEntry> entry, Sprite sprite, Color color, string text) {
            entry.Value.SetData(sprite, color, text, TimeUntilStartFade, TimeUntilHide);
            entry.Key.SetActive(true);
            entry.Key.transform.SetAsLastSibling();
        }
        #endregion

        #region Public Methods
        public void AddNewEntry(Sprite entrySprite, Color entryColor, string entryText) {

            // Grab the first available entry slot and populate it with our activity...
            foreach (var entry in _entries) {
                if (!entry.Key.activeInHierarchy) {
                    DisplayEntry(entry, entrySprite, entryColor, entryText);
                    return;
                }
            }

            // If not free slot found, grab the top-most, closest to hiding...
            foreach (var entry in _entries) {
                if (entry.Key.transform.GetSiblingIndex() == 0) {
                    DisplayEntry(entry, entrySprite, entryColor, entryText);
                    return;
                }
            }
        }
        #endregion

    }

}