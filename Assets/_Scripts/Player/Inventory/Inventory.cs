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

    public Inventory()
    {
        itemList = new Dictionary<int, ObjItems>();
        potionList = new Dictionary<int, ObjPotions>();
    }

    public bool AddItem(ObjItems item, int slot = -1)
    {
        int itemSlot = slot >= 0 ? slot : maxItemSlots;

        for (int i = 0; i < maxItemSlots; i++)
        {
            if (itemList.ContainsKey(i)) continue;

            itemSlot = i;
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

    public void SwapItemPosition(EInventorySlot slotType1, int slot1, EInventorySlot slotType2, int slot2)
    {

    }

    public bool AddPotion(ObjPotions potion, int slot = -1)
    {
        int potionSlot = slot >= 0 ? slot : maxPotionSlots;

        for (int i = 0; i < maxPotionSlots; i++)
        {
            if (potionList.ContainsKey(i)) continue;

            potionSlot = i;
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
}
