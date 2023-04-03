using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private ObjItems itemToAdd;

    private Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory();
        inventory.AddItem(itemToAdd);
    }
}
