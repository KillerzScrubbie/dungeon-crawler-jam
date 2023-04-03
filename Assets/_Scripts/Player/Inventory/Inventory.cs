using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ObjItems> itemList;
    private List<ObjPotions> potionList;

    public Inventory()
    {
        itemList = new List<ObjItems>();
        potionList = new List<ObjPotions>();
    }

    public void AddItem(ObjItems item)
    {
        itemList.Add(item);
        Debug.Log($"Added {item.Name}");
    }

    public void AddPotion(ObjPotions potion)
    {
        potionList.Add(potion);
    }
}
