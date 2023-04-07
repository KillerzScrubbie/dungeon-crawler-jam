using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChoosingTargetCheck", menuName = "Create Target Instance")]
public class ObjTarget : ScriptableObject
{
    [SerializeField] private bool isTargetting = false;

    public static event Action<bool> OnTargetting;

    public void ChoosingTarget(bool state)
    {
        isTargetting = state;
        OnTargetting?.Invoke(isTargetting);
    }
}
