using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "New SOnumberPopup", menuName = "My Assets/SOnumberPopup")]
public class SOnumberPopup : ScriptableObject
{
    public Vector3 startPosition;
    public Vector3 finishPosition;

    public float popupScale = 1.1f;
    public float popupScaleAfter = 0.95f;

    public float maxLifeDuration = 1;

    public Ease easeType = Ease.InOutCubic;
    public Color textColor;
}
