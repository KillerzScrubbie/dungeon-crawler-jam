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

    public override void LeaveState(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyChaseMovement().OnChaseExited();
    }

    public override void OnFinishedPath(EnemyStateManager enemyStateManager)
    {
        enemyStateManager.GetEnemyChaseMovement().ReachEndOfPath();
    }
}
