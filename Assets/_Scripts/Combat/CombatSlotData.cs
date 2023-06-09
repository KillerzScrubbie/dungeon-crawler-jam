using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatSlotData : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] private ObjItems defaultWeapon;
    [SerializeField] private Image outline;
    [SerializeField] private Image icon;
    [SerializeField] private ECombatSlot combatSlot;
    [SerializeField] private int id;

    [Space]
    [SerializeField] private ObjEnergy energy;
    [SerializeField] private ObjTarget target;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private CombatActionTextUpdater combatActionTextUpdater;
    [SerializeField] private RectTransform anchor;
    [SerializeField] private RectTransform potionPromptPanel;
    [SerializeField] private Button useButton;
    [SerializeField] private Color32 colorUsable;
    [SerializeField] private Color32 colorSelected;
    [SerializeField] private Color32 colorDisable;
    [SerializeField] private Color32 colorUnusable;

    private ObjItems item;
    private ObjPotions potion;

    private Inventory inventory;

    private CombatState state;

    private bool currentSelected = false;
    private bool actionUsed = false;

    private int currentEnergy;
    private int currentMana;

    private int energyCost1 = 0;
    private int energyCost2 = 0;
    private int manaCost1 = 0;
    private int manaCost2 = 0;

    public static event Action<ECombatSlot, int> OnSlotClicked;

    private void Start()
    {
        inventory = inventoryUI.Inventory;

        inventory.OnEquippedUpdated += HandleEquippedUpdated;
        inventory.OnPotionUpdated += HandlePotionUpdated;

        energy.OnEnergyUpdated += HandleEnergyUpdated;
        PlayerMana.OnPlayerUpdateMP += HandleManaUpdated;

        OnSlotClicked += HandleSlotClicked;
        InventoryPromptUI.OnPromptExit += Deselect;

        CombatManager.OnActionUsed += HandleActionUsed;
        CombatManager.OnCombatStateChanged += HandleCombatStateChanged;

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
        energyCost1 = item.ActionCost1;
        energyCost2 = item.ActionCost2;
        manaCost1 = item.ManaCost1;
        manaCost2 = item.ManaCost2;
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

    private void HandleEnergyUpdated(int energy)
    {
        currentEnergy = energy;
        HandleOutlineUpdated();
    }

    private void HandleManaUpdated(int mana, int maxMana)
    {
        currentMana = mana;
        HandleOutlineUpdated();
    }

    private void HandleActionUsed(int slotId)
    {
        combatActionTextUpdater.HideText();

        switch (combatSlot)
        {
            case ECombatSlot.Equipped:
                if (slotId != id) { return; }

                actionUsed = true;
                HandleOutlineUpdated();
                return;
            case ECombatSlot.Potions:
                return;
        }
    }

    private void HandleCombatStateChanged(CombatState state)
    {
        this.state = state;
        currentSelected = false;

        if (state == CombatState.PlayerTurn)
        {
            actionUsed = false;
        }

        HandleOutlineUpdated();
    }

    private void HandleOutlineUpdated()
    {
        switch (combatSlot)
        {
            case ECombatSlot.Equipped:
                if (item == null)
                {
                    outline.color = colorDisable;
                    return;
                }

                if (state != CombatState.PlayerTurn)
                {
                    outline.color = colorDisable;
                    return;
                }

                if (actionUsed)
                {
                    outline.color = colorDisable;
                    return;
                }

                if (CheckItemUnusable()) 
                {
                    outline.color = colorUnusable;
                    return; 
                }

                break;
            case ECombatSlot.Potions:
                if (potion == null || state != CombatState.PlayerTurn) 
                {
                    outline.color = colorDisable;
                    return; 
                }

                break;
        }

        if (!currentSelected)
        {
            outline.color = colorUsable;
            return;
        }

        outline.color = colorSelected;
    }

    private bool CheckItemUnusable()
    {
        return (currentEnergy < energyCost1 || currentMana < manaCost1) && (currentEnergy < energyCost2 || currentMana < manaCost2);
    }

    private void HandleSlotClicked(ECombatSlot slot, int id)
    {
        target.ChoosingTarget(false);
        currentSelected = false;

        if (combatSlot == slot && this.id == id)
        {
            currentSelected = true;
        }

        HandleOutlineUpdated();
    }

    public void Deselect()
    {
        currentSelected = false;
        HandleOutlineUpdated();
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (actionUsed) { return; }

        if (CheckItemUnusable()) { return; }

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
        if (state != CombatState.PlayerTurn) { return; }

        switch (combatSlot)
        {
            case ECombatSlot.Equipped:
                if (item == null) { break; }

                OnSlotClicked?.Invoke(combatSlot, id);
                outline.color = colorSelected;
                combatActionTextUpdater.UpdateText(item, id);
                break;

            case ECombatSlot.Potions:
                if (potion == null) { return; }

                useButton.onClick.RemoveAllListeners();
                OnSlotClicked?.Invoke(combatSlot, id);
                outline.color = colorSelected;
                potionPromptPanel.transform.position = anchor.position;
                useButton.onClick.AddListener(() => UsePotion(potion));
                potionPromptPanel.gameObject.SetActive(true);
                break;
        }
    }

    private void UsePotion(ObjPotions potion)
    {
        PotionManager.UsePotion(potion);

        inventory.RemovePotion(id);
        potionPromptPanel.gameObject.SetActive(false);
        outline.color = colorDisable;
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
        OnSlotClicked -= HandleSlotClicked;

        energy.OnEnergyUpdated -= HandleEnergyUpdated;
        PlayerMana.OnPlayerUpdateMP -= HandleManaUpdated;

        InventoryPromptUI.OnPromptExit -= Deselect;

        CombatManager.OnActionUsed -= HandleActionUsed;
        CombatManager.OnCombatStateChanged -= HandleCombatStateChanged;
    }
}

public enum ECombatSlot
{
    Equipped,
    Potions
}
