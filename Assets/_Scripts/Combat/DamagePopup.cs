using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] protected float _maxLifeDuration = 1;
    [SerializeField] protected TMP_Text _txtScpt;

    [SerializeField] protected float _popupScale = 1.1f;
    [SerializeField] protected float _popupScaleAfter = 0.95f;


    [SerializeField] protected float _yBefore = 1.1f;
    [SerializeField] protected float _yAfter = -1.1f;

    [SerializeField] protected Ease _easeType;
    [SerializeField] protected RectTransform _rect;


    void Start()
    {
        _rect = GetComponent<RectTransform>();
        DoPopAnimation();
    }

    public void SetupTextColor(Color newCol)
    {
        _txtScpt.color = newCol;
    }


    public virtual void DoPopAnimation()
    {
        var sequence = DOTween.Sequence();
        transform.DOShakePosition(_maxLifeDuration * .4f, 10);
        transform.DOScale(_popupScale, _maxLifeDuration * .2f);
        sequence.Append(transform.DOLocalMoveY(_yBefore, _maxLifeDuration * .3f)).SetEase(_easeType);
        sequence.Append(transform.DOLocalMoveY(_yAfter, _maxLifeDuration * .3f)).SetEase(_easeType);
        sequence.Append(transform.DOScale(_popupScaleAfter, _maxLifeDuration * .2f)).SetEase(_easeType);
        sequence.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

}
