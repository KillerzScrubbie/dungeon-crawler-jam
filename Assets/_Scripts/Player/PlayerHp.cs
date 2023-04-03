using UnityEngine;
using System;
using EasyButtons;

public class PlayerHp : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHP = 100;
    public int _currentHP { get; private set; }

    public static Action<int> OnPlayerTakeDamage;
    public static Action<int> OnPlayerHeal;
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
        OnPlayerTakeDamage?.Invoke(damage);

        if (!IsPlayerDead()) return;
        Debug.Log("Dead");  // Game over here
    }

    [Button]
    public void Heal(int healValue)
    {
        if (IsPlayerDead()) return;
        // AudioManager.instance.PlayOneRandomPitch("playerDmg1", .8f, 1.2f);
        _currentHP = Math.Clamp(_currentHP + healValue, 0, _maxHP);
        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
        OnPlayerHeal?.Invoke(healValue);
    }

    private bool IsPlayerDead() => _currentHP <= 0;


    [Button]
    public void TestFiveHeal()
    {
        Heal(5);
    }

    [Button]
    public void TestTakeTwoFiveDamage()
    {
        TakeDamage(25);
    }

    [Button]
    public void TestTakeFourDamage()
    {
        TakeDamage(5);
    }
}