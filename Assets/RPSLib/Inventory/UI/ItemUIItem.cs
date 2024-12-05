/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPSCore {

    public class ItemUIItem : ButtonInteractor {

        #region Private Properties
        private RectTransform _transform;
        private Button _button;
        private CanvasGroup _canvasGroup;
        private InventorySlot _inventorySlot;
        #endregion

        #region Public Properties       
        public Image ItemImage;
        public Image RarityPrimaryImage;
        public Image RarityBorderImage;
        public Image RarityBorderGlowImage;
        public Image RaritySecondaryImage;
        public GameObject IllegalImage;
        public Image AmountContainerImage;
        public TextMeshProUGUI AmountText;

        public ItemSO Item { get; set; }
        public int Amount { get; set; }
        #endregion


        #region Public Methods
        public void SetData(ItemSO itemSO, int amount) {
            _transform = GetComponent<RectTransform>();
            _button = GetComponent<Button>();
            _canvasGroup = GetComponent<CanvasGroup>();

            Item = itemSO;

            ItemImage.sprite = Item.ItemIcon;

            RarityPrimaryImage.color = Item.GetRarityPrimaryColor();
            RarityBorderImage.color = Item.GetRarityColorBorder();
            RarityBorderGlowImage.color = Item.GetRarityColorBorder();
            RaritySecondaryImage.color = Item.GetRaritySecondaryColor();
            Color amountContainerColor = Item.GetRarityColorBorder();
            amountContainerColor.a = .25f;
            AmountContainerImage.color = amountContainerColor;

            IllegalImage.SetActive(Item.Illegal);
            if (Item.Illegal) {
                RarityBorderImage.color = ItemRarityHandler.Instance.RarityColors.IllegalBorder;
                AmountContainerImage.color = ItemRarityHandler.Instance.RarityColors.IllegalBorder;
            }

            UpdateValue(amount, false);
        }

        public void UpdateValue(int amount, bool destroyIfZero) {
            Amount = amount;
            AmountText.text = $"{Amount:n0}"; // 1,234,456 - No decimals

            if (destroyIfZero && Amount <= 0) {
                _inventorySlot.SetItem(null);
                Destroy(gameObject);
            }
        }

        public RectTransform GetRectTransform() {
            return _transform;
        }

        public void SetInventorySlot(InventorySlot slot) {
            _inventorySlot = slot;
        }

        public InventorySlot GetInventorySlot() {
            return _inventorySlot;
        }

        public void DisableItemButton() {
            _button.interactable = false;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void EnableItemButton() {
            _button.interactable = true;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
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
                if (PlayerInventoryService.Instance.HasItemSelected()) {
                    return;
                }
                // Select this slot item if we don't already have one selected...
                if (!PlayerInventoryService.Instance.HasItemSelected()) {
                    PlayerInventoryService.Instance.SelectItem(this);
                }
            }
        }
        #endregion

    }

}