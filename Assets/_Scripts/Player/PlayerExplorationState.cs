using System;
using UnityEngine;

public class PlayerExplorationState : PlayerBaseState
{
    public static event Action OnPlayerExplorationState;

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        AudioManager.instance.StopAllSound();
        AudioManager.instance?.Play("bg1");

        OnPlayerExplorationState?.Invoke();
    }

    public override void UpdateState(PlayerStateManager playerStateManager)
    {
        playerStateManager.GetPlayerMovement().UpdateMovement();
    }
}
