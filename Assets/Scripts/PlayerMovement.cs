using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private bool smoothTransition = false;
    [SerializeField] private float transitionSpeed = 10f;
    [SerializeField] private float transitionRotationSpeed = 500f;
 
    private PlayerInputController controller;

    private Vector3 targetGridPos;
    private Vector3 prevTargetGridPos;
    private Vector3 nextTargetGridPos;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        controller.OnMove += MoveForward;
        controller.OnTurn += Turn;

        targetGridPos = Vector3Int.RoundToInt(transform.position);
    }

    private void MoveForward()
    {
        transform.position += Vector3.forward;
    }

    private void Turn(bool turningLeft)
    {

    }

    private void OnDestroy()
    {
        controller.OnMove -= MoveForward;
        controller.OnTurn -= Turn;
    }
}
