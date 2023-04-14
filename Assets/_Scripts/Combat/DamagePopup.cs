using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] protected RectTransform _rect;
    [SerializeField] protected TMP_Text _txtScpt;

    [Header("move data")]
    [SerializeField] protected float _maxLifeDuration = 1;
    [SerializeField] protected float _popupScale = 1.1f;
    [SerializeField] protected float _popupScaleAfter = 0.95f;
    [SerializeField] protected float _yAfter = -1.1f;
    [SerializeField] protected Ease _easeType;


    void Start()
    {
        _rect = GetComponent<RectTransform>();
        DoPopAnimation();
    }

    public void SetupSO(SOnumberPopup soData)
    {
        _rect.localPosition = soData.startPosition;
        _txtScpt.color = soData.textColor;
        _maxLifeDuration = soData.maxLifeDuration;
        _popupScale = soData.popupScale;
        _popupScaleAfter = soData.popupScaleAfter;
        _yAfter = soData.yAfterMove;
        _easeType = soData.easeType;
    }

    public virtual void DoPopAnimation()
    {
        _rect.localPosition = Vector3.zero;
        _rect.DOScale(_popupScale, _maxLifeDuration * .2f);

        var sequence = DOTween.Sequence();
        sequence.Append(_rect.DOShakePosition(_maxLifeDuration * .2f, 10)).SetEase(_easeType);
        sequence.Append(_rect.DOLocalMoveY(_yAfter, _maxLifeDuration * .6f)).SetEase(_easeType);
        sequence.Append(_rect.DOScale(_popupScaleAfter, _maxLifeDuration * .2f)).SetEase(_easeType);
        sequence.Append(_txtScpt.DOFade(0, _maxLifeDuration * .3f)).SetEase(_easeType);

        sequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
