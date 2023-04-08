using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private bool smoothTransition = false;
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float rotationDuration = 0.15f;
    [SerializeField] private int dimensionOffset = 200;
    [SerializeField] private float jumpCooldownTime = 2;

    [Space]
    [Header("Player Physics")]
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask stairsLayers;
    [SerializeField] private LayerMask stairsDownLayers;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private BoxCollider groundCheckCollider;

    public static event Action OnCombatEntered;
    public static event Action OnDimensionJumpBlocked;
    public static event Action OnDimensionJumpSuccess;
    public static event Action<float> OnTurned;

    private Queue<EMovementTypes> inputQueue = new();

    private PlayerInputController controller;

    private Vector3 targetGridPos;
    private Vector3 prevTargetGridPos; // Failsafe if player somehow glitches out or fall off the map.
    private RaycastHit hit;

    private bool isMoving = false;
    private bool isMainDimension = true;

    private readonly float gridSize = 1f;

    private float playerOffset = 0f;
    private bool canDimensionJump = true;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        PlayerInputController.OnQueue += QueueMovement;
        PlayerCombatState.OnPlayerCombatState += DisableMovement;
        PlayerExplorationState.OnPlayerExplorationState += EnableMovement;

        playerOffset = transform.position.y;
        SetGridPos(transform.position);
    }

    private void DisableMovement()
    {
        controller.DisableMovement();
    }

    private void EnableMovement()
    {
        controller.EnableMovement();
    }

    private void QueueMovement(EMovementTypes eMovementTypes)
    {
        if (inputQueue.Count > 1) { return; }

        inputQueue.Enqueue(eMovementTypes);
    }

    public void UpdateMovement()
    {
        CheckForEnemy();
        ProcessMovement();
    }

    private void CheckForEnemy()
    {
        if (isMoving) { return; }

        Collider[] enemies = Physics.OverlapSphere(transform.position, 0.2f, enemyLayers);

        if (enemies.Length == 0) { return; }

        enemies[0].GetComponent<EnemyCombatDetector>().EnterCombat();
        OnCombatEntered?.Invoke();
    }

    private void ProcessMovement()
    {
        if (inputQueue.Count == 0) { return; }

        if (isMoving) { return; }

        EMovementTypes movementType = inputQueue.Dequeue();
        LockMovement(true);
        AudioManager.instance?.PlayOneRandomPitch("walk", 0.85f, 1.2f);

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
            case EMovementTypes.DimensionJump:
                DimensionJump();
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

        if (CheckForStairs(direction, stairsLayers))
        {
            SetGridPos(transform.position + direction + Vector3.up);
        }
        else if (CheckForStairs(direction, stairsDownLayers))
        {
            SetGridPos(transform.position + direction + Vector3.down);
        }
        else
        {
            SetGridPos(transform.position + direction);
        }

        transform.DOMove(targetGridPos, duration).OnComplete(() => TryFalling(duration));
    }

    private void TryFalling(float duration)
    {
        if (!CheckGround(targetGridPos, out hit))
        {
            float groundHeight = Mathf.RoundToInt(hit.point.y);
            float fallDuration = Mathf.RoundToInt(targetGridPos.y - groundHeight) * duration * 0.5f;

            targetGridPos.y = Mathf.RoundToInt(hit.point.y) + playerOffset;
            transform.DOMove(targetGridPos, fallDuration).SetEase(Ease.InQuad).OnComplete(() => LockMovement(false));
        }
        else
        {
            LockMovement(false);
        }
    }

    private bool CheckForCollision(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, gridSize, obstacleLayers);
    }

    private bool CheckForCollisionInDimension(Vector3 position)
    {
        return Physics.OverlapSphere(position, 0f, groundLayers).Length != 0;
    }

    private bool CheckForStairs(Vector3 direction, LayerMask layers)
    {
        return Physics.Raycast(transform.position, direction, gridSize, layers);
    }

    private bool CheckGround(Vector3 targetGridPosition, out RaycastHit ground)
    {
        bool findGroundInRange;

        if (Physics.BoxCast(targetGridPosition, groundCheckCollider.bounds.extents / 2, Vector3.down, out ground, Quaternion.identity, 0.5f, groundLayers))
        {
            findGroundInRange = true;
        }
        else
        {
            Physics.BoxCast(targetGridPosition, groundCheckCollider.bounds.extents / 2, Vector3.down, out ground, Quaternion.identity, Mathf.Infinity, groundLayers);
            findGroundInRange = false;
        }

        return findGroundInRange;
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

        float yRotation = currentRotation.y + turnValue;

        OnTurned?.Invoke(-yRotation);
        transform.DORotate(new Vector3(currentRotation.x, yRotation, currentRotation.z), duration).OnComplete(() => LockMovement(false));
    }

    private void DimensionJump()
    {
        if (!canDimensionJump)
        {
            LockMovement(false);
            return;
        }

        Vector3 targetGridDimensionPos = isMainDimension ?
            transform.position + Vector3.down * dimensionOffset :
            transform.position + Vector3.up * dimensionOffset;

        if (CheckForCollisionInDimension(targetGridDimensionPos))
        {
            LockMovement(false);
            OnDimensionJumpBlocked?.Invoke();
            AudioManager.instance?.Play("teleportFail");
            return;
        }

        float duration = smoothTransition ? moveDuration : 0f;
        isMainDimension = !isMainDimension;
        SetGridPos(targetGridDimensionPos);

        transform.DOMove(targetGridPos, 0f).OnComplete(() => TryFalling(duration));
        OnDimensionJumpSuccess?.Invoke();
        StartCoroutine(StartDimentionalJumpCooldown());

        AudioManager.instance?.Play("teleportSuccess");

    }

    IEnumerator StartDimentionalJumpCooldown()
    {
        canDimensionJump = false;
        yield return new WaitForSeconds(jumpCooldownTime);
        canDimensionJump = true;
    }

    private void LockMovement(bool state) => isMoving = state;

    private void SetGridPos(Vector3 position)
    {
        targetGridPos = new Vector3(Mathf.RoundToInt(position.x), position.y, Mathf.RoundToInt(position.z));
    }

    public void SetSmoothMovement(bool newMovementType)
    {
        smoothTransition = newMovementType;
    }

    private void OnDestroy()
    {
        PlayerInputController.OnQueue -= QueueMovement;
        PlayerCombatState.OnPlayerCombatState -= DisableMovement;
        PlayerExplorationState.OnPlayerExplorationState -= EnableMovement;
    }
}
