using UnityEngine;

public class HideOnCombat : MonoBehaviour
{
    private void Start()
    {
        CombatManager.OnCombatStateChanged += HandleUI;
    }

    private void HandleUI(CombatState state)
    {
        switch (state)
        {
            case CombatState.StartCombat:
                gameObject.SetActive(false);
                break;
            case CombatState.NotInCombat: 
                gameObject.SetActive(true); 
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= HandleUI;
    }
}
