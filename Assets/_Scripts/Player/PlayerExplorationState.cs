using System;
using UnityEngine;

public class PlayerExplorationState : PlayerBaseState
{
    public static event Action OnPlayerExplorationState;

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        OnPlayerExplorationState?.Invoke();
    }

    public override void UpdateState(PlayerStateManager playerStateManager)
    {
        playerStateManager.GetPlayerMovement().UpdateMovement();
    }
}
