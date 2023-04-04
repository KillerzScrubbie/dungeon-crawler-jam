using System.Collections;
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
        // inventory.OnInventoryUpdated += SetInventory;
        PlayerInputController.OnInventoryOpened += HandleInventoryPopup;
    }

    private void HandleInventoryPopup()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        Dictionary<int, ObjItems> inventoryItemList = inventory.GetItemList();
        int inventoryCount = inventoryItemList.Count;

        for (int i = 0; i < inventoryCount; i++)
        {
            ObjItems item = inventoryItemList[i];
            if (item == null) continue;

            itemSlots[i].sprite = item.Icon;
        }

        /*for (int i = itemSlots.Count - inventoryCount - 1; i >= 0; i--)
        {
            itemSlots[i].sprite = inventoryItemList[i].Icon;
        }*/
    }

    private void OnDestroy()
    {
        // inventory.OnInventoryUpdated -= SetInventory;
        PlayerInputController.OnInventoryOpened -= HandleInventoryPopup;
    }
}
