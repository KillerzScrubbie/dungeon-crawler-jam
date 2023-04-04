using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class InventoryUI : SerializedMonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;

    [Space]
    [SerializeField] private List<Image> itemSlots;
    [SerializeField] private List<Image> potionSlots;
    [SerializeField] private List<Image> equippedSlots;

    private Inventory inventory;

    private void Start()
    {
        PlayerInputController.OnInventoryOpened += HandleInventoryPopup;
        MouseClickDetector.OnChestClicked += HandleChestOpened;
    }

    public void OnClicked()
    {
        HandleInventoryPopup();
    }

    private void HandleInventoryPopup()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    private void HandleChestOpened(ObjItems item)
    {
        inventory.AddItem(item);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnInventoryUpdated += RefreshInventoryItems;

        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        Dictionary<int, ObjItems> inventoryItemList = inventory.GetItemList();
        // int inventoryCount = inventoryItemList.Count;

        for (int i = 0; i < itemSlots.Count; i++)
        {
            Image currentSlot = itemSlots[i];

            if (!inventoryItemList.ContainsKey(i))
            {
                currentSlot.enabled = false;
                continue;
            }

            ObjItems item = inventoryItemList[i];
            ItemData itemData = currentSlot.GetComponent<ItemData>();

            if (item == null)
            {
                currentSlot.enabled = false;
                continue;
            }

            if (itemData.Item == item)
            {
                continue;
            }

            currentSlot.enabled = true;
            itemData.Item = item;
            currentSlot.sprite = item.Icon;
        }
    }

    private void OnDestroy()
    {
        inventory.OnInventoryUpdated -= RefreshInventoryItems;
        PlayerInputController.OnInventoryOpened -= HandleInventoryPopup;
        MouseClickDetector.OnChestClicked -= HandleChestOpened;
    }
}
