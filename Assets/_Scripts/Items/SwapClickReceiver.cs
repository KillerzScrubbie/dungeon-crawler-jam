using UnityEngine;
using UnityEngine.EventSystems;

public class SwapClickReceiver : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ItemData itemData;

    public void OnPointerClick(PointerEventData eventData)
    {
        itemData.Swap();
    }
}
