using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerHpUiDisplay : MonoBehaviour
{
    [SerializeField] Image _hpSlider;
    [SerializeField] TMP_Text _hpNow;
    [SerializeField] TMP_Text _hpMax;

    Color _startColor;

    [SerializeField] Color _takeDmgColor;
    [SerializeField] float _animationDuration = 0.2f;
    [SerializeField] Ease _easeType;

    void OnEnable()
    {
        PlayerHp.OnPlayerUpdateHp += UpdatePlayerHp;
    }
    void OnDisable()
    {
        PlayerHp.OnPlayerUpdateHp -= UpdatePlayerHp;
    }

    void Awake()
    {
        _startColor = _hpSlider.color;
    }

    private void UpdatePlayerHp(int nowHp, int maxHp)
    {
        float hpPercent = (float)nowHp / maxHp;

        AnimateHpSlider(hpPercent);

        _hpNow.SetText(nowHp.ToString());
        _hpMax.SetText($"/{maxHp.ToString()}");

    }

    private void AnimateHpSlider(float hpPercent)
    {
        _hpSlider.DOFillAmount(hpPercent, _animationDuration).SetEase(_easeType);

        var sequence = DOTween.Sequence();
        sequence.Append(_hpSlider.DOColor(_takeDmgColor, _animationDuration).SetEase(_easeType));
        sequence.Append(_hpSlider.DOColor(_startColor, _animationDuration).SetEase(_easeType));

    }
}
