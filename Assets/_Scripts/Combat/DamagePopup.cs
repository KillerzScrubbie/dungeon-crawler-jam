using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] float _maxLifeDuration = 1;

    [SerializeField] TMP_Text _txtScpt;
    [SerializeField] Color _txtCol;

    [SerializeField] float _popupScale = 1.1f;
    [SerializeField] float _popupScaleAfter = 0.95f;

    [SerializeField] float _yBefore = 1.1f;
    [SerializeField] float _yAfter = -1.1f;

    [SerializeField] Ease _easeType;

    void Start()
    {

        var sequence = DOTween.Sequence();
        transform.DOShakePosition(_maxLifeDuration * .4f, 20);
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