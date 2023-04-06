using System;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    int _blockValue;
    public int BlockValue => _blockValue;

    public static Action<int> OnPlayerUpdateBlock;

    void Start()
    {
        ResetBlockValue();
    }

    void OnEnable()
    {
        PotionManager.OnBlockPotionUsed += OnPlayerGetBlock;
    }

    void OnDisable()
    {
        PotionManager.OnBlockPotionUsed -= OnPlayerGetBlock;
    }

    private void OnPlayerGetBlock(int blockPower)
    {
        _blockValue += blockPower;

        OnPlayerUpdateBlock?.Invoke(_blockValue);
    }

    public int GetDamageExceedBlock(int takeDmg)
    {
        int damageExceedBlock = takeDmg - _blockValue;

        _blockValue = Math.Clamp(_blockValue - takeDmg, 0, 99);
        Debug.Log(_blockValue);

        OnPlayerUpdateBlock?.Invoke(_blockValue);

        return damageExceedBlock > 0 ? damageExceedBlock : 0;
    }

    void ResetBlockValue()
    {
        _blockValue = 0;
        OnPlayerUpdateBlock?.Invoke(_blockValue);
    }

}

