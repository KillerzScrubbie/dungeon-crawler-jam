using System.Collections;
using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public event Action OnMove;
    public event Action<bool> OnTurn;

    private PlayerInput playerInputMap;

    private void Awake()
    {
        playerInputMap = new PlayerInput();
    }

    private void Start()
    {
        playerInputMap.Player.Move.performed += _ => MoveForward();
        playerInputMap.Player.LookLeft.performed += _ => Turn(true);
        playerInputMap.Player.LookRight.performed += _ => Turn(false);
    }

    private void OnEnable() => playerInputMap.Enable();
    private void OnDisable() => playerInputMap.Disable();

    private void MoveForward()
    {
        OnMove?.Invoke();
    }

    private void Turn(bool turningLeft)
    {
        OnTurn?.Invoke(turningLeft);
    }
}
