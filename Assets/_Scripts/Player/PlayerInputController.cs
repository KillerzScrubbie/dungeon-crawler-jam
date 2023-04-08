using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static event Action<EMovementTypes> OnQueue;
    public static event Action OnPause;
    public static event Action OnInventoryOpened;

    private PlayerInput playerInputMap;

    private void Awake()
    {
        playerInputMap = new PlayerInput();
    }

    private void Start()
    {
        CombatManager.OnCombatStateChanged += HandleGameOver;

        playerInputMap.Player.MoveForward.performed += _ => Move(EMovementTypes.Forward);
        playerInputMap.Player.MoveBackwards.performed += _ => Move(EMovementTypes.Backward);
        playerInputMap.Player.MoveLeft.performed += _ => Move(EMovementTypes.Left);
        playerInputMap.Player.MoveRight.performed += _ => Move(EMovementTypes.Right);
        playerInputMap.Player.LookLeft.performed += _ => Move(EMovementTypes.TurnLeft);
        playerInputMap.Player.LookRight.performed += _ => Move(EMovementTypes.TurnRight);
        playerInputMap.Player.DimensionJump.performed += _ => Move(EMovementTypes.DimensionJump);

        playerInputMap.Player.Inventory.performed += _ => OpenInventory();

        playerInputMap.UI.Pause.performed += _ => Pause();
    }

    public void DisableMovement()
    {
        playerInputMap.Player.Disable();
    }

    public void EnableMovement()
    {
        playerInputMap.Player.Enable();
    }

    private void OnEnable() => playerInputMap.Enable();
    private void OnDisable() => playerInputMap.Disable();

    private void Move(EMovementTypes type)
    {
        OnQueue?.Invoke(type);
    }

    private void Pause()
    {
        OnPause?.Invoke();
    }

    private void OpenInventory()
    {
        OnInventoryOpened?.Invoke();
    }

    private void HandleGameOver(CombatState state)
    {
        switch (state)
        {
            case CombatState.Dead:
                playerInputMap.Disable();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= HandleGameOver;
    }
}
