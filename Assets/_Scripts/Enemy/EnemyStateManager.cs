using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyChaseMovement enemyChaseMovement;

    private EnemyBaseState currentState;

    private EnemyCombatState enemyCombatState = new();
    private EnemyChaseState enemyChaseState = new();
    private EnemyPatrolState enemyPatrolState = new();
    private EnemyPausedState enemyPausedState = new();

    public EnemyCombatState GetCombatState() => enemyCombatState;
    public EnemyChaseState GetChaseState() => enemyChaseState;
    public EnemyPatrolState GetPatrolState() => enemyPatrolState;
    public EnemyPausedState GetPausedState() => enemyPausedState;

    public EnemyMovement GetEnemyMovement() => enemyMovement;
    public EnemyChaseMovement GetEnemyChaseMovement() => enemyChaseMovement;

    public EnemyData EnemyData => enemyData;

    private void Start()
    {
        enemyMovement.OnChase += SwitchToChaseState;
        EnemyChaseMovement.OnCombatEntered += SwitchToCombatState;
        enemyChaseMovement.OnPatrolStarted += SwitchToPatrolState;

        CombatManager.OnCombatStateChanged += HandleStateChanged;

        currentState = GetPatrolState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void HandleStateChanged(CombatState state)
    {
        switch (state)
        {
            case CombatState.NotInCombat:
                SwitchToPatrolState();
                break;
            default:
                break;
        }
    }

    private void SwitchToCombatState(EnemyChaseMovement enemyChaseMovement)
    {
        if (this.enemyChaseMovement != enemyChaseMovement)
        {
            SwitchToPausedState();
            return;
        }

        SwitchState(enemyCombatState);
    }

    private void SwitchToChaseState()
    {
        SwitchState(enemyChaseState);
    }

    private void SwitchToPatrolState()
    {
        SwitchState(enemyPatrolState);
    }

    private void SwitchToPausedState()
    {
        SwitchState(enemyPausedState);
    }

    public void SwitchState(EnemyBaseState state)
    {
        if (currentState == state) return;

        currentState.LeaveState(this);
        currentState = state;
        state.EnterState(this);
    }

    public void HandlePathingFinished()
    {
        currentState.OnFinishedPath(this);
    }

    public void Kill()
    {
        Destroy(enemyData.gameObject);
    }

    // Pool enemies? if yes change to ondisable
    private void OnDestroy()
    {
        enemyMovement.OnChase -= SwitchToChaseState;
        EnemyChaseMovement.OnCombatEntered -= SwitchToCombatState;
        enemyChaseMovement.OnPatrolStarted -= SwitchToPatrolState;

        CombatManager.OnCombatStateChanged -= HandleStateChanged;
    }
}
