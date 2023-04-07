using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectsProcessor : MonoBehaviour
{
    // Player's actions

    [SerializeField] private List<EnemyHealthSystem> enemies;

    public static event Action<int> OnHealed;
    public static event Action<int> OnManaGained;
    public static event Action<int> OnBlockGained;

    public void ProcessEffect(Dictionary<EEffectTypes, int> effectList, EnemyHealthSystem target = null)
    {
        int hitInstances = effectList.ContainsKey(EEffectTypes.DamageInstances) ? effectList[EEffectTypes.DamageInstances] : 1;

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
                    foreach (var enemy in enemies)
                    {
                        if (!enemy.isActiveAndEnabled) { continue; }

                        enemy.TakeDamage(effectList[EEffectTypes.DamageAll]);
                    }
                    break;

                case EEffectTypes.Block:
                    OnBlockGained?.Invoke(effectList[EEffectTypes.Block]);
                    break;

                default:
                    Debug.Log("wait");
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
