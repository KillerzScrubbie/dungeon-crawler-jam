using System;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static event Action<EMovementTypes> OnQueue;
    public static event Action OnPause;
    public static event Action OnInventoryOpened;

    private PlayerInput playerInputMap;

    private float holdForwardTimer = -1;
    private float holdBackwardTimer = -1;
    private float holdLeftTimer = -1;
    private float holdRightTimer = -1;

    private float holdTimeInterval = 0.25f;

    void OnEnable()
    {
        playerInputMap.Enable();
    }
    void OnDisable()
    {
        playerInputMap.Disable();
    }

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

        playerInputMap.Player.HoldForward.performed += _ => holdForwardTimer = 0;
        playerInputMap.Player.HoldForward.canceled += _ => holdForwardTimer = -1;

        playerInputMap.Player.HoldBackward.performed += _ => holdBackwardTimer = 0;
        playerInputMap.Player.HoldBackward.canceled += _ => holdBackwardTimer = -1;

        playerInputMap.Player.HoldLeft.performed += _ => holdLeftTimer = 0;
        playerInputMap.Player.HoldLeft.canceled += _ => holdLeftTimer = -1;

        playerInputMap.Player.HoldRight.performed += _ => holdRightTimer = 0;
        playerInputMap.Player.HoldRight.canceled += _ => holdRightTimer = -1;

        playerInputMap.Player.Inventory.performed += _ => OpenInventory();

        playerInputMap.UI.Pause.performed += _ => Pause();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        CheckHoldForward(deltaTime);
        CheckHoldBackward(deltaTime);
        CheckHoldLeft(deltaTime);
        CheckHoldRight(deltaTime);
    }

    private void CheckHoldForward(float timeElapsed)
    {
        if (holdForwardTimer < 0) { return; }

        holdForwardTimer += timeElapsed;

        if (holdForwardTimer < holdTimeInterval) { return; }

        holdForwardTimer = 0f;
        Move(EMovementTypes.Forward);
    }

    private void CheckHoldBackward(float timeElapsed)
    {
        if (holdBackwardTimer < 0) { return; }

        holdBackwardTimer += timeElapsed;

        if (holdBackwardTimer < holdTimeInterval) { return; }

        holdBackwardTimer = 0f;
        Move(EMovementTypes.Backward);
    }

    private void CheckHoldLeft(float timeElapsed)
    {
        if (holdLeftTimer < 0) { return; }

        holdLeftTimer += timeElapsed;

        if (holdLeftTimer < holdTimeInterval) { return; }

        holdLeftTimer = 0f;
        Move(EMovementTypes.Left);
    }

    private void CheckHoldRight(float timeElapsed)
    {
        if (holdRightTimer < 0) { return; }

        holdRightTimer += timeElapsed;

        if (holdRightTimer < holdTimeInterval) { return; }

        holdRightTimer = 0f;
        Move(EMovementTypes.Right);
    }

    public void DisableMovement()
    {
        playerInputMap.Player.Disable();
    }

    public void EnableMovement()
    {
        playerInputMap.Enable();
        playerInputMap.Player.Enable();
    }

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
            case CombatState.Victory:
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
