using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static LTDescr delay;
    public float timeBeforePopUp=0.1f;
    public bool useTimeDelay=true;

    public string header;
    [Multiline()]
    public string content;

    private void Start() 
    {
        // if not reset it some time will not work
        LeanTween.reset();
    }


    // show word in this trigger
    public void OnPointerEnter(PointerEventData eventData)
    {
        /* 
            Normal without delay 
            ToolTipSystem.Show(content, header);

            If bug happen try this one below with help of Debug.log
            delay = LeanTween.delayedCall(this.gameObject, 0.5f, DoShowFunction);

        */
        if(!useTimeDelay)
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


    public void OnPointerExit(PointerEventData eventData)
    {
        if(delay == null)
        {
            return;
        }
        LeanTween.cancel(delay.uniqueId);

        if(ToolTipSystem.current != null)
        {
            /* 
            ------prevent error when game close and tool tip still show------
            คือถ้าเอาเมาส์ชี้ค้างไว้แล้วปิดเลย โดยไม่มีอันนี้ มันจะ error แดง
            */

            ToolTipSystem.Hide();
        }

    }
}
