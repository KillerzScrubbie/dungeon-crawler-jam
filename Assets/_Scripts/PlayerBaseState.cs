using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(PlayerStateManager playerStateManager);

    public abstract void UpdateState(PlayerStateManager playerStateManager);

    //abstract void
}
