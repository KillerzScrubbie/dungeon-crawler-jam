using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Strength", menuName = "Create Strength Instance")]
public class ObjStrength : ScriptableObject
{
    [SerializeField] private int strength = 0;

    public event Action<int> OnStrengthUpdated;

    [Button]
    public void ResetStrength()
    {
        strength = 0;
        OnStrengthUpdated?.Invoke(strength);
    }

    [Button]
    public void AddStrength(int amount = 1)
    {
        strength += amount;
        OnStrengthUpdated?.Invoke(strength);
    }

    public int Strength => strength;
}
