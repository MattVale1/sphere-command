/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPSCore {

    public class ItemHandler : MonoBehaviour {

        #region Private Properties
        public List<ItemSO> AllItems = new();
        #endregion

        #region Public Propertiies
        public static ItemHandler Instance { get; private set; }
        #endregion

        #region Unity Flow
        protected void Awake() {
            Instance = this;
        }
        #endregion

        #region Public Methods
        public ItemSO GetRandomItemSO(RarityLevel? rarityTarget = null) {
            if (rarityTarget == null) {
                return AllItems[Random.Range(0, AllItems.Count)];
            }

            List<ItemSO> filteredItemList = AllItems.Where(x => x.ItemRarity == rarityTarget).ToList();
            return filteredItemList[Random.Range(0, filteredItemList.Count)];
        }

        public ItemSO GetItemSOByID(string ID) {
            return AllItems.First(x => x.ItemID == ID);
        }

        public ItemSO GetItemByItemType(ItemType type) {
            List<ItemSO> filteredItemList = AllItems.Where(x => x.ItemType == type).ToList();
            return filteredItemList[Random.Range(0, filteredItemList.Count)];
        }
        #endregion
    }

}