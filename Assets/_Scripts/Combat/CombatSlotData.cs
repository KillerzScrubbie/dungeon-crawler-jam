using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CombatSlotData : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private ObjItems defaultWeapon;
    [SerializeField] private Image outline;
    [SerializeField] private Image icon;
    [SerializeField] private ECombatSlot combatSlot;
    [SerializeField] private int id;

    [Space]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private GameObject actionsPanel;
    [SerializeField] private List<Button> actionButtons;
    [SerializeField] private List<TextMeshProUGUI> actionTexts;
    [SerializeField] private List<TextMeshProUGUI> actionDescription;
    [SerializeField] private List<GameObject> energyCosts;
    [SerializeField] private RectTransform anchor;
    [SerializeField] private RectTransform potionPromptPanel;
    [SerializeField] private Color32 colorUsable;
    [SerializeField] private Color32 colorSelected;
    [SerializeField] private Color32 colorDisable;

    private ObjItems item;
    private ObjPotions potion;

    private Inventory inventory;

    private void Start()
    {
        inventory = inventoryUI.Inventory;
        inventory.OnEquippedUpdated += HandleEquippedUpdated;
        inventory.OnPotionUpdated += HandlePotionUpdated;

        HandleEquippedUpdated();
        HandlePotionUpdated();

        if (id != -1) { return; }

        SetItem(defaultWeapon);
    }

    private void HandleEquippedUpdated()
    {
        if (combatSlot == ECombatSlot.Potions) { return; }

        if (id < 0) { return; }

        Dictionary<int, ObjItems> equippedList = inventory.GetEquippedList();

        if (!equippedList.ContainsKey(id)) 
        {   
            SetItem(null);
            return; 
        }

        SetItem(equippedList[id]);
    }

    private void SetItem(ObjItems item)
    {
        this.item = item;

        if (item == null)
        {
            outline.color = colorDisable;
            icon.enabled = false;
            return;
        }

        outline.color = colorUsable;
        icon.enabled = true;
        icon.sprite = item.Icon;
    }

    private void HandlePotionUpdated()
    {
        if (combatSlot == ECombatSlot.Equipped) { return; }

        Dictionary<int, ObjPotions> potionList = inventory.GetPotionList();

        if (!potionList.ContainsKey(id))
        {
            SetPotion(null);
            return;
        }

        SetPotion(potionList[id]);
    }

    private void SetPotion(ObjPotions potion)
    {
        this.potion = potion;

        if (potion == null)
        {
            outline.color = colorDisable;
            icon.enabled = false;
            return;
        }

        outline.color = colorUsable;
        icon.enabled = true;
        icon.sprite = potion.Icon;
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
        for (int i = 0; i < energyCosts.Count; i++)
        {
            energyCosts[i].SetActive(false);
        }

        switch (combatSlot)
        {
            case ECombatSlot.Equipped:      
                actionTexts[0].text = $"{item.Action1} ({item.ManaCost1} MP)";
                actionDescription[0].text = $"{item.Description1}";
                for (int i = 0; i < item.ActionCost1; i++)
                {
                    energyCosts[i].SetActive(true);
                }

                actionTexts[1].text = $"{item.Action2} ({item.ManaCost2} MP)";
                actionDescription[1].text = $"{item.Description2}";
                for (int i = 0; i < item.ActionCost2; i++)
                {
                    energyCosts[i + 3].SetActive(true);
                }

                outline.color = colorSelected;
                actionsPanel.SetActive(true);
                break;
            case ECombatSlot.Potions:
                potionPromptPanel.transform.position = anchor.position;
                potionPromptPanel.gameObject.SetActive(true);
                break;
        }
    }

    private void ShowToolTip()
    {
        switch (combatSlot)
        {
            case ECombatSlot.Equipped:
                if (item == null) { break; }
                ToolTipSystem.Show(item.GetActionDescription(), item.Name);
                break;
            case ECombatSlot.Potions:
                if (potion == null) { break; }
                ToolTipSystem.Show(potion.Description, potion.name);
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

    private void OnDestroy()
    {
        inventory.OnEquippedUpdated -= HandleEquippedUpdated;
        inventory.OnPotionUpdated -= HandlePotionUpdated;
    }
}

public enum ECombatSlot
{
    Equipped,
    Potions
}
