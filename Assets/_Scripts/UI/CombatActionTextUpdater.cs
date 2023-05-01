using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatActionTextUpdater : MonoBehaviour
{
    [SerializeField] private ObjEnergy energy;
    [SerializeField] private Color32 colorEnable;
    [SerializeField] private Color32 colorDisable;
    [SerializeField] private List<Button> actionButtons;
    [SerializeField] private List<TextMeshProUGUI> actionTexts;
    [SerializeField] private List<TextMeshProUGUI> actionDescription;
    [SerializeField] private List<GameObject> energyCosts;

    private CombatManager combatManager;

    private int currentEnergy = 0;
    private int currentMana = 0;

    private void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();

        CombatManager.OnCombatStateChanged += HandleCombatStateChanged;
        energy.OnEnergyUpdated += HandleEnergyUpdated;
        PlayerMana.OnPlayerUpdateMP += HandleManaUpdated;

        HideText();
    }

    public void UpdateText(ObjItems item, int slotId)
    {
        for (int i = 0; i < actionButtons.Count; i++)
        {
            actionButtons[i].onClick.RemoveAllListeners();
        }

        for (int i = 0; i < energyCosts.Count; i++)
        {
            energyCosts[i].SetActive(false);
        }

        #region Action 1
        TextMeshProUGUI actionText1 = actionTexts[0];
        int manaCost1 = item.ManaCost1;
        int energyCost1 = item.ActionCost1;

        actionText1.text = $"{item.Action1} ({manaCost1} MP)";
        actionText1.color = currentEnergy < energyCost1 || currentMana < manaCost1 ? colorDisable : colorEnable;
        actionDescription[0].text = $"{item.Description1}";
        actionButtons[0].onClick.AddListener(() => combatManager.OnActionSelected(item.EffectList1, slotId, item.ManaCost1, item.ActionCost1));

        for (int i = 0; i < item.ActionCost1; i++)
        {
            energyCosts[i].SetActive(true);
        }
        #endregion

        #region Action 2
        TextMeshProUGUI actionText2 = actionTexts[1];
        int manaCost2 = item.ManaCost2;
        int energyCost2 = item.ActionCost2;

        actionText2.text = $"{item.Action2} ({manaCost2} MP)";
        actionText2.color = currentEnergy < energyCost2 || currentMana < manaCost2 ? colorDisable : colorEnable;
        actionDescription[1].text = $"{item.Description2}";
        actionButtons[1].onClick.AddListener(() => combatManager.OnActionSelected(item.EffectList2, slotId, item.ManaCost2, item.ActionCost2));

        for (int i = 0; i < item.ActionCost2; i++)
        {
            energyCosts[i + 3].SetActive(true);
        }
        #endregion

        gameObject.SetActive(true);
    }

    private void HandleCombatStateChanged(CombatState state)
    {
        switch (state)
        {
            case CombatState.EnemyTurn:
            case CombatState.Victory:
            case CombatState.Dead:
                HideText();
                break;
        }
    }

    public void HideText()
    {
        gameObject.SetActive(false);
    }

    private void HandleEnergyUpdated(int newEnergy)
    {
        currentEnergy = newEnergy;
    }

    private void HandleManaUpdated(int newMana, int maxMana)
    {
        currentMana = newMana;
    }

    private void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= HandleCombatStateChanged;
        energy.OnEnergyUpdated -= HandleEnergyUpdated;
        PlayerMana.OnPlayerUpdateMP -= HandleManaUpdated;
    }
}
