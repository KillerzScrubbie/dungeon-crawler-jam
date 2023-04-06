using DG.Tweening;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float doorOpenTime;

    public void OpenDoor()
    {
        transform.DOMove(targetPosition.position, doorOpenTime).SetEase(Ease.InOutSine);
    }
}
