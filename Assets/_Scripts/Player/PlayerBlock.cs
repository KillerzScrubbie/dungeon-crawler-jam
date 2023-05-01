using System;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    int _blockValue;
    public int BlockValue => _blockValue;

    public static Action<int> OnPlayerUpdateBlock;
    public static Action<int> OnUseBlockValue;

    void Start()
    {
        CombatManager.OnCombatStateChanged += HandleNewTurn;

        ResetBlockValue();
    }

    void OnEnable()
    {
        PotionManager.OnBlockPotionUsed += OnPlayerGetBlock;
        EffectsProcessor.OnBlockGained += OnPlayerGetBlock;
    }

    void OnDisable()
    {
        PotionManager.OnBlockPotionUsed -= OnPlayerGetBlock;
        EffectsProcessor.OnBlockGained -= OnPlayerGetBlock;
    }

    private void OnPlayerGetBlock(int blockPower)
    {
        _blockValue += blockPower;

        OnPlayerUpdateBlock?.Invoke(_blockValue);
    }

    public int GetDamageExceedBlock(int takeDmg)
    {
        int damageExceedBlock = takeDmg - _blockValue;

        OnUseBlockValue?.Invoke(Math.Clamp(takeDmg, 0, _blockValue));

        _blockValue = Math.Clamp(_blockValue - takeDmg, 0, 99);

        OnPlayerUpdateBlock?.Invoke(_blockValue);

        return damageExceedBlock > 0 ? damageExceedBlock : 0;
    }

    void ResetBlockValue()
    {
        _blockValue = 0;
        OnPlayerUpdateBlock?.Invoke(_blockValue);
    }

    private void HandleNewTurn(CombatState state)
    {
        switch (state)
        {
            case CombatState.PlayerTurn:
            case CombatState.NotInCombat:
                ResetBlockValue();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= HandleNewTurn;
    }
}

