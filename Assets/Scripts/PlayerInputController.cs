using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
// private Vector2 _moveInput;

    private void OnMove(InputValue value)
    {
        if (!value.isPressed) return;

        var _moveInput = value.Get<Vector2>();

        Debug.Log($"{_moveInput}");
    }
}
