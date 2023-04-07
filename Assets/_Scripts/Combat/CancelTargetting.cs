using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelTargetting : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;
    [SerializeField] private ObjTarget target;

    private void Start()
    {
        ObjTarget.OnTargetting += SetButtonVisible;
        SetButtonVisible(false);
    }

    private void SetButtonVisible(bool state)
    {
        gameObject.SetActive(state);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        target.ChoosingTarget(false);
    }

    private void OnDestroy()
    {
        ObjTarget.OnTargetting -= SetButtonVisible;
    }
}
