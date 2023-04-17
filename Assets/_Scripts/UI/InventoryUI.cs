using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class InventoryUI : SerializedMonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject chestPanel;
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI usageText;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private Button useButton;
    [SerializeField] private Button discardButton;

    [Space]
    [SerializeField] private List<Image> itemSlots;
    [SerializeField] private List<Image> potionSlots;
    [SerializeField] private List<Image> equippedSlots;
    [SerializeField] private List<Image> chestSlots;

    private Inventory inventory;
    private ChestLoot currentChest;

    public static event Action<ItemData, bool> OnSwapped;

    private bool isSwapping = false;

    public Inventory Inventory { get { return inventory; } }

    private void Start()
    {
        PlayerInputController.OnInventoryOpened += HandleInventoryPopup;
        PlayerInputController.OnQueue += HandleMovement;
        MouseClickDetector.OnChestClicked += HandleChestOpened;
        ItemData.OnItemRemoved += HandleItemRemoved;
        ItemData.OnItemLooted += HandleItemLooted;
        ItemData.OnPotionLooted += HandlePotionLooted;
        ItemData.OnPromptClicked += HandlePromptClicked;
        ItemData.OnItemSuccessSwapped += HandleItemSwapped;

        CombatManager.OnCombatStateChanged += HandleCombatUI;
    }

    public void OnClicked()
    {
        HandleInventoryPopup();
    }

    private void HandlePromptClicked(ItemData itemData, EInventorySlot slot, RectTransform promptAnchorTransform)
    {
        if (isSwapping) { return; }

        string useButtonText = "Use";

        promptPanel.transform.position = promptAnchorTransform.position;

        useButton.onClick.RemoveAllListeners();
        discardButton.onClick.RemoveAllListeners();
        useButton.interactable = true;

        switch (slot)
        {
            case EInventorySlot.Inventory:
                useButtonText = "Equip";
                useButton.onClick.AddListener(() => EquipItem(itemData));
                break;
            case EInventorySlot.Equipped:
                useButtonText = "Swap";
                useButton.onClick.AddListener(() => EquipItem(itemData));
                break;
            case EInventorySlot.Potions:
                useButton.interactable = !itemData.Potion.CombatOnly;
                useButtonText = !itemData.Potion.CombatOnly ? "Drink" : "Combat";

                useButton.onClick.AddListener(() => UsePotion(itemData));
                break;
            default:
                break;
        }

        discardButton.onClick.AddListener(() => RemoveItem(itemData));
        usageText.text = useButtonText;
        promptPanel.SetActive(true);
    }

    private void EquipItem(ItemData itemData)
    {
        isSwapping = true;
        OnSwapped?.Invoke(itemData, isSwapping);

        promptPanel.SetActive(false);
    }

    private void HandleItemSwapped(EInventorySlot slot1, int id1, EInventorySlot slot2, int id2)
    {
        inventory.SwapItemPosition(slot1, id1, slot2, id2);
        AudioManager.instance?.PlayRandomPitch("playerEquip", 0.75f, 1.5f);
        isSwapping = false;
        OnSwapped?.Invoke(null, isSwapping);
    }

    private void UsePotion(ItemData itemData)
    {
        itemData.UsePotion();
        promptPanel.SetActive(false);
    }

    private void RemoveItem(ItemData itemData)
    {
        itemData.RemoveItem();
        promptPanel.SetActive(false);
    }

    private void HandleCombatUI(CombatState state)
    {
        switch (state)
        {
            case CombatState.NotInCombat:
                inventoryButton.SetActive(true);
                break;

            case CombatState.StartCombat:
                CancelSwapOperation();
                inventoryPanel.SetActive(false);
                inventoryButton.SetActive(false);
                promptPanel.SetActive(false);
                CloseChest();
                chestPanel.SetActive(false);
                break;

            default:  
                break;
        }
    }

    public void CloseChest(bool sameChest = false)
    {
        if (currentChest == null) { return; }

        if (sameChest) { return; }

        currentChest.CloseChest();
        currentChest = null;
    }

    private void HandleInventoryPopup()
    {
        bool inventoryStatus = inventoryPanel.activeSelf;
        CancelSwapOperation();
        inventoryPanel.SetActive(!inventoryStatus);

        if (!inventoryStatus)
        {
            AudioManager.instance?.PlayRandomPitch("invenOpen", .7f, 1.5f);
        }
        else
        {
            AudioManager.instance?.PlayRandomPitch("invenClose", .7f, 1.5f);
            chestPanel.SetActive(false);
            CloseChest(false);
            HideToolTip();
        }     
    }

    private void HideToolTip()
    {
        if (ToolTipSystem.current != null)
        {
            ToolTipSystem.Hide();
        }
    }

    private void CancelSwapOperation()
    {
        isSwapping = false;
        OnSwapped?.Invoke(null, isSwapping);
    }

    private void HandleChestOpened(ChestLoot chest, List<ObjItems> items, List<ObjPotions> potions)
    {
        inventoryPanel.SetActive(true);
        chestPanel.SetActive(true);

        CloseChest(currentChest == chest);
        currentChest = chest;

        ProcessChestLootUI(items, potions);
    }

    private void HandleItemLooted(ItemData itemData, ObjItems item, int slot)
    {
        if (!inventory.AddItem(item)) { return; }

        AudioManager.instance?.PlayRandomPitch("itemLoot", 0.75f, 1.5f);

        itemData.Loot();
        currentChest.RemoveItem(item);
        chestSlots[slot].enabled = false;
    }

    private void HandlePotionLooted(ItemData itemData, ObjPotions potion, int slot)
    {
        if (!inventory.AddPotion(potion)) { return; }
        AudioManager.instance?.PlayRandomPitch("potionLoot", 0.75f, 1.5f);

        itemData.Loot();
        currentChest.RemovePotion(potion);
        chestSlots[slot].enabled = false;
    }

    private void ClearChestLootUI()
    {
        for (int i = 0; i < chestSlots.Count; i++)
        {
            Image currentSlot = chestSlots[i];

            currentSlot.enabled = false;
            currentSlot.GetComponent<ItemData>().Item = null;
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
        inventory.OnEquippedUpdated += RefreshEquippedItems;

        RefreshInventoryItems();
        RefreshPotionItems();
        RefreshEquippedItems();
    }

    private void RefreshInventoryItems()
    {
        Dictionary<int, ObjItems> inventoryItemList = inventory.GetItemList();

        for (int i = 0; i < itemSlots.Count; i++)
        {
            Image currentSlot = itemSlots[i];
            ItemData itemData = currentSlot.GetComponent<ItemData>();

            if (!inventoryItemList.ContainsKey(i))
            {
                currentSlot.enabled = false;
                itemData.Item = null;
                continue;
            }

            ObjItems item = inventoryItemList[i];

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
            ItemData potionData = currentSlot.GetComponent<ItemData>();

            if (!potionItemList.ContainsKey(i))
            {
                currentSlot.enabled = false;
                potionData.Potion = null;
                continue;
            }

            ObjPotions potion = potionItemList[i];

            if (potionData.Potion == potion)
            {
                continue;
            }

            currentSlot.enabled = true;
            potionData.Potion = potion;
            currentSlot.sprite = potion.Icon;
        }
    }

    private void RefreshEquippedItems()
    {
        Dictionary<int, ObjItems> equippedItemList = inventory.GetEquippedList();

        for (int i = 0; i < equippedSlots.Count; i++)
        {
            Image currentSlot = equippedSlots[i];
            ItemData itemData = currentSlot.GetComponent<ItemData>();

            if (!equippedItemList.ContainsKey(i))
            {
                currentSlot.enabled = false;
                itemData.Item = null;
                continue;
            }

            ObjItems item = equippedItemList[i];

            if (itemData.Item == item)
            {
                continue;
            }

            currentSlot.enabled = true;
            itemData.Item = item;
            currentSlot.sprite = item.Icon;
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
                inventory.RemoveEquipped(slot);
                break;
            case EInventorySlot.Potions:
                inventory.RemovePotion(slot);
                break;
            case EInventorySlot.Chest:
                break;
        }

    }

    private void HandleMovement(EMovementTypes movementType)
    {
        CloseChest();
        chestPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        inventory.OnInventoryUpdated -= RefreshInventoryItems;
        inventory.OnPotionUpdated -= RefreshPotionItems;
        inventory.OnEquippedUpdated -= RefreshEquippedItems;
        PlayerInputController.OnInventoryOpened -= HandleInventoryPopup;
        PlayerInputController.OnQueue -= HandleMovement;
        MouseClickDetector.OnChestClicked -= HandleChestOpened;
        ItemData.OnItemRemoved -= HandleItemRemoved;
        ItemData.OnItemLooted -= HandleItemLooted;
        ItemData.OnPotionLooted -= HandlePotionLooted;
        ItemData.OnPromptClicked -= HandlePromptClicked;
        ItemData.OnItemSuccessSwapped -= HandleItemSwapped;

        CombatManager.OnCombatStateChanged -= HandleCombatUI;
    }
}
