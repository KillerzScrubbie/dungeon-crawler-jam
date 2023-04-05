using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

// original code learn from: https://www.youtube.com/watch?v=HXFoUGw7eKk

// [ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TMP_Text headerField;
    public TMP_Text contentField;

    public LayoutElement layoutElementObj;
    public RectTransform rectTrans;

    public Vector2 offSet = new Vector2(30, 20);

    // to make it not become bad when it show on center like if not 200 in each edge it will not set the pivot
    public Vector2 customRange;

    private void Awake()
    {
        rectTrans = GetComponent<RectTransform>();
    }

    void smartCustomRange()
    {
        // get box width and height and set range accordingly
        customRange = new Vector2(rectTrans.rect.width, rectTrans.rect.height);
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.SetText(header);
            headerField.gameObject.SetActive(true);
        }

        contentField.SetText(content);
        smartCustomRange();
        DoBoxSize();

    }


    void Update()
    {
        DoUpdatePosNewInput();
    }

    public void DoUpdatePosNewInput()
    {
        Vector2 position = Mouse.current.position.ReadValue();
        DoSetPosSmart(position);
    }


    void DoSetPosSmart(Vector2 position)
    {
        // add own code to make it off set if in between center 
        if (IsBetweenFloat(position.x, customRange.x, Screen.width - customRange.x) && IsBetweenFloat(position.y, customRange.y, Screen.height - customRange.y))
        {
            // set pivot to 0 1 - it will be lower right
            rectTrans.pivot = new Vector2(0, 1);
            // this should alway set after pivot 
            transform.position = position + offSet;
            return;
        }
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTrans.pivot = new Vector2(pivotX, pivotY);
        // this should alway set after pivot 
        // not use offset if it in edge of screen 
        transform.position = position;
    }


    void DoBoxSize()
    {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        layoutElementObj.enabled = Mathf.Max(headerField.preferredWidth, contentField.preferredWidth) >= layoutElementObj.preferredWidth;
    }

    public bool IsBetweenFloat(float testValue, float bound1, float bound2)
    {
        return (testValue >= Mathf.Min(bound1, bound2) && testValue <= Mathf.Max(bound1, bound2));
    }


}
