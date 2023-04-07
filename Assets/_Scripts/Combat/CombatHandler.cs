using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    [SerializeField] private EffectsProcessor effectsProcessor;
    [SerializeField] private ObjEnergy energy;

    public static event Action<int> OnActionUsed;
    public static event Action<int> OnManaUsed;

    public enum CombatState
    {
        PlayerTurn,
        EnemyTurn
    }

    private CombatState state;

    private void Update()
    {
        
    }

    public void OnActionSelected(Dictionary<EEffectTypes, int> effectList, int slot, int manaCost, int energyCost)
    {
        if (effectList.ContainsKey(EEffectTypes.DamageSingle))
        {
            ChooseTarget(effectList);
            return;
        }

        PerformAction(() => effectsProcessor.ProcessEffect(effectList), manaCost, energyCost);
        OnActionUsed?.Invoke(slot);
    }

    private void ChooseTarget(Dictionary<EEffectTypes, int> effectList)
    {
        // If target is selected
        //PerformAction(() => effectsProcessor.ProcessEffect(effectList));

        // else deselect action
    }

    private void PerformAction(Action action, int manaCost, int energyCost)
    {
        energy.UpdateEnergy(energyCost);
        OnManaUsed?.Invoke(manaCost);
        action();
    }

    public void EndTurn()
    {
        state = CombatState.EnemyTurn;
    }

    public void StartPlayerTurn()
    {
        state = CombatState.PlayerTurn;
    }
}
