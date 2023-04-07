using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "HP/MP", menuName = "Create HP/MP Instance")]
public class ObjValue : SerializedScriptableObject
{
    [SerializeField] private int maxValue = 30;

    private int currentValue = 0;

    public event Action<int> OnValueUpdated;

    public int Value { get { return currentValue; } }

    public bool UpdateValue(int value)
    {
        currentValue -= value;

        OnValueUpdated?.Invoke(currentValue);
        return true;
    }

    [Button]
    public void FullHeal()
    {
        currentValue = maxValue;
        OnValueUpdated?.Invoke(currentValue);
    }

    [Button]
    public void LoseValue(int value = 1)
    {
        UpdateValue(value);
    }

    [Button]
    public void GainValue(int value = 1)
    {
        UpdateValue(-value);
    }
}
