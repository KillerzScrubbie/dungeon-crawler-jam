using UnityEngine;
using Sirenix.OdinInspector;

public class AddDebugWeapon : MonoBehaviour
{
    [SerializeField] ObjItems godObj;
    [SerializeField] PlayerInventory playerInventory;
    Inventory _inventoryScpt;
    InventoryUI _inventoryUI;

    void Start()
    {
        _inventoryScpt = playerInventory.GetInventory();
        _inventoryUI = playerInventory.GetUI();
        GetGodWeapon();
    }

    [Button]
    void GetGodWeapon()
    {
        _inventoryScpt.AddEquipped(godObj, 0);
        _inventoryUI.SetInventory(_inventoryScpt);
    }
}

