using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectsProcessor : MonoBehaviour
{
    // Player's actions

    [SerializeField] private CombatManager combatManager;

    public static event Action<int> OnHealed;
    public static event Action<int> OnManaGained;
    public static event Action<int> OnBlockGained;

    public void ProcessEffect(Dictionary<EEffectTypes, int> effectList, EnemyCombat target = null)
    {
        int hitInstances = effectList.ContainsKey(EEffectTypes.DamageInstances) ? effectList[EEffectTypes.DamageInstances] : 1;
        List<EnemyCombat> activeEnemies = combatManager.ActiveEnemies;

        foreach (var effect in effectList)
        {
            switch (effect.Key)
            {
                case EEffectTypes.DamageInstances:
                    break;

                case EEffectTypes.DamageSingle:
                    HitEnemy(hitInstances, () => target.TakeDamage(effectList[EEffectTypes.DamageSingle]));
                    break;

                case EEffectTypes.DamageAll:
                    foreach (var enemy in activeEnemies)
                    {
                        enemy.TakeDamage(effectList[EEffectTypes.DamageAll]);
                    }
                    break;

                case EEffectTypes.DamageRandom:
                    HitEnemy(hitInstances, () => activeEnemies[UnityEngine.Random.Range(0, activeEnemies.Count)].TakeDamage(effectList[EEffectTypes.DamageRandom]));
                    break;

                case EEffectTypes.Block:
                    OnBlockGained?.Invoke(effectList[EEffectTypes.Block]);
                    break;

                case EEffectTypes.Heal:
                    OnHealed?.Invoke(effectList[EEffectTypes.Heal]);
                    break;

                case EEffectTypes.ManaThisTurn:
                    OnManaGained?.Invoke(effectList[EEffectTypes.ManaThisTurn]);
                    break;

                default:
                    Debug.Log("wait for next patch lmao");
                    break;
            }
        }
    }

    private void HitEnemy(int hitInstances, Action action)
    {
        for (int i = 0; i < hitInstances; i++)
        {
            action();
        }
    }
}
