using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCanvas;

    private void Start()
    {
        PlayerMovement.OnCombatEntered += CombatEntered;
    }

    private void CombatEntered()
    {
        combatCanvas.SetActive(true);
    }

    private void CombatFinished()
    {
        combatCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        PlayerMovement.OnCombatEntered -= CombatEntered;
    }
}
