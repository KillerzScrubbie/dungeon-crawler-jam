using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPromptUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDeselectHandler
{
    private bool mouseIsOver = false;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (mouseIsOver) return;

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
