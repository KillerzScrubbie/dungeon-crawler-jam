using System;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public event Action<int, int> OnHealthUpdated;
    public event Action OnDeath;
    public static event Action<EnemyCombat> OnEnemyDeath;

    private int health = 0;
    private int maxHealth = 100;

    private int _lastTakeDamage;
    public int GetLastTakeDamage => _lastTakeDamage;

    private EnemyCombat enemyCombat;

    private void Start()
    {
        enemyCombat = GetComponent<EnemyCombat>();
    }

    public void SetupEnemy(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;

        OnHealthUpdated?.Invoke(health, maxHealth);
    }

    public int TakeDamage(int damage)
    {
        int originalHealth = health;
        health = Math.Clamp(health - damage, 0, maxHealth);
        // Play Dmg effect
        _lastTakeDamage = damage;
        OnHealthUpdated?.Invoke(health, maxHealth);

        CheckDeath();

        return originalHealth - health;
    }


    private void CheckDeath()
    {
        if (health > 0) { return; }

        OnEnemyDeath?.Invoke(enemyCombat);
        OnDeath?.Invoke();
    }

    public void Heal(int heal)
    {
        health = Math.Clamp(health + heal, 0, maxHealth);
        // Play heal effect

        OnHealthUpdated?.Invoke(health, maxHealth);
    }
}
