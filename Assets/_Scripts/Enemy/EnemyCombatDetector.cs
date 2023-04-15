using System;
using UnityEngine;

public class EnemyCombatDetector : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
   
    public event Action OnCombatDetected;

    private bool isBoss = false;

    private void Start()
    {
        isBoss = enemyData.EnemyGroup.IsBoss;
    }

    public bool EnterCombat()
    {
        OnCombatDetected?.Invoke();
        return isBoss;
    }
}
