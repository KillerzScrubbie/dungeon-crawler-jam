using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputController))]
public class PlayerMovement : MonoBehaviour
{
     private PlayerInputController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerInputController>();
    }

    private void Start()
    {
        controller.OnMove += MoveForward;
        controller.OnTurn += Turn;
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
