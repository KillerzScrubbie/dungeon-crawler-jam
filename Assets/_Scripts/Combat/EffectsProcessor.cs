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
        int damageDone = 0;
        List<EnemyCombat> activeEnemies = combatManager.ActiveEnemies;
        int extraDamage = strength.Strength;

        foreach (var effect in effectList)
        {
            switch (effect.Key)
            {
                case EEffectTypes.DamageInstances:
                case EEffectTypes.DamageToBlock:
                case EEffectTypes.Heal:
                    break;

                case EEffectTypes.DamageSingle:
                    
                    HitEnemy(hitInstances, () => damageDone += target.TakeDamage(effectList[EEffectTypes.DamageSingle] + extraDamage));
                    break;

                case EEffectTypes.DamageAll:

                    AudioManager.instance?.PlayRandomPitch("playerAtkAll", .6f, 1.5f);
                    foreach (var enemy in activeEnemies)
                    {
                        damageDone += enemy.TakeDamage(effectList[EEffectTypes.DamageAll] + extraDamage);
                    }
                    break;

                case EEffectTypes.DamageRandom:

                    HitEnemy(hitInstances, () => damageDone += activeEnemies[UnityEngine.Random.Range(0, activeEnemies.Count)].TakeDamage(effectList[EEffectTypes.DamageRandom] + extraDamage));
                    break;

                case EEffectTypes.Block:

                    AudioManager.instance?.PlayRandomPitch("playerGetBlock", .6f, 2.2f);

                    OnBlockGained?.Invoke(effectList[EEffectTypes.Block]);
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

        ProcessDamageToBlock(effectList, damageDone);
        ProcessHealing(effectList);
    }

    private void ProcessHealing(Dictionary<EEffectTypes, int> effectList)
    {
        if (!effectList.ContainsKey(EEffectTypes.Heal)) { return; }

        AudioManager.instance?.PlayRandomPitch("playerGetHeal", .6f, 2.2f);
        OnHealed?.Invoke(effectList[EEffectTypes.Heal]);
    }

    private void ProcessDamageToBlock(Dictionary<EEffectTypes, int> effectList, int damageDone)
    {
        if (!effectList.ContainsKey(EEffectTypes.DamageToBlock)) { return; } // Give block after calculating dmg dealt.

        AudioManager.instance?.PlayRandomPitch("playerGetBlock", .6f, 2.2f);
        OnBlockGained?.Invoke(damageDone);
    }

    private async void HitEnemy(int hitInstances, Action action)
    {
        for (int i = 0; i < hitInstances; i++)
        {
            action();
            AudioManager.instance?.PlayRandomPitch("playerAtkOne", .8f, 1.6f);
            await Task.Delay(150);
        }
    }
}
