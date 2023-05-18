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

    public static event Action<int, Transform> OnEnemyTakeDamage;
    public static event Action<int, Transform> OnEnemyHeal;

    private void Start()
    {
        healthSystem = GetComponent<EnemyHealthSystem>();
    }

    public void SetupEnemy(ObjEnemy enemy)
    {
        this.enemy = enemy;
        nameText.text = enemy.EnemyName;
    }

    public int TakeDamage(int damage)
    {
        int damageTaken = healthSystem.TakeDamage(damage);
        OnEnemyTakeDamage?.Invoke(damage, transform);

        return damageTaken;
    }

    public void PerformAction()
    {
        Dictionary<EEffectTypes, int> intents = enemy.GetRandomIntent();

        foreach (var intent in intents)
        {
            switch (intent.Key)
            {
                case EEffectTypes.DamageSingle:
                    AudioManager.instance?.Play("enemyAtk", .8f, 1.2f);
                    OnPlayerTakeDamage?.Invoke(intent.Value);
                    break;
                case EEffectTypes.Heal:
                    AudioManager.instance?.Play("enemyHeal", .8f, 1.2f);
                    healthSystem.Heal(intent.Value);

                    OnEnemyHeal?.Invoke(intent.Value, transform);
                    break;
                default:
                    break;
            }
        }
    }
}
