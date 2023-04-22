using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //[SerializeField] private ObjItems[] itemsToAdd;
    [SerializeField] private InventoryUI inventoryUI;

    private Inventory inventory;
    public Inventory GetInventory() => inventory;
    public InventoryUI GetUI() => inventoryUI;

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
