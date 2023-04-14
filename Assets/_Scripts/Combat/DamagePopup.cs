using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] TMP_Text _txtScpt;

    [Header("move data")]
    [SerializeField] float _maxLifeDuration = 1;
    [SerializeField] float _popupScale = 1.1f;
    [SerializeField] float _popupScaleAfter = 0.95f;
    [SerializeField] Vector3 _finishPosition;
    [SerializeField] Ease _easeType;


    void Start()
    {
        DoPopAnimation();
    }

    public void SetupSO(SOnumberPopup soData)
    {
        _rect = GetComponent<RectTransform>();
        _rect.localPosition = soData.startPosition;
        _txtScpt.color = soData.textColor;
        _maxLifeDuration = soData.maxLifeDuration;
        _popupScale = soData.popupScale;
        _popupScaleAfter = soData.popupScaleAfter;
        _finishPosition = soData.finishPosition;
        _easeType = soData.easeType;
    }

    public virtual void DoPopAnimation()
    {
        _rect.localPosition = Vector3.zero;
        _rect.DOScale(_popupScale, _maxLifeDuration * .2f);

        var sequence = DOTween.Sequence();
        sequence.Append(_rect.DOShakePosition(_maxLifeDuration * .2f, 10)).SetEase(_easeType);
        sequence.Append(_rect.DOLocalMove(_finishPosition, _maxLifeDuration * .6f)).SetEase(_easeType);
        sequence.Append(_rect.DOScale(_popupScaleAfter, _maxLifeDuration * .2f)).SetEase(_easeType);
        sequence.Append(_txtScpt.DOFade(0, _maxLifeDuration * .3f)).SetEase(_easeType);

        sequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
