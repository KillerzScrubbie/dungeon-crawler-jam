using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler 
{
    [SerializeField] private ObjItems item;
    [SerializeField] private ObjPotions potion;
    [SerializeField] private EInventorySlot slot;
    [SerializeField] private int id;

    public static event Action<EInventorySlot, int> OnItemRemoved;

    public ObjItems Item { get { return item; } set { item = value; } }
    public ObjPotions Potion { get { return potion; } set { potion = value; } }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Prompt();
        UsePotion();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }

    private void ShowToolTip()
    {
        switch (slot)
        {
            case EInventorySlot.Inventory:
            case EInventorySlot.Equipped:
                // Show item data
                break;
            case EInventorySlot.Potions:
                // Show potion data
                break;
        }
    }

    private void HideToolTip()
    {
        // Hide both item and potion tooltip
    }

    private void Prompt()
    {
        // Show use and drop
        // Set buttons for use and drop
    }

    private void UsePotion()
    {
        if (slot != EInventorySlot.Potions) { return; }

        PotionManager.UsePotion(potion);
        RemoveItem();
    }

    private void RemoveItem()
    {
        switch (slot)
        {
            case EInventorySlot.Inventory:
            case EInventorySlot.Equipped:
                item = null;
                break;

            case EInventorySlot.Potions:
                potion = null;
                break;
        }
        
        OnItemRemoved?.Invoke(slot, id);
    }

    
}
