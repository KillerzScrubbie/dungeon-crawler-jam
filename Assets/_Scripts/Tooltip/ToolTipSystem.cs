using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem current;
    public Tooltip tooltip;
    public static LeanTweenType easeType;

    private void Awake()
    {
        current = this;
    }

    public static void Show(string content, string header = "")
    {
        current.tooltip.DoUpdatePosNewInput();

        popUpAnimation();
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
    }


    public static void popUpAnimation()
    {
        LeanTween.scale(current.tooltip.gameObject, new Vector3(1.02f, 1.02f, 1.02f), .04f).setOnComplete(() =>
        {
            LeanTween.scale(current.tooltip.gameObject, new Vector3(1f, 1f, 1f), .06f).setEase(easeType);
        }).setEase(easeType);
    }



    public static void JustShow()
    {
        current.tooltip.DoUpdatePosNewInput();
        current.tooltip.gameObject.SetActive(true);
    }


    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }


}
