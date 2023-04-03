using System.Collections;
using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public event Action<EMovementTypes> OnQueue;

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
}
