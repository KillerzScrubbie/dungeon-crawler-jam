using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Energy", menuName = "Create Energy Instance")]
public class ObjEnergy : SerializedScriptableObject
{
    [SerializeField] private int maxEnergy = 3;

    private int energy = 0;

    public event Action<int> OnEnergyUpdated;

    public bool UpdateEnergy(int energyUsed)
    {
        if (energyUsed > energy) { return false; }

        energy -= energyUsed;

        OnEnergyUpdated?.Invoke(energy);
        return true;
    }

    [Button]
    public void RefreshEnergy()
    {
        energy = maxEnergy;
        OnEnergyUpdated?.Invoke(energy);
    }

    [Button]
    public void UseEnergy()
    {
        UpdateEnergy(1);
    }

    [Button]
    public void GainEnergy()
    {
        UpdateEnergy(-1);
    }
}
