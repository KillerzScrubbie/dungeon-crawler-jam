using DG.Tweening;

public class DamagePopupEnemy : DamagePopup
{
    public override void DoPopAnimation()
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
