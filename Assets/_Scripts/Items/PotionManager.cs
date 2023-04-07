using System;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    public static event Action<int> OnHealPotionUsed;
    public static event Action<int> OnManaPotionUsed;
    public static event Action<int> OnMaxHpPotionUsed;
    public static event Action<int> OnBlockPotionUsed;
    public static event Action<int> OnStrPotionUsed;

    public static void UsePotion(ObjPotions potion)
    {
        Dictionary<EEffectTypes, int> effectList = potion.EffectList;

        AudioManager.instance?.PlayRandomPitch("potionDrink", .7f, 1.5f);
        foreach (var effect in effectList)
        {
            ProcessPotionEffect(effect.Key, effect.Value);
        }
    }

    private static void ProcessPotionEffect(EEffectTypes effect, int power)
    {
        switch (effect)
        {
            case EEffectTypes.HealPercent:
                OnHealPotionUsed?.Invoke(power);
                break;
            case EEffectTypes.ManaPercent:
                OnManaPotionUsed?.Invoke(power);
                break;
            case EEffectTypes.Block:
                OnBlockPotionUsed?.Invoke(power);
                break;
            case EEffectTypes.Strength:
                OnStrPotionUsed?.Invoke(power);
                break;
            case EEffectTypes.MaxHP:
                OnMaxHpPotionUsed?.Invoke(power);
                break;
            default:
                break;
        }
    }
}
