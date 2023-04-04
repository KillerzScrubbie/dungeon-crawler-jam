using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public event Action<EMovementTypes> OnQueue;
    public static event Action OnPause;
    public static event Action OnInventoryOpened;

    private PlayerInput playerInputMap;

    private void Awake()
    {
        playerInputMap = new PlayerInput();
    }

    private void Start()
    {
        playerInputMap.Player.MoveForward.performed += _ => Move(EMovementTypes.Forward);
        playerInputMap.Player.MoveBackwards.performed += _ => Move(EMovementTypes.Backward);
        playerInputMap.Player.MoveLeft.performed += _ => Move(EMovementTypes.Left);
        playerInputMap.Player.MoveRight.performed += _ => Move(EMovementTypes.Right);
        playerInputMap.Player.LookLeft.performed += _ => Move(EMovementTypes.TurnLeft);
        playerInputMap.Player.LookRight.performed += _ => Move(EMovementTypes.TurnRight);
        playerInputMap.Player.DimensionJump.performed += _ => Move(EMovementTypes.DimensionJump);

        playerInputMap.Inventory.Inventory.performed += _ => OpenInventory();

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

    public void SetInventoryActive(bool state)
    {
        switch (state)
        {
            case false:
                playerInputMap.Inventory.Inventory.Disable();
                break;
            case true:
                playerInputMap.Inventory.Inventory.Enable();
                break;
        }
    }
}
