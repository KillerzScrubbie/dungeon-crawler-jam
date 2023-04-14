using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCanvas;
    [SerializeField] private ObjEnergy energy;
    [SerializeField] private EffectsProcessor effectsProcessor;
    [SerializeField] private ObjTarget targetCheck;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI endTurnText;
    [SerializeField] private VictoryCombatScreen victoryCanvas;
    [SerializeField] private GameObject gameOverScreen;

    [Space]
    [Header("UI Slots List")]
    [SerializeField] private List<Image> enemySlot;
    [SerializeField] private List<EnemyHealthBar> healthBars;
    [SerializeField] private List<Image> energyIcons;
    [SerializeField] private List<GameObject> targetConfirms;

    public static event Action<int> OnActionUsed;
    public static event Action<int> OnManaUsed;
    public static event Action<CombatState> OnCombatStateChanged;
    public static event Action OnStartNewTurn;

    private ObjEnemyGroup currentCombatGroup;
    private List<ObjEnemy> enemyList = new();
    private List<EnemyCombat> activeEnemies = new();

    public List<EnemyCombat> ActiveEnemies { get { return activeEnemies; } }

    private EnemyCombat enemyTarget;
    private Action savedAction;
    private int savedManaCost;
    private int savedEnergyCost;
    private int savedSlot;

    private int currentEnemiesInCombat = 0;

    private int currentMana = 0;
    private int currentEnergy = 0;

    private CombatState state = CombatState.NotInCombat;

    private void Start()
    {
        Initialize();
        HideCombatCanvas();

        PotionManager.OnEnergyPotionUsed += UseEnergyPotion;
        PlayerMovement.OnCombatEntered += CombatEntered;
        PlayerMana.OnPlayerUpdateMP += SetCurrentMana;
        EnemyCombatState.OnCombatEntered += SetupCombatScreen;
        EnemyHealthSystem.OnEnemyDeath += RemoveEnemyFromCombat;
        ObjTarget.OnTargetting += HandleTargettingUpdated;
        TargetButton.OnTargetChosen += ChooseTarget;
        energy.OnEnergyUpdated += HandleEnergyUpdated;

        PlayerHp.OnPlayerDeath += HandleGameOver;
    }

    private void HandleGameOver()
    {
        UpdateGameState(CombatState.Dead);
        gameOverScreen.SetActive(true);
    }

    private void Initialize()
    {
        HandleTargettingUpdated(false);
        combatCanvas.SetActive(true);
    }

    private void CombatEntered()
    {
        energy.RefreshEnergy();
        Initialize();
        UpdateGameState(CombatState.StartCombat);
    }

    private void CombatFinished()
    {
        UpdateGameState(CombatState.Victory);
    }

    public void UpdateGameState(CombatState newState)
    {
        state = newState;

        switch (newState)
        {
            case CombatState.StartCombat:
                HandleStartCombat();
                break;
            case CombatState.PlayerTurn:
                break;
            case CombatState.EnemyTurn:
                HandleEnemyTurn();
                break;
            case CombatState.Dead:
                break;
            case CombatState.NotInCombat:
                HideCombatCanvas();
                break; ;
            case CombatState.Victory:
                ShowVictoryScreen();
                break;
        }

        OnCombatStateChanged?.Invoke(state);
    }

    private void HandleStartCombat()
    {
        //StartPlayerTurn();
    }

    private void HideCombatCanvas()
    {
        combatCanvas.SetActive(false);
    }

    private void ShowVictoryScreen()
    {
        victoryCanvas.StartTransition();
    }

    private async void HandleEnemyTurn()
    {
        await Task.Delay(1000);

        foreach (var enemy in activeEnemies)
        {
            enemy.PerformAction();
            await Task.Delay(750);
        }

        await Task.Delay(250);
        StartPlayerTurn();
    }

    public void OnActionSelected(Dictionary<EEffectTypes, int> effectList, int slot, int manaCost, int energyCost)
    {
        if (currentMana < manaCost || currentEnergy < energyCost) { return; } // Play SFX if unusuable

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

    private void ChooseTarget(EnemyCombat enemy)
    {
        enemyTarget = enemy;

        PerformAction(savedAction, savedManaCost, savedEnergyCost, savedSlot);
    }

    private void PerformAction(Action action, int manaCost, int energyCost, int slot)
    {
        energy.UseEnergy(energyCost);
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
        endTurnButton.interactable = false;
        endTurnText.color = new Color(0.55f, 0.3f, 0.3f, 1f);
        ClearAction();
        UpdateGameState(CombatState.EnemyTurn);

        AudioManager.instance?.PlayRandomPitch("endTurn", .8f, 1.2f);

    }

    public void StartPlayerTurn()
    {
        if (endTurnButton == null) { return; } // Error when quitting on enemy turn

        endTurnButton.interactable = true;
        endTurnText.color = new Color(0.95f, 0.95f, 0.95f, 95f);
        UpdateGameState(CombatState.PlayerTurn);
        energy.RefreshEnergy();
        OnStartNewTurn?.Invoke();
    }

    private async void RemoveEnemyFromCombat(EnemyCombat enemy)
    {
        await Task.Delay(5);

        activeEnemies.Remove(enemy);

        if (activeEnemies.Count > 0) { return; }

        CombatFinished();
    }

    private void SetupCombatScreen(EnemyData enemyData)
    {
        currentCombatGroup = enemyData.EnemyGroup;
        enemyList.Clear();
        activeEnemies.Clear();
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

            EnemyHealthBar healthBar = healthBars[i];
            healthBar.Setup(enemy.MaxHealth);
            EnemyCombat enemyCombat = healthBar.GetComponent<EnemyCombat>();
            enemyCombat.SetupEnemy(enemy);

            activeEnemies.Add(enemyCombat);
        }
    }

    private void HandleEnergyUpdated(int energy)
    {
        currentEnergy = energy;

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

    private void UseEnergyPotion(int energyGain)
    {
        energy.GainEnergy(energyGain);
    }

    private void SetCurrentMana(int currentMana, int maxMana)
    {
        this.currentMana = currentMana;
    }

    private void OnDestroy()
    {
        PlayerMana.OnPlayerUpdateMP -= SetCurrentMana;
        PotionManager.OnEnergyPotionUsed -= UseEnergyPotion;
        PlayerMovement.OnCombatEntered -= CombatEntered;
        EnemyCombatState.OnCombatEntered -= SetupCombatScreen;
        EnemyHealthSystem.OnEnemyDeath -= RemoveEnemyFromCombat;
        ObjTarget.OnTargetting -= HandleTargettingUpdated;
        TargetButton.OnTargetChosen -= ChooseTarget;
        energy.OnEnergyUpdated -= HandleEnergyUpdated;

        PlayerHp.OnPlayerDeath -= HandleGameOver;
    }
}

public enum CombatState
{
    PlayerTurn,
    EnemyTurn,
    Victory,
    Dead,
    NotInCombat,
    StartCombat,
}
