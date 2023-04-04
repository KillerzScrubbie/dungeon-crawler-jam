using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private ObjItems item;

    public ObjItems Item { get { return item; } set { item = value; } }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(Item.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(Item.Action1);
    }
}
