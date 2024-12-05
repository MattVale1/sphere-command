/* Author:		Matthew Vale
 * Role:		Lead Game Developer
 * Company:		Red Phoenix Studios
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace RPSCore {

    public class PlayerInventoryService : MonoBehaviour {

        #region Instancing
        public static PlayerInventoryService Instance { get; private set; }
        #endregion

        #region Private Properties
        private int _maxInventorySlots = 64;
        private List<InventorySlot> _inventorySlots = new();

        private Dictionary<CurrencySO, int> _currencies = new();
        private List<CurrencyUIItem> _currencyUIItems = new();

        private Dictionary<ItemSO, int> _items = new();
        private List<ItemUIItem> _itemUIItems = new();

        private ItemUIItem _selectedItem;
        private InventorySlot _selectedItemInventorySlot;

        private InputAction _mousePosition;
        private InputAction _rightMouseButton;
        #endregion

        #region Public Propertiies
        public bool InventoryOpen => inventoryCanvas.enabled;
        public Canvas inventoryCanvas;
        public Canvas itemScrollRectCanvas;

        [Header("Inventory Slot Base")]
        public GameObject InventorySlotPrefab;

        [Header("Currencies UI")]
        public GameObject CurrencyUIItemPrefab;
        public RectTransform CurrencyPanelRoot;


        [Header("Items UI")]
        public GameObject ItemUIItemPrefab;
        public RectTransform ItemPanelRoot;

        #endregion


        #region Unity Flow
        protected void Awake() {
            Instance = this;

            CacheInput();

            CloseInventory();
            SpawnInventorySlots();
        }

        private void CacheInput() {
            _mousePosition = InputSystem.actions.FindAction(InputActionConstants.MousePosition);
            _rightMouseButton = InputSystem.actions.FindAction(InputActionConstants.RMB);
            _rightMouseButton.started += CancelSelectedItem;
        }

        protected void Start() {
            GiveStartingResources();
        }

        protected void Update() {
            if (Input.GetKeyDown(KeyCode.I)) {
                if (!InventoryOpen) {
                    OpenInventory();
                } else {
                    CloseInventory();
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                TEST_GiveRandomItems(1, Random.Range(8, 256));
            }

            if (Input.GetKeyDown(KeyCode.T)) {
                TEST_RemoveFirstStack();
            }

            // Right click to cancel item move
            if (_selectedItem != null) {
                _selectedItem.transform.position = _mousePosition.ReadValue<Vector2>();
            }
        }
        #endregion

        #region Private Methods
        private void SpawnInventorySlots() {
            for (int i = 0; i < _maxInventorySlots; i++) {
                InventorySlot newSlot = Instantiate(InventorySlotPrefab, ItemPanelRoot).GetComponent<InventorySlot>();
                _inventorySlots.Add(newSlot);
            }
        }

        private ItemUIItem CreateItemStack(ItemSO item, int amount) {
            // Create it...
            ItemUIItem newItem = Instantiate(ItemUIItemPrefab).GetComponent<ItemUIItem>();
            // Set it's initial Data...
            newItem.SetData(item, amount);
            // Add to the list of existing UI items...
            _itemUIItems.Add(newItem);

            return newItem;
        }

        private void AddStackToFirstEmptySlot(ItemUIItem newItem) {
            // Add to first empty inventory slot
            for (int i = 0; i < _inventorySlots.Count; i++) {
                if (!_inventorySlots[i].IsOccupied()) {
                    PlaceItemInInventorySlot(newItem, _inventorySlots[i]);
                    break;
                }
            }
        }

        private void UpdateCurrencyUI(CurrencySO currency) {
            _currencyUIItems.Find(x => x.Currency == currency).UpdateValue(_currencies[currency]);

            SortCurrencyUIItems(ResourceSortingType.RARITY);
        }

        private void SortCurrencyUIItems(ResourceSortingType sortingMethod) {
            switch (sortingMethod) {
                case ResourceSortingType.NONE:
                    _currencyUIItems.Sort((x, y) => x.Currency.ItemRarity.CompareTo(y.Currency.ItemRarity));
                    break;
                case ResourceSortingType.QUANTITY:
                    _currencyUIItems.Sort((x, y) => x.Amount.CompareTo(y.Amount));
                    break;
                case ResourceSortingType.RARITY:
                    _currencyUIItems.Sort((x, y) => x.Currency.ItemRarity.CompareTo(y.Currency.ItemRarity));
                    break;
                case ResourceSortingType.ALPHABETICALLY:
                    _currencyUIItems.Sort((x, y) => x.Currency.ItemID.CompareTo(y.Currency.ItemID));
                    break;
                default:
                    _currencyUIItems.Sort((x, y) => x.Currency.ItemRarity.CompareTo(y.Currency.ItemRarity));
                    break;
            }

            UpdateCurrencySortOrder();
        }

        private void SortInventoryUIItems(ResourceSortingType sortingMethod) {
            switch (sortingMethod) {
                case ResourceSortingType.NONE:
                    _itemUIItems.Sort((x, y) => x.Amount.CompareTo(y.Amount));
                    break;
                case ResourceSortingType.QUANTITY:
                    _itemUIItems.Sort((x, y) => x.Amount.CompareTo(y.Amount));
                    break;
                case ResourceSortingType.RARITY:
                    _itemUIItems.Sort((x, y) => x.Item.ItemRarity.CompareTo(y.Item.ItemRarity));
                    break;
                case ResourceSortingType.ALPHABETICALLY:
                    _itemUIItems.Sort((x, y) => x.Item.ItemID.CompareTo(y.Item.ItemID));
                    break;
                default:
                    _itemUIItems.Sort((x, y) => x.Amount.CompareTo(y.Amount));
                    break;
            }

            UpdateInventorySortOrder();
        }

        private void UpdateCurrencySortOrder() {
            for (int i = 0; i < _currencyUIItems.Count; i++) {
                _currencyUIItems[i].transform.SetSiblingIndex(i);
            }
        }

        private void UpdateInventorySortOrder() {
            for (int i = 0; i < _itemUIItems.Count; i++) {
                PlaceItemInInventorySlot(_itemUIItems[i], _inventorySlots[i]);
                //_itemUIItems[i].transform.SetParent(_inventorySlots[i].transform);
                //_itemUIItems[i].transform.localPosition = Vector3.zero;
            }
        }

        private void PlaceItemInInventorySlot(ItemUIItem item, InventorySlot inventorySlot) {
            inventorySlot.SetItem(item);
            item.SetInventorySlot(inventorySlot);

            // Make sure our item matches our inventory slot sizes, this is dynamic.
            RectTransform slotRectTransform = inventorySlot.GetComponent<RectTransform>();
            item.transform.SetParent(inventorySlot.transform);
            item.GetComponent<RectTransform>().sizeDelta = slotRectTransform.sizeDelta;
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            item.EnableItemButton();
        }

        private void CancelSelectedItem(CallbackContext ctx) {
            if (InventoryOpen && _selectedItem != null) {
                MoveItemToSlot(_selectedItem, _selectedItemInventorySlot);
            }
        }

        private bool InventoryFull() {
            //RPS.Debug.Log($"Inventory State: {_itemUIItems.Count} of {_maxInventorySlots} slots occupied.", RPS.Debug.Style.Success);
            return _itemUIItems.Count >= _maxInventorySlots;
        }

        private void GiveStartingResources() {
            AddCurrency(CurrencyHandler.Instance.GalacticCredits, TransactionType.ADD, 100000, true);
            AddCurrency(CurrencyHandler.Instance.BitShards, TransactionType.ADD, 256, true);
            AddCurrency(CurrencyHandler.Instance.Quantumite, TransactionType.ADD, 2, true);
        }

        private void SendToActivityFeed(ItemSO item, int amount, TransactionType transaction) {
            string addedremoved = transaction == TransactionType.ADD ? "<color=green>Added</color>" : "<color=red>Removed</color>";
            string tofrom = transaction == TransactionType.ADD ? "to" : "from";

            ActivityFeedService.Instance.AddNewEntry(
                item.ItemIcon,
                item.GetRarityPrimaryColor(),
                $"{addedremoved} {amount} <color=#{ColorUtility.ToHtmlStringRGB(item.GetRarityPrimaryColor())}>{item.ItemID}</color> {tofrom} inventory."
            );
        }

        private void SendToActivityFeed(CurrencySO currency, int amount, TransactionType transaction) {
            string addedremoved = transaction == TransactionType.ADD ? "<color=green>Added</color>" : "<color=red>Removed</color>";
            string tofrom = transaction == TransactionType.ADD ? "to" : "from";

            ActivityFeedService.Instance.AddNewEntry(
                currency.CurrencyIcon,
                currency.CurrencyColor,
                $"{addedremoved} {amount} <color=#{ColorUtility.ToHtmlStringRGB(currency.CurrencyColor)}>{currency.ItemID}</color> {tofrom} inventory."
            );
        }
        #endregion

        #region Public Methods
        public void AddCurrency(CurrencySO currency, TransactionType transaction, int amount, bool silent = false) {
            // If we don't already have this type of item stored...
            if (!_currencies.ContainsKey(currency)) {
                // Create it...
                CurrencyUIItem newCurrencyItem = Instantiate(CurrencyUIItemPrefab, CurrencyPanelRoot).GetComponent<CurrencyUIItem>();
                // Set it's initial Data...
                newCurrencyItem.SetData(currency, 0);
                // Add to the list of existing UI currency items...
                _currencyUIItems.Add(newCurrencyItem);
                // Add to the private currency dictionary.
                _currencies.Add(currency, 0);
            }

            // Perform transaction
            if (transaction == TransactionType.ADD) {
                _currencies[currency] += amount;
            } else {
                // If the amount would be less than 0 and we can't go into debt, ignore
                if (_currencies[currency] - amount < 0 && !currency.CanBeInDebt) {
                    return;
                }

                // Otherwise, subtract this transaction
                _currencies[currency] -= amount;

            }

            UpdateCurrencyUI(currency);
            if (!silent)
                SendToActivityFeed(currency, amount, transaction);
        }

        public void AddItem(ItemSO item, TransactionType transaction, int amount, bool silent = false) {

            if (transaction == TransactionType.ADD) {

                // If we don't already have this type of item stored, add it to the first empty slot we find.
                if (!_items.ContainsKey(item) && !InventoryFull()) {
                    // Create new ItemUI prefab.
                    ItemUIItem newItem = CreateItemStack(item, 0);

                    // Add to the private item dictionary.
                    _items.Add(item, 0);

                    AddStackToFirstEmptySlot(newItem);
                }

                // Checking for max stacks
                bool stackFound = false;
                for (int i = 0; i < _itemUIItems.Count; i++) {
                    if (_itemUIItems[i].Item == item) {
                        // If we find a full stack of this item, go to next
                        if (_itemUIItems[i].Amount >= item.MaxStack) {
                            continue;
                        } else { // Otherwise check if it has room for new amount
                                 // Add entire amount
                            _itemUIItems[i].UpdateValue(_itemUIItems[i].Amount + amount, false);
                            stackFound = true;
                            // Check if we are in overflow
                            if (_itemUIItems[i].Amount > item.MaxStack) {
                                int overflowAmount = _itemUIItems[i].Amount - item.MaxStack;
                                // Make sure original stack shows max amount
                                _itemUIItems[i].UpdateValue(item.MaxStack, false);
                                // Make new stack
                                ItemUIItem newItem = CreateItemStack(item, overflowAmount);
                                AddStackToFirstEmptySlot(newItem);
                                stackFound = true;
                                break;
                            }
                        }
                    }
                }

                if (!stackFound && !InventoryFull()) {
                    ItemUIItem newItem = CreateItemStack(item, amount);
                    AddStackToFirstEmptySlot(newItem);
                }
            }

            if (transaction == TransactionType.SUBTRACT) {

                if (!CanAfford(item, amount)) {
                    RPSLib.Debug.Log($"Tried to remove {amount} {item.ItemID}, but not enough stored. Doing nothing.", RPSLib.Debug.Style.Warning);
                    return;
                }

                for (int i = 0; i < _itemUIItems.Count; i++) {
                    if (_itemUIItems[i].Item == item) {
                        _itemUIItems[i].UpdateValue(_itemUIItems[i].Amount - amount, true);
                        if (_itemUIItems[i].Amount <= 0) {
                            _itemUIItems.RemoveAt(i);
                            //RemoveItem(_itemUIItems[i]);
                        }
                    }
                }
            }

            // Perform transaction - data only
            if (transaction == TransactionType.ADD) {
                _items[item] += amount;
            } else {
                _items[item] -= amount;
            }

            // Remove from the list - data only
            if (_items[item] <= 0) {
                _items.Remove(item);
            }

            if (!silent)
                SendToActivityFeed(item, amount, transaction);
        }

        public void SelectItem(ItemUIItem selectedItem) {
            _selectedItem = selectedItem;
            _selectedItem.DisableItemButton();

            //_selectedItemInventorySlot = selectedItem.transform.parent.GetComponent<InventorySlot>();
            // Get the inventory slot this item was in and empty it...
            _selectedItemInventorySlot = _selectedItem.GetInventorySlot();
            _selectedItemInventorySlot.SetItem(null);

            // Set the new parent of the item to the inventory canvas...
            // ... this let's us see the item and drag it around the canvas
            _selectedItem.GetRectTransform().SetParent(inventoryCanvas.transform);
        }

        public bool HasItemSelected(out ItemUIItem selectedItem) {
            if (_selectedItem != null) {
                selectedItem = _selectedItem;
                return true;
            } else {
                selectedItem = null;
                return false;
            }
        }

        public bool HasItemSelected() {
            return _selectedItem != null;
        }

        public void MoveItemToSlot(ItemUIItem item, InventorySlot slot) {
            PlaceItemInInventorySlot(item, slot);
            _selectedItem = null;
            _selectedItemInventorySlot = null;
        }

        public bool CanAfford(Currency targetCurrency, int amount) {
            foreach (var currency in _currencies) {
                if (currency.Key.Currency == targetCurrency) {
                    return currency.Value >= amount;
                }
            }

            return false;
        }

        public bool CanAfford(ItemSO targetItem, int amount) {
            foreach (var item in _items) {
                if (item.Key == targetItem) {
                    return item.Value >= amount;
                }
            }

            return false;
        }
        #endregion

        #region Button Handlers
        private void OpenInventory() {
            inventoryCanvas.enabled = true;
            itemScrollRectCanvas.enabled = true;
        }

        public void CloseInventory() {
            if (_selectedItem) {
                MoveItemToSlot(_selectedItem, _selectedItemInventorySlot);
            }

            inventoryCanvas.enabled = false;
            itemScrollRectCanvas.enabled = false;
        }

        public void SortByRarity() {
            SortInventoryUIItems(ResourceSortingType.RARITY);
        }
        #endregion

        #region SAVING/LOADING
        //private void SetSaveableData() {
        //    //Data.HAPPINESS = storedResources[Resources.Type.HAPPINESS];
        //    //Data.VILLAGERS = storedResources[Resources.Type.VILLAGER];
        //    //Data.FOOD = storedResources[Resources.Type.FOOD];
        //    //Data.WHEAT = storedResources[Resources.Type.WHEAT];
        //    //Data.CORN = storedResources[Resources.Type.CORN];
        //    //Data.FISH = storedResources[Resources.Type.FISH];
        //    //Data.STRAWBERRIES = storedResources[Resources.Type.STRAWBERRY];
        //    //Data.WOOD = storedResources[Resources.Type.WOOD];
        //    //Data.STONE = storedResources[Resources.Type.STONE];
        //    //Data.METAL = storedResources[Resources.Type.METAL];
        //    //Data.CRYSTALS = storedResources[Resources.Type.CRYSTAL];
        //    //Data.COIN = storedResources[Resources.Type.COIN];
        //    //Data.ANIMALS = storedResources[Resources.Type.ANIMALS];
        //    //Data.CHICKEN = storedResources[Resources.Type.CHICKEN];
        //    //Data.SHEEP = storedResources[Resources.Type.SHEEP];
        //    //Data.COW = storedResources[Resources.Type.COW];
        //    //Data.EGG = storedResources[Resources.Type.EGG];
        //    //GameSaveData.Instance.Resources = Data;
        //}

        //public void LoadSavedData(ResourceData Data) {
        //    //storedResources[Resources.Type.HAPPINESS] = Data.HAPPINESS;
        //    //storedResources[Resources.Type.VILLAGER] = Data.VILLAGERS;
        //    //storedResources[Resources.Type.FOOD] = Data.FOOD;
        //    //storedResources[Resources.Type.WHEAT] = Data.WHEAT;
        //    //storedResources[Resources.Type.CORN] = Data.CORN;
        //    //storedResources[Resources.Type.FISH] = Data.FISH;
        //    //storedResources[Resources.Type.STRAWBERRY] = Data.STRAWBERRIES;
        //    //storedResources[Resources.Type.WOOD] = Data.WOOD;
        //    //storedResources[Resources.Type.STONE] = Data.STONE;
        //    //storedResources[Resources.Type.METAL] = Data.METAL;
        //    //storedResources[Resources.Type.CRYSTAL] = Data.CRYSTALS;
        //    //storedResources[Resources.Type.COIN] = Data.COIN;
        //    ////storedResources[Resources.Type.ANIMALS] = Data.ANIMALS;
        //    ////storedResources[Resources.Type.CHICKEN] = Data.CHICKEN;
        //    //storedResources[Resources.Type.SHEEP] = Data.SHEEP;
        //    //storedResources[Resources.Type.COW] = Data.COW;
        //    //storedResources[Resources.Type.EGG] = Data.EGG;

        //    //UpdateUI();
        //}
        #endregion

        #region TESTS
        private void TEST_GiveRandomItems(int numberOfItems, int amountPerItem) {
            for (int i = 0; i < numberOfItems; i++) {
                AddItem(ItemHandler.Instance.GetRandomItemSO(), TransactionType.ADD, amountPerItem);
            }
        }

        private void TEST_GiveSpecificItem(string id, int amountPerItem) {
            AddItem(ItemHandler.Instance.GetItemSOByID(id), TransactionType.ADD, amountPerItem);
        }

        private void TEST_RemoveFirstStack() {
            AddCurrency(CurrencyHandler.Instance.GalacticCredits, TransactionType.SUBTRACT, 6666);
            AddItem(ItemHandler.Instance.GetRandomItemSO(), TransactionType.SUBTRACT, 100);
        }
        #endregion

    }

}