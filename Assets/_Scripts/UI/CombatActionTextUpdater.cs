using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatActionTextUpdater : MonoBehaviour
{
    [SerializeField] private List<Button> actionButtons;
    [SerializeField] private List<TextMeshProUGUI> actionTexts;
    [SerializeField] private List<TextMeshProUGUI> actionDescription;
    [SerializeField] private List<GameObject> energyCosts;

    private CombatManager combatManager;

    private void Start()
    {
        combatManager = FindObjectOfType<CombatManager>();
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

        actionTexts[0].text = $"{item.Action1} ({item.ManaCost1} MP)";
        actionDescription[0].text = $"{item.Description1}";
        actionButtons[0].onClick.AddListener(() => combatManager.OnActionSelected(item.EffectList1, slotId, item.ManaCost1, item.ActionCost1));
        for (int i = 0; i < item.ActionCost1; i++)
        {
            energyCosts[i].SetActive(true);
        }

        actionTexts[1].text = $"{item.Action2} ({item.ManaCost2} MP)";
        actionDescription[1].text = $"{item.Description2}";
        actionButtons[1].onClick.AddListener(() => combatManager.OnActionSelected(item.EffectList2, slotId, item.ManaCost2, item.ActionCost2));
        for (int i = 0; i < item.ActionCost2; i++)
        {
            energyCosts[i + 3].SetActive(true);
        }

        gameObject.SetActive(true);
    }

    public void HideText()
    {
        gameObject.SetActive(false);
    }
}