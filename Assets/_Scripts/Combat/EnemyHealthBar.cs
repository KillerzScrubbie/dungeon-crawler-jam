using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealthSystem healthSystem;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Awake()
    {
        healthSystem.OnHealthUpdated += HandleHealthUpdated;
        healthSystem.OnDeath += HandleDeath;
    }

    public void Setup(int maxHealth)
    {
        healthSystem.SetupEnemy(maxHealth);
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        healthSystem.OnHealthUpdated -= HandleHealthUpdated;
        healthSystem.OnDeath -= HandleDeath;
    }
}
