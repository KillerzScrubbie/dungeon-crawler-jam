using System;
using UnityEngine;

public class EnemyCombatDetector : MonoBehaviour
{
    public event Action OnCombatDetected;

    public void EnterCombat()
    {
        OnCombatDetected?.Invoke();
    }
}
