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
        ItemData.OnItemRemoved += HandleItemRemoved;
    }

    public void OnClicked()
    {
        HandleInventoryPopup();
    }

    private void HandleInventoryPopup()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    private void HandleChestOpened(ObjItems item, ObjPotions potion)
    {
        if (item != null) { inventory.AddItem(item); }
        if (potion != null) { inventory.AddPotion(potion); }
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
        }
        
    }

    private void OnDestroy()
    {
        inventory.OnInventoryUpdated -= RefreshInventoryItems;
        inventory.OnPotionUpdated -= RefreshPotionItems;
        PlayerInputController.OnInventoryOpened -= HandleInventoryPopup;
        MouseClickDetector.OnChestClicked -= HandleChestOpened;
        ItemData.OnItemRemoved -= HandleItemRemoved;
    }
}
