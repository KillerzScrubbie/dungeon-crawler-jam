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
    [SerializeField] private GameObject highlightPanel;

    private EInventorySlot slotToSwap = EInventorySlot.Inventory;
    private int slotIdToSwap = 0;

    public static event Action<EInventorySlot, int> OnItemRemoved;
    public static event Action<ItemData, ObjItems, int> OnItemLooted;
    public static event Action<ItemData, ObjPotions, int> OnPotionLooted;
    public static event Action<ItemData, EInventorySlot, RectTransform> OnPromptClicked;
    public static event Action<EInventorySlot, int, EInventorySlot, int> OnItemSuccessSwapped;

    public ObjItems Item { get { return item; } set { item = value; } }
    public ObjPotions Potion { get { return potion; } set { potion = value; } }
    public EInventorySlot Slot { get { return slot; } }
    public int Id { get { return id; } }

    public bool useTimeDelay = true;
    public static LTDescr delay;

    private void Start()
    {
        LeanTween.reset();

        InventoryUI.OnSwapped += HighlightSlots;
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
            default:
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

    private void HighlightSlots(ItemData itemToSwap, bool isSwapping)
    {
        if (isSwapping)
        {
            slotToSwap = itemToSwap.Slot;
            slotIdToSwap = itemToSwap.Id;

            switch (slot)
            {
                case EInventorySlot.Inventory:
                case EInventorySlot.Equipped:
                    highlightPanel.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (highlightPanel == null) { return; }

            highlightPanel.SetActive(false);
        }
    }

    public void Swap()
    {
        OnItemSuccessSwapped?.Invoke(slotToSwap, slotIdToSwap, slot, id);
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
            case EInventorySlot.Equipped:
                if (item == null) break;
                ShowItemTooltip(item);
                break;
            case EInventorySlot.Potions:
                if (potion == null) break;
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

    private void OnDestroy()
    {
        InventoryUI.OnSwapped -= HighlightSlots;
    }
}
