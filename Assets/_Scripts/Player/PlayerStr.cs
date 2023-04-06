using System;
using UnityEngine;

public class PlayerStr : MonoBehaviour
{
    public static Action<int> OnPlayerUpdateStr;
    int _playerStrValue;
    public int PlayerStrValue => _playerStrValue;

    void Start()
    {
        ResetPlayerStr();
    }

    void OnEnable()
    {
        PotionManager.OnStrPotionUsed += OnPlayerGetStr;
    }
    void OnDisable()
    {
        PotionManager.OnStrPotionUsed -= OnPlayerGetStr;
    }

    private void OnPlayerGetStr(int addedStrValue)
    {
        _playerStrValue += addedStrValue;
        OnPlayerUpdateStr?.Invoke(_playerStrValue);
    }

    void ResetPlayerStr()
    {
        _playerStrValue = 0;
        OnPlayerUpdateStr?.Invoke(_playerStrValue);
    }
}
