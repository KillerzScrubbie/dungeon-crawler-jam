using System;
using UnityEngine;

public class EnemyHealthSystem :MonoBehaviour, IDamageable
{
    public event Action<int, int> OnHealthUpdated;
    public event Action OnDeath;

    private int health = 0;
    private int maxHealth = 100;

    public void SetupEnemy(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;

        OnHealthUpdated?.Invoke(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        health = Math.Clamp(health - damage, 0, maxHealth);
        // Play Dmg effect

        OnHealthUpdated?.Invoke(health, maxHealth);

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health != 0) { return; }

        OnDeath?.Invoke();
    }

    public void Heal(int heal)
    {
        health = Math.Clamp(health + heal, 0, maxHealth);
        // Play heal effect

        OnHealthUpdated?.Invoke(health, maxHealth);
    }
}
