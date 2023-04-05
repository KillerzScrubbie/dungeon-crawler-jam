using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private ObjItems item;
    [SerializeField] private ObjPotions potion;
    [SerializeField] private EInventorySlot slot;
    [SerializeField] private int id;

    public static event Action<EInventorySlot, int> OnItemRemoved;
    public static event Action<ItemData, ObjItems, int> OnItemLooted;
    public static event Action<ItemData, ObjPotions, int> OnPotionLooted;

    private Canvas canvas;
    private RectTransform dragPreview;

    public ObjItems Item { get { return item; } set { item = value; } }
    public ObjPotions Potion { get { return potion; } set { potion = value; } }

    private void Awake()
    {
        dragPreview = GetComponent<RectTransform>();
        //dragPreview = GameObject.Find("DragPreview").GetComponent<RectTransform>();
        canvas = GameObject.Find("Inventory Canvas").GetComponent<Canvas>();
        //Debug.Log(transform.root.name);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Prompt();
        ProcessClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Click");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragPreview.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
    }

    private void ProcessClick()
    {
        switch (slot)
        {
            case EInventorySlot.Inventory:
            case EInventorySlot.Equipped:
                // Prompt to drop
                break;
            case EInventorySlot.Potions:
                UsePotion();
                break;
            case EInventorySlot.Chest:
                if (item != null)
                {
                    OnItemLooted?.Invoke(this, item, id);
                    break;
                }
                else if (potion != null)
                {
                    OnPotionLooted?.Invoke(this, potion, id);
                    break;
                }

                break;
        }
    }

    public void Loot()
    {
        RemoveItem();
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
            case EInventorySlot.Chest:
                if (item != null)
                {
                    // show item
                    break;
                }
                else if (potion != null)
                {
                    // show potion
                    break;
                }
                
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

            case EInventorySlot.Chest:
                item = null;
                potion = null;
                break;
        }
        
        OnItemRemoved?.Invoke(slot, id);
    }
}
