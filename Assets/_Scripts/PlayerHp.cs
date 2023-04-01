using UnityEngine;
using System;
using EasyButtons;

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

    [Button]
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        CheckIfPlayerDead();

        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
    }

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

    void CheckIfPlayerDead()
    {
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            Debug.Log("Player is dead");
        }
    }



}
