using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyChaseMovement().OnChaseEntered();
    }  

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyChaseMovement().UpdateChaseMovement();
    }

    public override void OnLeaveState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyChaseMovement().OnChaseExited();
    }
}
