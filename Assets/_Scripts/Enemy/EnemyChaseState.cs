using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        Debug.Log("Entered chase state");
    }  

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {

    }

    public override void OnLeaveState(EnemyStateManager enemyStateManager)
    {

    }
}
