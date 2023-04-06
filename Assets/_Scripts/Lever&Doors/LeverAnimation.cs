using DG.Tweening;
using UnityEngine;

public class LeverAnimation : MonoBehaviour
{
    [SerializeField] private float leverFlipTime = 1f;

    private Vector3 endLeverPosition = new Vector3(65f, 0f, 0f);

    public void FlipLever()
    {
        transform.DORotate(endLeverPosition, leverFlipTime).SetEase(Ease.OutExpo);
    }
}
