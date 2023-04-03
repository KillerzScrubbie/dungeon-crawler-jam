using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField] private Transform compassPivot;

    private void Start()
    {
        PlayerMovement.OnTurned += RotateCompass;
    }

    private void RotateCompass(float angle)
    {
        compassPivot.eulerAngles = new Vector3(0f, 0f, angle);
    }

    private void OnDestroy()
    {
        PlayerMovement.OnTurned -= RotateCompass;
    }
}
