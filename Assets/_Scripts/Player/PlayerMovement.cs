using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UIElements;

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
    public static event Action OnBossEntered;
    public static event Action OnDimensionJumpBlocked;
    public static event Action OnDimensionJumpOnCooldown;
    public static event Action<float> OnDimensionJumpSuccess;
    public static event Action<float> OnTurned;
    public static event Action<bool> OnDimensionJumpChecked;
    public static event Action<bool> OnDimensionJumpIsMainDimension;

    private Queue<EMovementTypes> inputQueue = new();

    private PlayerInputController controller;

    private Vector3 targetGridPos;
    private Vector3 targetGridDimensionPos;
    private RaycastHit hit;

    private bool isMoving = false;
    private bool isMainDimension = true;
    private bool isTeleporting = false;

    private readonly float gridSize = 1f;

    private float playerOffset = 0f;
    private bool canDimensionJump = true;
    private bool isDimensionTargetBlocked = true;

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

        CheckForCollisionInDimension();
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

        if (isTeleporting) { return; }

        Collider[] enemies = Physics.OverlapSphere(transform.position, 0.2f, enemyLayers);

        if (enemies.Length == 0) { return; }

        bool isBoss = enemies[0].GetComponent<EnemyCombatDetector>().EnterCombat();
        OnCombatEntered?.Invoke();

        if (!isBoss) { return; }
        OnBossEntered?.Invoke();
    }

    private void ProcessMovement()
    {
        if (inputQueue.Count == 0) { return; }

        if (isMoving) { return; }

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

        transform.DOMove(targetGridPos, duration).OnComplete(() =>
        {
            TryFalling(duration);
            AudioManager.instance?.PlayOneShot("walk");
        }
        );
    }

    private void TryFalling(float duration)
    {
        if (!CheckGround(targetGridPos, out hit))
        {
            float groundHeight = Mathf.RoundToInt(hit.point.y);
            float fallDuration = Mathf.RoundToInt(targetGridPos.y - groundHeight) * duration * 0.5f;

            targetGridPos.y = Mathf.RoundToInt(hit.point.y) + playerOffset;
            transform.DOMove(targetGridPos, fallDuration).SetEase(Ease.InQuad).OnComplete(() =>
            {
                CheckForCollisionInDimension();
                LockMovement(false);
                AudioManager.instance?.PlayOneShot("fall");
            }
            );
        }
        else
        {
            CheckForCollisionInDimension();
            LockMovement(false);
        }
    }

    private bool CheckForCollision(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, gridSize, obstacleLayers);
    }

    private bool CheckForCollisionInDimension()
    {
        targetGridDimensionPos = isMainDimension ?
            transform.position + Vector3.down * dimensionOffset :
            transform.position + Vector3.up * dimensionOffset;

        isDimensionTargetBlocked = Physics.OverlapSphere(targetGridDimensionPos, 0f, groundLayers).Length != 0;
        OnDimensionJumpChecked?.Invoke(isDimensionTargetBlocked);

        return !isDimensionTargetBlocked;
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
        transform.DORotate(new Vector3(currentRotation.x, yRotation, currentRotation.z), duration).OnComplete(() =>
        {
            LockMovement(false);
        });
    }

    private void DimensionJump()
    {
        if (!canDimensionJump)
        {
            LockMovement(false);
            OnDimensionJumpOnCooldown?.Invoke();
            AudioManager.instance?.Play("teleportFail");
            return;
        }

        /*targetGridDimensionPos = isMainDimension ?
            transform.position + Vector3.down * dimensionOffset :
            transform.position + Vector3.up * dimensionOffset;*/

        if (isDimensionTargetBlocked)
        {
            LockMovement(false);
            OnDimensionJumpBlocked?.Invoke();
            AudioManager.instance?.Play("teleportFail");
            return;
        }

        isTeleporting = true;
        float duration = smoothTransition ? moveDuration : 0f;
        isMainDimension = !isMainDimension;
        OnDimensionJumpIsMainDimension?.Invoke(isMainDimension);
        SetGridPos(targetGridDimensionPos);

        transform.DOMove(targetGridPos, 0f).OnComplete(() => OnSuccessfulTeleport(duration));
        OnDimensionJumpSuccess?.Invoke(jumpCooldownTime);
        StartCoroutine(StartDimensionalJumpCooldown());

        AudioManager.instance?.Play("teleportSuccess");
    }

    private void OnSuccessfulTeleport(float duration)
    {
        TryFalling(duration);
        isTeleporting = false;
    }

    IEnumerator StartDimensionalJumpCooldown()
    {
        canDimensionJump = false;
        yield return new WaitForSeconds(jumpCooldownTime);
        canDimensionJump = true;
    }

    private void LockMovement(bool state) => isMoving = state;

    private void SetGridPos(Vector3 position)
    {
        targetGridPos = new Vector3(Mathf.RoundToInt(position.x), Mathf.FloorToInt(position.y) + playerOffset, Mathf.RoundToInt(position.z));
    }

    public void SetSmoothMovement(bool newMovementType)
    {
        smoothTransition = newMovementType;
    }

    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
        PlayerInputController.OnQueue -= QueueMovement;
        PlayerCombatState.OnPlayerCombatState -= DisableMovement;
        PlayerExplorationState.OnPlayerExplorationState -= EnableMovement;
    }
}
