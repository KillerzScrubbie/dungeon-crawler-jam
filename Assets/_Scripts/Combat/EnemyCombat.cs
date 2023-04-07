using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;

    private EnemyHealthSystem healthSystem;
    private ObjEnemy enemy;

    public static event Action<int> OnPlayerTakeDamage;

    private void Start()
    {
        healthSystem = GetComponent<EnemyHealthSystem>();
    }

    public void SetupEnemy(ObjEnemy enemy)
    {
        this.enemy = enemy;
        nameText.text = enemy.EnemyName;
    }

    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage);
    }

    public void PerformAction()
    {
        Dictionary<EEffectTypes, int> intents = enemy.GetRandomIntent();

        foreach (var intent in intents)
        {
            switch (intent.Key)
            {
                case EEffectTypes.DamageSingle:
                    AudioManager.instance?.PlayRandomPitch("enemyAtk", .8f, 1.2f);
                    OnPlayerTakeDamage?.Invoke(intent.Value);
                    break;
                case EEffectTypes.Heal:
                    AudioManager.instance?.PlayRandomPitch("enemyHeal", .8f, 1.2f);
                    healthSystem.Heal(intent.Value);
                    break;
                default:
                    break;
            }
        }
    }
}
