using UnityEngine;
using System;
using EasyButtons;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int _maxMP = 10;
    public int _currentMP { get; private set; }

    public static Action<int> OnPlayerLoseMP;
    public static Action<int> OnPlayerGetMP;
    public static Action OnPlayerNotEnoughMP;
    public static Action<int, int> OnPlayerUpdateMP;

    void OnEnable()
    {
        PotionManager.OnManaPotionUsed += HealPercentage;

    }
    void OnDisable()
    {
        PotionManager.OnManaPotionUsed -= HealPercentage;
    }

    private void Start()
    {
        ResetPlayerMP();
    }

    private void ResetPlayerMP()
    {
        _currentMP = _maxMP;
        OnPlayerUpdateMP?.Invoke(_currentMP, _maxMP);
    }

    private void HealPercentage(int percentage)
    {
        int mpValue = Mathf.CeilToInt(_maxMP * percentage / 100f);
        AddMP(mpValue);
    }

    [Button]
    public void ReduceMP(int mpUsed)
    {
        if (_currentMP - mpUsed < 0)
        {
            Debug.Log("Player out of mana");  // Game over here
            OnPlayerNotEnoughMP?.Invoke();
            return;
        }

        // AudioManager.instance.PlayOneRandomPitch("playerDmg1", .8f, 1.2f);
        _currentMP = Math.Clamp(_currentMP - mpUsed, 0, _maxMP);
        OnPlayerUpdateMP?.Invoke(_currentMP, _maxMP);
        OnPlayerLoseMP?.Invoke(mpUsed);

        if (!IsPlayerOutOfMP()) return;
    }

    [Button]
    public void AddMP(int addedValue)
    {
        // AudioManager.instance.PlayOneRandomPitch("playerDmg1", .8f, 1.2f);
        _currentMP = Math.Clamp(_currentMP + addedValue, 0, _maxMP);
        OnPlayerUpdateMP?.Invoke(_currentMP, _maxMP);
        OnPlayerGetMP?.Invoke(addedValue);
    }

    private bool IsPlayerOutOfMP() => _currentMP <= 0;


    [Button]
    public void TestFiveHeal()
    {
        AddMP(5);
    }

    [Button]
    public void TestTakeTwoFiveDamage()
    {
        ReduceMP(25);
    }

    [Button]
    public void TestTakeFourDamage()
    {
        ReduceMP(5);
    }

}
