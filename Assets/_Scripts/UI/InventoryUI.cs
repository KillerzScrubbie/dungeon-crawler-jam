using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using static UnityEditor.Progress;
using System;

public class InventoryUI : SerializedMonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject chestPanel;

    [Space]
    [SerializeField] private List<Image> itemSlots;
    [SerializeField] private List<Image> potionSlots;
    [SerializeField] private List<Image> equippedSlots;
    [SerializeField] private List<Image> chestSlots;

    private Inventory inventory;
    private ChestLoot currentChest;

    private void Start()
    {
        PlayerInputController.OnInventoryOpened += HandleInventoryPopup;
        MouseClickDetector.OnChestClicked += HandleChestOpened;
        ItemData.OnItemRemoved += HandleItemRemoved;
        ItemData.OnItemLooted += HandleItemLooted;
        ItemData.OnPotionLooted += HandlePotionLooted;
    }

    public void OnClicked()
    {
        HandleInventoryPopup();
    }

    public void CloseChest()
    {
        if (currentChest == null) { return; }

        currentChest.CloseChest();
    }

    private void HandleInventoryPopup()
    {
        bool inventoryStatus = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!inventoryStatus);

        if (!inventoryStatus) { return; }

        chestPanel.SetActive(false);

        CloseChest();
    }

    private void HandleChestOpened(ChestLoot chest, List<ObjItems> items, List<ObjPotions> potions)
    {
        inventoryPanel.SetActive(true);
        chestPanel.SetActive(true);

        currentChest = chest;

        ProcessChestLootUI(items, potions);
        /*if (item != null) { inventory.AddItem(item); }
        if (potion != null) { inventory.AddPotion(potion); }*/
    }

    private void HandleItemLooted(ItemData itemData, ObjItems item, int slot)
    {
        if (!inventory.AddItem(item)) { return; }

        itemData.Loot();
        currentChest.RemoveItem(item);
        chestSlots[slot].enabled = false;
    }

    private void HandlePotionLooted(ItemData itemData, ObjPotions potion, int slot)
    {
        if (!inventory.AddPotion(potion)) { return; }

        itemData.Loot();
        currentChest.RemovePotion(potion);
        chestSlots[slot].enabled = false;
    }

    private void ClearChestLootUI()
    {
        for (int i = 0; i < chestSlots.Count; i++)
        {
            chestSlots[i].enabled = false;
        }
    }

    private void ProcessChestLootUI(List<ObjItems> items, List<ObjPotions> potions)
    {
        ClearChestLootUI();

        int itemCount = items.Count;

        for (int itemSlot = 0; itemSlot < itemCount; itemSlot++)
        {
            Image currentSlot = chestSlots[itemSlot];

            ObjItems item = items[itemSlot];
            ItemData itemData = currentSlot.GetComponent<ItemData>();

            currentSlot.enabled = true;
            itemData.Item = item;
            currentSlot.sprite = item.Icon;
        }

        for (int potionSlot = 0; potionSlot < potions.Count; potionSlot++)
        {
            Image currentSlot = chestSlots[potionSlot + itemCount];

            ObjPotions potion = potions[potionSlot];
            ItemData itemData = currentSlot.GetComponent<ItemData>();

            currentSlot.enabled = true;
            itemData.Potion = potion;
            currentSlot.sprite = potion.Icon;
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnInventoryUpdated += RefreshInventoryItems;
        inventory.OnPotionUpdated += RefreshPotionItems;

        RefreshInventoryItems();
        RefreshPotionItems();
    }

    private void RefreshInventoryItems()
    {
        Dictionary<int, ObjItems> inventoryItemList = inventory.GetItemList();

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

            if (itemData.Item == item)
            {
                continue;
            }

            currentSlot.enabled = true;
            itemData.Item = item;
            currentSlot.sprite = item.Icon;
        }
    }

    private void RefreshPotionItems()
    {
        Dictionary<int, ObjPotions> potionItemList = inventory.GetPotionList();

        for (int i = 0; i < potionSlots.Count; i++)
        {
            Image currentSlot = potionSlots[i];

            if (!potionItemList.ContainsKey(i))
            {
                currentSlot.enabled = false;
                continue;
            }

            ObjPotions potion = potionItemList[i];
            ItemData potionData = currentSlot.GetComponent<ItemData>();

            if (potionData.Potion == potion)
            {
                continue;
            }

            currentSlot.enabled = true;
            potionData.Potion = potion;
            currentSlot.sprite = potion.Icon;
        }
    }

    private void HandleItemRemoved(EInventorySlot slotType, int slot)
    {
        switch (slotType)
        {
            case EInventorySlot.Inventory:
                inventory.RemoveItem(slot);
                break;
            case EInventorySlot.Equipped:
                // inventory.RemoveEquipped(slot);
                break;
            case EInventorySlot.Potions:
                inventory.RemovePotion(slot);
                break;
            case EInventorySlot.Chest:
                break;
        }
        
    }

    private void OnDestroy()
    {
        inventory.OnInventoryUpdated -= RefreshInventoryItems;
        inventory.OnPotionUpdated -= RefreshPotionItems;
        PlayerInputController.OnInventoryOpened -= HandleInventoryPopup;
        MouseClickDetector.OnChestClicked -= HandleChestOpened;
        ItemData.OnItemRemoved -= HandleItemRemoved;
        ItemData.OnItemLooted -= HandleItemLooted;
        ItemData.OnPotionLooted -= HandlePotionLooted;
    }
}
