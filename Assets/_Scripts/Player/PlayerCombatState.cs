using System;
using UnityEngine;

public class PlayerCombatState : PlayerBaseState
{
    public static event Action OnPlayerCombatState;

    public override void EnterState(PlayerStateManager playerStateManager)
    {
        AudioManager.instance.StopAllSound();
        AudioManager.instance?.Play("bgBattle");

        OnPlayerCombatState?.Invoke();
    }

    public override void UpdateState(PlayerStateManager playerStateManager)
    {

    }

    public void BossTheme()
    {
        AudioManager.instance.StopAllSound();
        AudioManager.instance?.Play("bgBattle");
    }
}
