using UnityEngine;

public class PlayerCombatState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        Debug.Log("Entered combat state");
    }

    public override void UpdateState(PlayerStateManager playerStateManager)
    {
        Debug.Log("Update combat state");
    }
}
