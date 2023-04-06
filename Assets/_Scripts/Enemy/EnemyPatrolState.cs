using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyMovement().OnStateEntered();
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyMovement().UpdateMovement();
    }

    public override void OnLeaveState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyMovement().OnStateExited();
    }
}
