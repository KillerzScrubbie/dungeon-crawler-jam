using UnityEngine;

public class PlayerCombatState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        AudioManager.instance.StopAllSound();
        AudioManager.instance?.Play("bgBattle");

        Debug.Log("Entered combat state");
    }

    public override void UpdateState(PlayerStateManager playerStateManager)
    {

    }
}
