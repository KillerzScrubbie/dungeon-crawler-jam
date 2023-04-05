using UnityEngine;
using UnityEngine.UI;

public class ToolTipSystem : MonoBehaviour
{
    /* 
    เรื่องการ fade UI ยากไปเลยยังไม่ได้ใส่เข้ามา
    */

    public static ToolTipSystem current;
    public Tooltip tooltip;
    public static LeanTweenType easeType;

    private void Awake() 
    {
        current = this;
    }
    
    public static void Show(string content, string header="")
    {
        // choose one below

        // current.tooltip.DoUpdatePosNewInput();
        current.tooltip.DoUpdatePosOldInput();

        popUpAnimation();

        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
    }


    public static void popUpAnimation()
    {
        //Animation!
        LeanTween.scale(current.tooltip.gameObject, new Vector3(1.02f, 1.02f, 1.02f), .04f).setOnComplete(() =>
        {
            LeanTween.scale(current.tooltip.gameObject, new Vector3(1f, 1f, 1f), .06f).setEase(easeType);
        }).setEase(easeType);
    }



    public static void JustShow()
    {
        // choose one below

        // current.tooltip.DoUpdatePosNewInput();
        current.tooltip.DoUpdatePosOldInput();


        current.tooltip.gameObject.SetActive(true);
    }


    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }


}
