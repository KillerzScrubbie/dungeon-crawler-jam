using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    private EnemyBaseState currentState;

    private EnemyCombatState enemyCombatState = new();
    private EnemyChaseState enemyChaseState = new();
    private EnemyPatrolState enemyPatrolState = new();
    private EnemyPausedState enemyPausedState = new();

    public EnemyCombatState GetCombatState() => enemyCombatState;
    public EnemyChaseState GetChaseState() => enemyChaseState;
    public EnemyPatrolState GetPatrolState() => enemyPatrolState;
    public EnemyPausedState GetPausedState() => enemyPausedState;

    private void Start()
    {
        currentState = GetPausedState();
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
