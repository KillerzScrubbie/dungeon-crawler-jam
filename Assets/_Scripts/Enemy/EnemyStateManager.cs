using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyChaseMovement chaseMovement;

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
    public EnemyChaseMovement GetEnemyChaseMovement() => chaseMovement;

    private void Start()
    {
        enemyMovement.OnChase += SwitchToChaseState;
        EnemyChaseMovement.OnCombatEntered += SwitchToCombatState;

        currentState = GetPatrolState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void SwitchToCombatState(EnemyChaseMovement chaseMovement)
    {
        if (this.chaseMovement != chaseMovement)
        {
            SwitchToPausedState();
            return;
        }

        SwitchState(enemyCombatState);
    }

    private void SwitchToChaseState()
    {
        // if (this.enemyMovement != enemyMovement) { return; }

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

        previousState = currentState;
        currentState.OnLeaveState(this);
        currentState = state;
        state.EnterState(this);
    }

    private void OnDestroy()
    {
        enemyMovement.OnChase -= SwitchToChaseState;
        EnemyChaseMovement.OnCombatEntered -= SwitchToCombatState;
    }
}
