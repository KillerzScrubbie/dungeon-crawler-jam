using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
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
        RemoveItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log(Item.Action1);
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
