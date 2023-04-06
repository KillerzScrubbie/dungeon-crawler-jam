using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyMovement().OnPatrolEntered();
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyMovement().UpdatePatrolMovement();
    }

    public override void LeaveState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyMovement().OnPatrolExited();
    }

    public override void OnFinishedPath(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyMovement().ReachEndOfPath();
    }
}
