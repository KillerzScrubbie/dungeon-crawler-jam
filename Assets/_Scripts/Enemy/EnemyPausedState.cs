using UnityEngine;

public class EnemyPausedState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        Debug.Log("Entered paused state");
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
