using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCanvas;
    [SerializeField] private ObjEnergy energy;
    [SerializeField] private EffectsProcessor effectsProcessor;

    [Space]
    [Header("UI Slots List")]
    [SerializeField] private List<Image> enemySlot;
    [SerializeField] private List<EnemyHealthBar> healthBars;
    [SerializeField] private List<Image> energyIcons;

    public static event Action<int> OnActionUsed;
    public static event Action<int> OnManaUsed;

    private ObjEnemyGroup currentCombatGroup;
    private List<ObjEnemy> enemyList = new();

    private void Start()
    {
        CombatEntered();
        CombatFinished();

        PlayerMovement.OnCombatEntered += CombatEntered;
        EnemyCombatState.OnCombatEntered += SetupCombatScreen;
        energy.OnEnergyUpdated += HandleEnergyUpdated;
    }

    private void CombatEntered()
    {
        energy.RefreshEnergy();
        combatCanvas.SetActive(true);
    }

    private void CombatFinished()
    {
        combatCanvas.SetActive(false);
    }

    private void SetupCombatScreen(EnemyData enemyData)
    {
        currentCombatGroup = enemyData.EnemyGroup;
        enemyList.Clear();
        enemyList = currentCombatGroup.EnemyGroup;

        foreach (Image enemyImage in enemySlot)
        {
            enemyImage.sprite = null;
            enemyImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            ObjEnemy enemy = enemyList[i];
            Image currentEnemy = enemySlot[i];

            currentEnemy.sprite = enemy.Image;
            currentEnemy.gameObject.SetActive(true);
            healthBars[i].Setup(enemy.MaxHealth);
        }
    }

    private void HandleEnergyUpdated(int energy)
    {
        for (int i = 0; i < energy; i++)
        {
            switch (i)
            {
                case > 2:
                    energyIcons[i].gameObject.SetActive(true);
                    break;
                default:
                    energyIcons[i].DOFade(1f, 0f);
                    break;
            }
        }

        for (int i = energyIcons.Count - 1; i >= energy; i--)
        {
            switch (i)
            {
                case > 2:
                    energyIcons[i].gameObject.SetActive(false);
                    break;
                default:
                    energyIcons[i].DOFade(0.3f, 0f);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        PlayerMovement.OnCombatEntered -= CombatEntered;
        EnemyCombatState.OnCombatEntered -= SetupCombatScreen;
        energy.OnEnergyUpdated -= HandleEnergyUpdated;
    }
    

    public enum CombatState
    {
        PlayerTurn,
        EnemyTurn
    }

    private CombatState state;

    private void Update()
    {

    }

    public void OnActionSelected(Dictionary<EEffectTypes, int> effectList, int slot, int manaCost, int energyCost)
    {
        if (effectList.ContainsKey(EEffectTypes.DamageSingle))
        {
            ChooseTarget(effectList);
            return;
        }

        PerformAction(() => effectsProcessor.ProcessEffect(effectList), manaCost, energyCost);
        OnActionUsed?.Invoke(slot);
    }

    private void ChooseTarget(Dictionary<EEffectTypes, int> effectList)
    {
        // If target is selected
        //PerformAction(() => effectsProcessor.ProcessEffect(effectList));

        // else deselect action
    }

    private void PerformAction(Action action, int manaCost, int energyCost)
    {
        energy.UpdateEnergy(energyCost);
        OnManaUsed?.Invoke(manaCost);
        action();
    }

    public void EndTurn()
    {
        state = CombatState.EnemyTurn;
    }

    public void StartPlayerTurn()
    {
        state = CombatState.PlayerTurn;
    }
}
