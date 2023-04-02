using UnityEngine;
using System;
using EasyButtons;

public class PlayerHp : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHP = 100;
    public int _currentHP { get; private set; }

    public static Action<int, int> OnPlayerUpdateHp;

    private void Start()
    {
        ResetPlayerHP();
    }

    private void ResetPlayerHP()
    {
        _currentHP = _maxHP;
        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
    }

    [Button]
    public void TakeDamage(int damage)
    {
        // AudioManager.instance.PlayOneRandomPitch("playerDmg1", .8f, 1.2f);

        _currentHP = Math.Clamp(_currentHP - damage, 0, _maxHP);
        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);

        if (!IsPlayerDead()) return;

        Debug.Log("Dead");  // Game over here
    }


    private bool IsPlayerDead() => _currentHP <= 0;

    [Button]
    public void TestTakeFiftyDamage()
    {
        TakeDamage(50);
    }

    [Button]
    public void TestTakeFourDamage()
    {
        TakeDamage(4);
    }
}
