using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        Debug.Log("Entered patrol state");
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {

    }
}
