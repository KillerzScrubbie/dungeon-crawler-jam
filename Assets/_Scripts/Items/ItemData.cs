using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private ObjItems item;
    [SerializeField] private ObjPotions potion;
    [SerializeField] private EInventorySlot slot;
    [SerializeField] private int id;
    [SerializeField] private RectTransform promptAnchorTransform;

    public static event Action<EInventorySlot, int> OnItemRemoved;
    public static event Action<ItemData, ObjItems, int> OnItemLooted;
    public static event Action<ItemData, ObjPotions, int> OnPotionLooted;
    public static event Action<ItemData, EInventorySlot, RectTransform> OnPromptClicked;

    public ObjItems Item { get { return item; } set { item = value; } }
    public ObjPotions Potion { get { return potion; } set { potion = value; } }

    public bool useTimeDelay = true;
    public static LTDescr delay;

    void Start()
    {
        LeanTween.reset();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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

    private void ProcessClick()
    {
        switch (slot)
        {
            case EInventorySlot.Inventory:
            case EInventorySlot.Equipped:
            case EInventorySlot.Potions:
                Prompt();
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

    private void ShowItemTooltip(ObjItems _item)
    {
        ToolTipSystem.Show(_item.GetActionDescription(), _item.Name);
    }

    private void ShowPotionTooltip(ObjPotions _potion)
    {
        ToolTipSystem.Show(_potion.Description, _potion.name);
    }

    private void ShowToolTip()
    {
        switch (slot)
        {
            case EInventorySlot.Inventory:
                ShowItemTooltip(item);
                break;
            case EInventorySlot.Equipped:
                if (item == null) break;
                ShowItemTooltip(item);
                break;
            case EInventorySlot.Potions:
                ShowPotionTooltip(potion);
                break;
            case EInventorySlot.Chest:
                if (item != null)
                {
                    ShowItemTooltip(item);
                    break;
                }
                else if (potion != null)
                {

                    ShowPotionTooltip(potion);
                    // show potion
                    break;
                }

                break;
        }
    }

    private void HideToolTip()
    {
        if (ToolTipSystem.current != null)
        {
            ToolTipSystem.Hide();
        }
    }

    private void Prompt()
    {
        OnPromptClicked?.Invoke(this, slot, promptAnchorTransform);
    }

    public void UsePotion()
    {
        if (slot != EInventorySlot.Potions) { return; }

        PotionManager.UsePotion(potion);
        RemoveItem();
    }

    public void RemoveItem()
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
