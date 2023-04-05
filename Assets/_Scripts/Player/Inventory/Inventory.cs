using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<int, ObjItems> itemList;
    private Dictionary<int, ObjPotions> potionList;
    private Dictionary<int, ObjItems> equippedList;

    public event Action OnInventoryUpdated;
    public event Action OnPotionUpdated;
    public event Action OnEquippedUpdated;

    private readonly int maxItemSlots = 4;
    private readonly int maxPotionSlots = 2;
    private readonly int maxEquippedSlots = 4;

    public Inventory()
    {
        itemList = new Dictionary<int, ObjItems>();
        potionList = new Dictionary<int, ObjPotions>();
        equippedList = new Dictionary<int, ObjItems>();
    }

    public bool AddItem(ObjItems item, int slot = -1)
    {
        int itemSlot = slot >= 0 ? slot : maxItemSlots;

        for (int i = 0; i < maxItemSlots; i++)
        {
            if (itemList.ContainsKey(i)) continue;

            itemSlot = itemSlot < maxItemSlots ? itemSlot : i;
            break;
        }

        if (itemSlot > maxItemSlots - 1)
        {
            Debug.Log("Inventory Full");
            return false;
        }

        if (itemList.ContainsKey(itemSlot)) 
        {
            itemList[itemSlot] = item;
        }
        else
        {
            itemList.Add(itemSlot, item);
        }
        
        OnInventoryUpdated?.Invoke();
        return true;
    }

    public void RemoveItem(int slot) 
    {
        itemList.Remove(slot);

        OnInventoryUpdated?.Invoke();
    }

    public bool AddEquipped(ObjItems item, int slot = -1)
    {
        int equippedSlot = slot >= 0 ? slot : maxEquippedSlots;

        for (int i = 0; i < maxEquippedSlots; i++)
        {
            if (equippedList.ContainsKey(i)) continue;

            equippedSlot = i < maxEquippedSlots ? equippedSlot : i;
            break;
        }

        if (equippedSlot > maxEquippedSlots - 1)
        {
            Debug.Log("Equipped Full");
            return false;
        }

        if (equippedList.ContainsKey(equippedSlot))
        {
            equippedList[equippedSlot] = item;
        }
        else
        {
            equippedList.Add(equippedSlot, item);
        }

        OnEquippedUpdated?.Invoke();
        return true;
    }

    public void RemoveEquipped(int slot)
    {
        equippedList.Remove(slot);

        OnEquippedUpdated?.Invoke();
    }

    public void SwapItemPosition(EInventorySlot slotType1, int slot1, EInventorySlot slotType2, int slot2)
    {
        EInventorySlot startSlotType = slotType1;
        EInventorySlot endSlotType = slotType2;
        int startSlotId = slot1;
        int endSlotId = slot2;

        if (slotType1 == slotType2 && slot1 == slot2) { return; }

        ObjItems item1 = null;
        ObjItems item2 = null;

        switch (startSlotType)
        {
            case EInventorySlot.Equipped:
                if (!equippedList.ContainsKey(startSlotId)) { break; }

                item1 = equippedList[startSlotId];
                RemoveEquipped(startSlotId);
                break;

            case EInventorySlot.Inventory:
                if (!itemList.ContainsKey(startSlotId)) { break; }

                item1 = itemList[startSlotId];
                RemoveItem(startSlotId);
                break;

            default:
                break;
        }

        switch (endSlotType)
        {
            case EInventorySlot.Equipped:
                if (equippedList.ContainsKey(endSlotId))
                {
                    item2 = equippedList[endSlotId];
                    RemoveEquipped(endSlotId);
                } 

                if (item1 == null) { break; }
                AddEquipped(item1, endSlotId);

                break;

            case EInventorySlot.Inventory:
                if (itemList.ContainsKey(endSlotId))
                {
                    item2 = itemList[endSlotId];
                    RemoveItem(endSlotId);
                }   

                if (item1 == null) { break; }
                AddItem(item1, endSlotId);
                Debug.Log($"Added {item1.Name} to slot {endSlotId}");
                break;
            default:
                break;
        }

        switch (startSlotType)
        {
            case EInventorySlot.Equipped:
                if (item2 == null) { break; }
                AddEquipped(item2, startSlotId);

                break;

            case EInventorySlot.Inventory:
                if (item2 == null) { break; }
                AddItem(item2, startSlotId);

                break;

            default:
                break;
        }
    }

    public bool AddPotion(ObjPotions potion, int slot = -1)
    {
        int potionSlot = slot >= 0 ? slot : maxPotionSlots;

        for (int i = 0; i < maxPotionSlots; i++)
        {
            if (potionList.ContainsKey(i)) continue;

            potionSlot = potionSlot < maxPotionSlots ? potionSlot : i;
            break;
        }

        if (potionSlot > maxPotionSlots - 1)
        {
            Debug.Log("Potions Full");
            return false;
        }

        if (potionList.ContainsKey(potionSlot))
        {
            potionList[potionSlot] = potion;
        }
        else
        {
            potionList.Add(potionSlot, potion);
        }

        OnPotionUpdated?.Invoke();
        return true;
    }

    public void RemovePotion(int slot)
    {
        potionList.Remove(slot);

        OnPotionUpdated?.Invoke();
    }

    public Dictionary<int, ObjItems> GetItemList()
    {
        return itemList;
    }

    public Dictionary<int, ObjPotions> GetPotionList()
    {
        return potionList;
    }

    public Dictionary<int, ObjItems> GetEquippedList()
    {
        return equippedList;
    }
}
