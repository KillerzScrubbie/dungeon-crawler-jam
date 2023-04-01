using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHpUiDisplay : MonoBehaviour
{
    [SerializeField] Image _hpSlider;
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
        float hpPercent = (float)nowHp / maxHp;
        _hpSlider.fillAmount = hpPercent;

        _hpNow.SetText(nowHp.ToString());
        _hpMax.SetText(maxHp.ToString());

    }
}
