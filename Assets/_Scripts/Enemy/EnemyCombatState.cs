using System;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public static event Action<EnemyData> OnCombatEntered;

    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        OnCombatEntered?.Invoke(enemyStateManager.EnemyData);
        enemyStateManager.Kill();
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {

    }

    public override void LeaveState(EnemyStateManager enemyStateManager)
    {
        
    }

    public override void OnFinishedPath(EnemyStateManager enemyStateManager)
    {

    }
}
