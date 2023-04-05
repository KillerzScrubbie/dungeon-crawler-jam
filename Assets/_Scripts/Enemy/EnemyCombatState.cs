using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public override void EnterState(EnemyStateManager enemyStateManager)
    {
        Debug.Log("Entered combat state");
    }

    public override void UpdateState(EnemyStateManager enemyStateManager)
    {

    }
}
