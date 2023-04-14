using DG.Tweening;
using UnityEngine;

public class DamagePopupEnemy : DamagePopup
{
    public override void DoPopAnimation()
    {
        _rect.localPosition = Vector3.zero;

        _rect.DOScale(_popupScale, _maxLifeDuration * .2f);
        // _rect.DOLocalMoveY(_yAfter, _maxLifeDuration * .3f);

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
