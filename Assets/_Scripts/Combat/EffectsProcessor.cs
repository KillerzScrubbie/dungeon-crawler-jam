using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EffectsProcessor : MonoBehaviour
{
    // Player's actions

    [SerializeField] private ObjStrength strength;
    [SerializeField] private CombatManager combatManager;

    public static event Action<int> OnHealed;
    public static event Action<int> OnManaGained;
    public static event Action<int> OnBlockGained;

    public void ProcessEffect(Dictionary<EEffectTypes, int> effectList, EnemyCombat target = null)
    {
        int hitInstances = effectList.ContainsKey(EEffectTypes.DamageInstances) ? effectList[EEffectTypes.DamageInstances] : 1;
        List<EnemyCombat> activeEnemies = combatManager.ActiveEnemies;
        int extraDamage = strength.Strength;

        foreach (var effect in effectList)
        {
            switch (effect.Key)
            {
                case EEffectTypes.DamageInstances:
                    break;

                case EEffectTypes.DamageSingle:
                    AudioManager.instance?.PlayRandomPitch("playerAtkOne", .8f, 1.2f);
                    HitEnemy(hitInstances, () => target.TakeDamage(effectList[EEffectTypes.DamageSingle] + extraDamage));
                    break;

                case EEffectTypes.DamageAll:

                    AudioManager.instance?.PlayRandomPitch("playerAtkAll", .6f, 1.5f);
                    foreach (var enemy in activeEnemies)
                    {
                        enemy.TakeDamage(effectList[EEffectTypes.DamageAll] + extraDamage);
                    }
                    break;

                case EEffectTypes.DamageRandom:

                    AudioManager.instance?.PlayRandomPitch("playerAtkOne", .6f, 2.2f);
                    HitEnemy(hitInstances, () => activeEnemies[UnityEngine.Random.Range(0, activeEnemies.Count)].TakeDamage(effectList[EEffectTypes.DamageRandom] + extraDamage));
                    break;

                case EEffectTypes.Block:

                    AudioManager.instance?.PlayRandomPitch("playerGetBlock", .6f, 2.2f);

                    OnBlockGained?.Invoke(effectList[EEffectTypes.Block]);
                    break;

                case EEffectTypes.Heal:

                    AudioManager.instance?.PlayRandomPitch("playerGetHeal", .6f, 2.2f);
                    OnHealed?.Invoke(effectList[EEffectTypes.Heal]);
                    break;

                case EEffectTypes.ManaThisTurn:
                    AudioManager.instance?.PlayRandomPitch("playerGetMana", .6f, 2.2f);
                    OnManaGained?.Invoke(effectList[EEffectTypes.ManaThisTurn]);
                    break;

                default:
                    Debug.Log("wait for next patch lmao");
                    break;
            }
        }
    }

    private async void HitEnemy(int hitInstances, Action action)
    {
        for (int i = 0; i < hitInstances; i++)
        {
            action();
            await Task.Delay(150);
        }
    }
}
