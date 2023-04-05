using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static LTDescr delay;
    public float timeBeforePopUp = 0.1f;
    public bool useTimeDelay = true;

    public string header;
    [Multiline()]
    public string content;

    private void Start()
    {
        LeanTween.reset();
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

    void OnMouseEnter()
    {
        Debug.Log("mouse enter");
        ShowTooltip();
    }

    void OnMouseExit()
    {
        Debug.Log("mouse exit");
        HideTooltip();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    private static void HideTooltip()
    {
        if (delay == null)
        {
            return;
        }
        LeanTween.cancel(delay.uniqueId);

        if (ToolTipSystem.current != null)
        {
            ToolTipSystem.Hide();
        }
    }
}
