using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseClickDetector : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1.5f;
    [SerializeField] private LayerMask lootMask;

    private Camera mainCamera;

    public static event Action<ObjItems> OnChestClicked;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

        ProcessMouseClick();
    }

    private void ProcessMouseClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, rayDistance, lootMask)) { return; }

        if (!hit.transform.TryGetComponent(out ChestLoot loot)) { return; }

        OnChestClicked?.Invoke(loot.GetItem());
    }
}
