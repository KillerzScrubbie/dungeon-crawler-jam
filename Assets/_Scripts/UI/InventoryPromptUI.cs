using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPromptUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    private bool mouseIsOver = false;
    private bool isTargetting = false;

    public static event Action OnPromptExit;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
        ObjTarget.OnTargetting += ChangeTargettingStatus;
    }

    private void OnDisable()
    {
        ObjTarget.OnTargetting -= ChangeTargettingStatus;
    }

    private void ChangeTargettingStatus(bool state)
    {
        isTargetting = state;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (mouseIsOver) return;

        if (isTargetting) { return; }

        OnPromptExit?.Invoke();
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
