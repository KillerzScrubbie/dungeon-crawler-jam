using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private bool smoothTransition = false;
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float rotationDuration = 0.15f;

    [Space]
    [Header("Player Physics")]
    [SerializeField] private LayerMask obstacleLayers;

    private Queue<EMovementTypes> inputQueue = new Queue<EMovementTypes>();

    private PlayerInputController controller;

    private Vector3 targetGridPos;
    private Vector3 prevTargetGridPos; // Failsafe if player somehow glitches out or fall off the map.

    private bool isMoving = false;

    private readonly float gridSize = 1f;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        controller.OnQueue += QueueMovement;

        SetGridPos(transform.position);
    }

    private void QueueMovement(EMovementTypes eMovementTypes)
    {
        if (inputQueue.Count > 1) { return; }

        inputQueue.Enqueue(eMovementTypes);
    }

    private void Update()
    {
        if (inputQueue.Count == 0) { return; }

        if (isMoving) { return; }

        ProcessMovement();
    }

    private void ProcessMovement()
    {
        EMovementTypes movementType = inputQueue.Dequeue();
        LockMovement(true);

        switch (movementType)
        {
            case EMovementTypes.Forward:
                Move(transform.forward);
                break;
            case EMovementTypes.Backward:
                Move(-transform.forward);
                break;
            case EMovementTypes.Left:
                Move(-transform.right);
                break;
            case EMovementTypes.Right:
                Move(transform.right);
                break;
            case EMovementTypes.TurnLeft:
                Turn(true);
                break;
            case EMovementTypes.TurnRight:
                Turn(false);
                break;
        }
    }

    private void Move(Vector3 direction)
    {
        if (CheckForCollision(direction)) 
        {
            LockMovement(false);
            return; 
        }

        float duration = smoothTransition ? moveDuration : 0f;

        SetGridPos(transform.position + direction);
        transform.DOMove(targetGridPos, duration).OnComplete(() => LockMovement(false));
    }

    private bool CheckForCollision(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, gridSize, obstacleLayers);
    }

    private void Turn(bool turningLeft)
    {
        Vector3 currentRotation = transform.eulerAngles;
        float duration = smoothTransition ? rotationDuration : 0f;

        float turnValue;

        switch (turningLeft)
        {
            case true:
                turnValue = -90;
                break;
            case false:
                turnValue = 90;
                break;
        }

        transform.DORotate(new Vector3(currentRotation.x, currentRotation.y + turnValue, currentRotation.z), duration).OnComplete(() => LockMovement(false));
    }

    private void LockMovement(bool state) => isMoving = state;

    private void SetGridPos(Vector3 position)
    {
        targetGridPos = new Vector3(Mathf.RoundToInt(position.x), position.y, Mathf.RoundToInt(position.z));
    }

    private void OnDestroy()
    {
        controller.OnQueue -= QueueMovement;
    }
}
