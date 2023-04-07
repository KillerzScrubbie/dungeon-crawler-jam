using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCanvas;
    [SerializeField] private ObjEnergy energy;
    [SerializeField] private EffectsProcessor effectsProcessor;
    [SerializeField] private ObjTarget targetCheck;

    [Space]
    [Header("UI Slots List")]
    [SerializeField] private List<Image> enemySlot;
    [SerializeField] private List<EnemyHealthBar> healthBars;
    [SerializeField] private List<Image> energyIcons;
    [SerializeField] private List<GameObject> targetConfirms;

    public static event Action<int> OnActionUsed;
    public static event Action<int> OnManaUsed;

    private ObjEnemyGroup currentCombatGroup;
    private List<ObjEnemy> enemyList = new();

    private EnemyHealthSystem enemyTarget;
    private Action savedAction;
    private int savedManaCost;
    private int savedEnergyCost;
    private int savedSlot;

    private int currentEnemiesInCombat = 0;
    public enum CombatState
    {
        PlayerTurn,
        EnemyTurn
    }

    private CombatState state;


    private void Start()
    {
        CombatEntered();
        CombatFinished();

        PlayerMovement.OnCombatEntered += CombatEntered;
        EnemyCombatState.OnCombatEntered += SetupCombatScreen;
        EnemyHealthSystem.OnEnemyDeath += RemoveEnemyFromCombat;
        ObjTarget.OnTargetting += HandleTargettingUpdated;
        TargetButton.OnTargetChosen += ChooseTarget;
        energy.OnEnergyUpdated += HandleEnergyUpdated;
    }

    private void CombatEntered()
    {
        energy.RefreshEnergy();
        HandleTargettingUpdated(false);
        combatCanvas.SetActive(true);
    }

    private void CombatFinished()
    {
        combatCanvas.SetActive(false);
    }

    private void Update()
    {

    }

    public void OnActionSelected(Dictionary<EEffectTypes, int> effectList, int slot, int manaCost, int energyCost)
    {
        if (effectList.ContainsKey(EEffectTypes.DamageSingle))
        {
            targetCheck.ChoosingTarget(true);
            savedAction = () => effectsProcessor.ProcessEffect(effectList, enemyTarget);
            savedManaCost = manaCost;
            savedEnergyCost = energyCost;
            savedSlot = slot;
            return;
        }

        PerformAction(() => effectsProcessor.ProcessEffect(effectList), manaCost, energyCost, slot);
    }

    private void HandleTargettingUpdated(bool state)
    {
        foreach (var targetButton in targetConfirms)
        {
            targetButton.SetActive(state);
        }
    }

    private void ChooseTarget(EnemyHealthSystem enemy)
    {
        enemyTarget = enemy;

        PerformAction(savedAction, savedManaCost, savedEnergyCost, savedSlot);
    }

    private void PerformAction(Action action, int manaCost, int energyCost, int slot)
    {
        energy.UpdateEnergy(energyCost);
        OnManaUsed?.Invoke(manaCost);
        OnActionUsed?.Invoke(slot);
        action();
        ClearAction();
    }

    private void ClearAction()
    {
        savedAction = null;
        savedManaCost = 0;
        savedEnergyCost = 0;
        savedSlot = 0;
        targetCheck.ChoosingTarget(false);
    }

    public void EndTurn()
    {
        state = CombatState.EnemyTurn;
    }

    public void StartPlayerTurn()
    {
        state = CombatState.PlayerTurn;
    }

    private void RemoveEnemyFromCombat()
    {
        currentEnemiesInCombat -= 1;

        if (currentEnemiesInCombat > 0) { return; }

        CombatFinished();
        Debug.Log("YOU WIN THIS FIGHT");
    }

    private void SetupCombatScreen(EnemyData enemyData)
    {
        currentCombatGroup = enemyData.EnemyGroup;
        enemyList.Clear();
        enemyList = currentCombatGroup.EnemyGroup;
        currentEnemiesInCombat = enemyList.Count;

        foreach (Image enemyImage in enemySlot)
        {
            enemyImage.sprite = null;
            enemyImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < currentEnemiesInCombat; i++)
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
        EnemyHealthSystem.OnEnemyDeath -= RemoveEnemyFromCombat;
        ObjTarget.OnTargetting -= HandleTargettingUpdated;
        TargetButton.OnTargetChosen -= ChooseTarget;
        energy.OnEnergyUpdated -= HandleEnergyUpdated;
    }
}
