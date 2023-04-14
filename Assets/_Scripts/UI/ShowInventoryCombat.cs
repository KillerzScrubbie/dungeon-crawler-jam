using UnityEngine;

public class ShowInventoryCombat : MonoBehaviour
{
    private void Start()
    {
        PlayerMovement.OnCombatEntered += ShowUI;
        CombatManager.OnCombatStateChanged += HandleStateChanged;
        HideUI();
    }

    private void HandleStateChanged(CombatState state)
    {
        switch (state)
        {
            case CombatState.NotInCombat:
            case CombatState.Dead:
                HideUI();
                break;
            default:
                break;
        }
    }

    private void ShowUI()
    {
        gameObject.SetActive(true);
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerMovement.OnCombatEntered -= ShowUI;
        CombatManager.OnCombatStateChanged -= HandleStateChanged;
    }
}
