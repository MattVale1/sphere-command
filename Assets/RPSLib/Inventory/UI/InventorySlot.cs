/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using UnityEngine.EventSystems;

namespace RPSCore {

    public class InventorySlot : ButtonInteractor {

        #region Private Properties
        private ItemUIItem _item;
        #endregion


        #region Public Methods
        public void SetItem(ItemUIItem item) {
            _item = item;
        }

        public bool IsOccupied() {
            return _item != null;
        }

        // ----- Pointer Events ----- //
        // -------------------------- //
        public override void OnPointerEnter(PointerEventData eventData) {
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData) {
            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData) {
            base.OnPointerClick(eventData);

            // Ensure left click only
            if (eventData.button == PointerEventData.InputButton.Left) {
                // If we have an item already selected...
                if (PlayerInventoryService.Instance.HasItemSelected(out ItemUIItem selectedItem)) {
                    // If this is an empty slot, tell inventory to move item here...
                    if (!IsOccupied()) {
                        //RPS.Debug.Log($"Slot is empty, adding item {selectedItem.Item.ItemID}", RPS.Debug.Style.Success);
                        PlayerInventoryService.Instance.MoveItemToSlot(selectedItem, this);
                    } else {
                        //RPS.Debug.Log($"Slot is occupied by {_item.Item.ItemID}, can't add {selectedItem.Item.ItemID}", RPS.Debug.Style.Warning);
                    }
                }
            }
        }
        #endregion

    }

}