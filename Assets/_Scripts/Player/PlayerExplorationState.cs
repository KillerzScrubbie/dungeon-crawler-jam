using UnityEngine;

public class PlayerExplorationState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager playerStateManager)
    {
        AudioManager.instance.StopAllSound();
        AudioManager.instance?.Play("bg1");

        Debug.Log("Entered exploration state");
    }

    public override void UpdateState(PlayerStateManager playerStateManager)
    {
        playerStateManager.GetPlayerMovement().UpdateMovement();
    }
}
