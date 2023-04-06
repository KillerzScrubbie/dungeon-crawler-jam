using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private PlayerBaseState currentState;

    private PlayerCombatState playerCombatState = new();
    private PlayerExplorationState playerExplorationState = new();

    public PlayerMovement GetPlayerMovement() => playerMovement;

    // Get States
    public PlayerCombatState GetPlayerCombatState() => playerCombatState;
    public PlayerExplorationState GetPlayerExplorationState() => playerExplorationState;

    private void Start()
    {
        PlayerMovement.OnCombatEntered += SwitchToCombatState;

        currentState = playerExplorationState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    private void SwitchToCombatState()
    {
        SwitchState(playerCombatState);
    }

    public void SwitchState(PlayerBaseState state)
    {
        if (state != currentState) { return; }

        currentState = state;
        state.EnterState(this);
    }

    private void OnDestroy()
    {
        PlayerMovement.OnCombatEntered -= SwitchToCombatState;
    }
}
