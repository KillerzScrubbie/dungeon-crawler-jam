using UnityEngine;
using System;
using EasyButtons;

public class PlayerHp : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHP = 100;
    [SerializeField] PlayerBlock _playerBlockScpt;
    public int _currentHP { get; private set; }

    public static Action<int> OnPlayerTakeDamage;
    public static Action<int> OnPlayerHeal;
    public static Action<int, int> OnPlayerUpdateHp;

    private void Start()
    {
        ResetPlayerHP();  
    }

    void OnEnable()
    {
        PotionManager.OnMaxHpPotionUsed += AddPlayerMaxHp;
        PotionManager.OnHealPotionUsed += HealPercentage;
        EffectsProcessor.OnHealed += Heal;
        EnemyCombat.OnPlayerTakeDamage += TakeDamage;
    }
    void OnDisable()
    {
        PotionManager.OnMaxHpPotionUsed -= AddPlayerMaxHp;
        PotionManager.OnHealPotionUsed -= HealPercentage;
        EffectsProcessor.OnHealed -= Heal;
        EnemyCombat.OnPlayerTakeDamage -= TakeDamage;
    }

    private void ResetPlayerHP()
    {
        _currentHP = _maxHP;
        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
    }

    void AddPlayerMaxHp(int addedVal)
    {
        _maxHP += addedVal;
        _currentHP += addedVal;
        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
    }

    private void HealPercentage(int percentage)
    {
        int healValue = Mathf.CeilToInt(_maxHP * percentage / 100f);
        Heal(healValue);
    }

    [Button]
    public void TakeDamage(int damage)
    {
        int damageAfterBlock = _playerBlockScpt.GetDamageExceedBlock(damage);

        AudioManager.instance?.PlayOneRandomPitch("playerDmg1", .8f, 1.2f);
        _currentHP = Math.Clamp(_currentHP - damageAfterBlock, 0, _maxHP);
        OnPlayerUpdateHp?.Invoke(_currentHP, _maxHP);
        OnPlayerTakeDamage?.Invoke(damageAfterBlock);

        if (!IsPlayerDead()) return;
        Debug.Log("Dead");  // Game over here
    }

    [Button]
    public void Heal(int healValue)
    {
        if (IsPlayerDead()) return;
        AudioManager.instance?.PlayOneRandomPitch("playerDmg1", .8f, 1.2f);
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
    [ContextMenu("Take dmg big")]
    public void TestTakeTwoFiveDamage()
    {
        TakeDamage(25);
    }

    [Button]

    [ContextMenu("Take dmg ")]
    public void TestTakeFourDamage()
    {
        TakeDamage(5);
    }

}
