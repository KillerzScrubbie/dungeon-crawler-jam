using UnityEngine;
using System;

public class PlayerHp : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHP = 100;
    public int _currentHP { get; private set; }
    public static Action<int, int> OnPlayerUpdateHp;

    void Start()
    {
        ResetPlayerHP();
    }

    void ResetPlayerHP()
    {
        _currentHP = _maxHP;
        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        CheckIfPlayerDead();

        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
    }

    void CheckIfPlayerDead()
    {
        if (_currentHP <= 0) Debug.Log("Player is dead");
    }



}
