using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private EnemyHealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = new EnemyHealthSystem(0, 100);
        healthSystem.OnHealthUpdated += HandleHealthUpdated;
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

    private void OnDestroy()
    {
        healthSystem.OnHealthUpdated -= HandleHealthUpdated;
    }
}
