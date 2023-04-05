using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseClickDetector : MonoBehaviour
{
    [SerializeField] private float rayDistance = 1.5f;
    [SerializeField] private LayerMask lootMask;

    private Camera mainCamera;

    public static event Action<ChestLoot, List<ObjItems>, List<ObjPotions>> OnChestClicked;

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

        if (IsMouseOverUIObject()) { return; }

        if (!hit.transform.TryGetComponent(out ChestLoot loot)) { return; }

        loot.OpenChest();
        OnChestClicked?.Invoke(loot, loot.GetItems(), loot.GetPotions());
    }

    private bool IsMouseOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
