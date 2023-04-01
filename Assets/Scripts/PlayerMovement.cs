using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool smoothTransition = false;
    [SerializeField] private float moveDuration = 0.15f;
    [SerializeField] private float rotationDuration = 0.15f;
 
    private PlayerInputController controller;

    private Vector3 targetGridPos;
    private Vector3 prevTargetGridPos;
    private Vector3 nextTargetGridPos;

    private bool isTurning = false;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        controller.OnMove += MoveForward;
        controller.OnTurn += Turn;

        SetGridPos(transform.position);
    }

    private void MoveForward()
    {
        float duration = smoothTransition ? moveDuration : 0f;

        SetGridPos(transform.position + transform.forward);
        transform.DOMove(targetGridPos, duration);
    }

    private void Turn(bool turningLeft)
    {
        if (isTurning) { return; }

        LockTurning(true);

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

        transform.DORotate(new Vector3(currentRotation.x, currentRotation.y + turnValue, currentRotation.z), duration).OnComplete(() => LockTurning(false));
    }

    private void LockTurning(bool state) => isTurning = state;

    private void SetGridPos(Vector3 position)
    {
        targetGridPos = new Vector3(Mathf.RoundToInt(position.x), position.y, Mathf.RoundToInt(position.z));
    }

    private void OnDestroy()
    {
        controller.OnMove -= MoveForward;
        controller.OnTurn -= Turn;
    }
}
