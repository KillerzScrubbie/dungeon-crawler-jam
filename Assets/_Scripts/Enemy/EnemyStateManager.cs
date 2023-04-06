using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyChaseMovement enemyChaseMovement;

    private EnemyBaseState currentState;
    private EnemyBaseState previousState;

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

        currentState = GetPatrolState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
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

    private void SwitchToPreviousState()
    {
        SwitchState(previousState);
    }

    public void SwitchState(EnemyBaseState state)
    {
        if (currentState == state) return;

        previousState = currentState;
        currentState.LeaveState(this);
        currentState = state;
        state.EnterState(this);
    }

    public void HandlePathingFinished()
    {
        currentState.OnFinishedPath(this);
    }

    // Pool enemies? if yes change to ondisable
    private void OnDestroy()
    {
        enemyMovement.OnChase -= SwitchToChaseState;
        EnemyChaseMovement.OnCombatEntered -= SwitchToCombatState;
        enemyChaseMovement.OnPatrolStarted -= SwitchToPatrolState;
    }
}
