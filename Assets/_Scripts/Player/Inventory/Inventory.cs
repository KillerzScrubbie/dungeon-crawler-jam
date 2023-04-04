using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<int, ObjItems> itemList;
    private List<ObjPotions> potionList;

    public event Action OnInventoryUpdated;

    private readonly int maxItemSlots = 4;
    private readonly int maxPotionSlots = 2;

    public Inventory()
    {
        itemList = new Dictionary<int, ObjItems>(maxItemSlots);
        potionList = new List<ObjPotions>();
    }

    public void AddItem(ObjItems item, int slot = -1)
    {
        int itemSlot = slot >= 0 ? slot : itemList.Count;

        if (itemSlot > maxItemSlots - 1)
        {
            Debug.Log("Full");
            return;
        }

        if (itemList.ContainsKey(itemSlot)) 
        {
            itemList[itemSlot] = item;
        }
        else
        {
            itemList.Add(itemSlot, item);
        }
        
        Debug.Log($"Added {item.Name} to slot {itemSlot}");
        OnInventoryUpdated?.Invoke();
    }

    public void AddPotion(ObjPotions potion)
    {
        potionList.Add(potion);
    }

    public Dictionary<int, ObjItems> GetItemList()
    {
        return itemList;
    }

    public List<ObjPotions> GetPotionList()
    {
        return potionList;
    }
}
