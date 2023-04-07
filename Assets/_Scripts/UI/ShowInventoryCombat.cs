using UnityEngine;

public class ShowInventoryCombat : MonoBehaviour
{
    private void Start()
    {
        PlayerMovement.OnCombatEntered += ShowUI;
        HideUI();
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
    }
}
