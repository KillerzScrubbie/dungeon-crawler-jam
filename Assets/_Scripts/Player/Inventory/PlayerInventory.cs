using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //[SerializeField] private ObjItems[] itemsToAdd;
    [SerializeField] private InventoryUI inventoryUI;

    private Inventory inventory;

    private void Awake()
    {
        inventory = new Inventory();

        /*foreach (var item in itemsToAdd)
        {
            inventory.AddItem(item);
        }*/
        
        inventoryUI.SetInventory(inventory);
    }
}
