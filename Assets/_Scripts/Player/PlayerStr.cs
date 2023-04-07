using System;
using UnityEngine;

public class PlayerStr : MonoBehaviour
{
    [SerializeField] private ObjStrength strengthData;

    // public static Action<int> OnPlayerUpdateStr;

    private int _playerStrValue;
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
        strengthData.AddStrength(addedStrValue);
        // OnPlayerUpdateStr?.Invoke(_playerStrValue);
    }

    private void ResetPlayerStr()
    {
        strengthData.ResetStrength();
        // OnPlayerUpdateStr?.Invoke(_playerStrValue);
    }
}
