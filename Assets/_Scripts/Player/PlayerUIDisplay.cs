using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerUIDisplay : MonoBehaviour
{
    [SerializeField] Image _hpSlider;
    [SerializeField] TMP_Text _hpNow;
    [SerializeField] TMP_Text _hpMax;

    [SerializeField] GameObject _mpGroupPanel;
    [SerializeField] Image _mpSlider;
    [SerializeField] TMP_Text _mpNow;
    [SerializeField] TMP_Text _mpMax;

    Color _startHPColor;
    Color _startMPColor;

    [SerializeField] Color _takeDmgColor;
    [SerializeField] Color _takeMPColor;

    [SerializeField] float _animationDuration = 0.2f;
    [SerializeField] Ease _easeType;

    void OnEnable()
    {
        PlayerHp.OnPlayerUpdateHp += UpdatePlayerHp;
        PlayerMana.OnPlayerUpdateMP += UpdatePlayerMp;
        PlayerBlockUI.OnPlayerHpColorUpdate += ChangeHpColor;
    }

    void OnDisable()
    {
        PlayerHp.OnPlayerUpdateHp -= UpdatePlayerHp;
        PlayerMana.OnPlayerUpdateMP -= UpdatePlayerMp;
        PlayerBlockUI.OnPlayerHpColorUpdate -= ChangeHpColor;
    }

    void Awake()
    {
        _startHPColor = _hpSlider.color;
        _startMPColor = _mpSlider.color;
    }

    public void ChangeHpColor(Color newColor)
    {
        _startHPColor = newColor;
    }

    void EnableMpPanel()
    {
        _mpGroupPanel.SetActive(true);
    }

    void DisableMpPanel()
    {
        _mpGroupPanel.SetActive(false);
    }

    private void UpdatePlayerMp(int nowMp, int maxMp)
    {
        float mpPercent = (float)nowMp / maxMp;

        AnimateSlider(mpPercent, _mpSlider, _takeMPColor, _startMPColor);

        _mpNow.SetText(nowMp.ToString());
        _mpMax.SetText($"/{maxMp.ToString()}");
    }

    private void UpdatePlayerHp(int nowHp, int maxHp)
    {
        float hpPercent = (float)nowHp / maxHp;

        AnimateSlider(hpPercent, _hpSlider, _takeDmgColor, _startHPColor);

        _hpNow.SetText(nowHp.ToString());
        _hpMax.SetText($"/{maxHp.ToString()}");

    }

    private void AnimateSlider(float newPercent, Image targetSlider, Color dmgColor, Color startColor)
    {
        targetSlider.DOFillAmount(newPercent, _animationDuration).SetEase(_easeType);

        var sequence = DOTween.Sequence();
        sequence.Append(targetSlider.DOColor(dmgColor, _animationDuration).SetEase(_easeType));
        sequence.Append(targetSlider.DOColor(startColor, _animationDuration).SetEase(_easeType));
    }
}
