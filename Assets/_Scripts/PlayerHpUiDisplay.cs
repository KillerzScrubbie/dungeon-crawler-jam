using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHpUiDisplay : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] TMP_Text _hpNow;
    [SerializeField] TMP_Text _hpMax;

    void OnEnable()
    {
        PlayerHp.OnPlayerUpdateHp += UpdatePlayerHp;
    }
    void OnDisable()
    {
        PlayerHp.OnPlayerUpdateHp -= UpdatePlayerHp;
    }

    private void UpdatePlayerHp(int nowHp, int maxHp)
    {
        // in case max hp change it take that as well
        _hpSlider.maxValue = maxHp;
        _hpSlider.value = nowHp;

        _hpNow.SetText(nowHp.ToString());
        _hpMax.SetText(maxHp.ToString());

    }
}
