public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateManager enemyStateManager);

    public abstract void UpdateState(EnemyStateManager enemyStateManager);

    public abstract void LeaveState(EnemyStateManager enemyStateManager);
    public abstract void OnFinishedPath(EnemyStateManager enemyStateManager);
}
