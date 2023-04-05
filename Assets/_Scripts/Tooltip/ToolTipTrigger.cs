using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static LTDescr delay;
    public float timeBeforePopUp = 0.1f;
    public bool useTimeDelay = true;

    public string header;
    [Multiline()]
    public string content;

    [SerializeField] private float rayDistance = 1.5f;
    [SerializeField] private LayerMask lootMask;
    private Camera mainCamera;

    private void Start()
    {
        LeanTween.reset();
        mainCamera = Camera.main;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip();
    }

    private void ShowTooltip()
    {
        if (!useTimeDelay)
        {
            ToolTipSystem.Show(content, header);
            return;
        }

        delay = LeanTween.delayedCall(timeBeforePopUp, DoShowFunction);
    }

    public void DoShowFunction()
    {
        ToolTipSystem.Show(content, header);
    }

    void RayDistanceCheck()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, rayDistance, lootMask)) { return; }
    }

    void OnMouseEnter()
    {

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, rayDistance, lootMask)) { return; }
        ShowTooltip();
    }

    void OnMouseExit()
    {
        HideTooltip();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private static void HideTooltip()
    {

        if (ToolTipSystem.current != null)
        {
            ToolTipSystem.Hide();
        }

        if (delay == null)
        {
            return;
        }
        LeanTween.cancel(delay.uniqueId);

    }
}
