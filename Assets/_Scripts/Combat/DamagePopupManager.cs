using UnityEngine;
using TMPro;
using System;

public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] TMP_Text _dmgEnemyPrefab;
    [SerializeField] TMP_Text _dmgPlayerPrefab;
    [SerializeField] Transform _mainCombatCanvas;

    [SerializeField] Color _healPlayerColor;
    [SerializeField] Color _blockColor;
    [SerializeField] Color _manaColor;

    [SerializeField] Color _healEnemyColor;

    void OnEnable()
    {
        EnemyCombat.OnEnemyTakeDamage += DoEnemyDamagePopup;
        EnemyCombat.OnEnemyHeal += DoEnemyHealPopup;
        PlayerHp.OnPlayerTakeDamage += DoPlayerDamagePopup;
        PlayerHp.OnPlayerHeal += DoPopupPlayerHeal;
        PlayerMana.OnPlayerGetMP += DoPopupPlayerGetMP;
        PlayerBlock.OnUseBlockValue += DoPlayerTakeBlockDamage;
    }
    void OnDisable()
    {
        EnemyCombat.OnEnemyTakeDamage -= DoEnemyDamagePopup;
        EnemyCombat.OnEnemyHeal -= DoEnemyHealPopup;
        PlayerHp.OnPlayerTakeDamage -= DoPlayerDamagePopup;
        PlayerHp.OnPlayerHeal -= DoPopupPlayerHeal;
        PlayerMana.OnPlayerGetMP -= DoPopupPlayerGetMP;
        PlayerBlock.OnUseBlockValue -= DoPlayerTakeBlockDamage;
    }

    private void DoPlayerTakeBlockDamage(int blockUsed)
    {
        if (blockUsed <= 0) return;
        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        dmgPopupTrans.GetComponent<DamagePopup>().SetupTextColor(_blockColor);
        dmgPopupTrans.transform.localPosition = new Vector3(-200, 0, 0);
        dmgPopupTrans.SetText($"({blockUsed.ToString()})");
    }

    private void DoPopupPlayerGetMP(int getMpValue)
    {

        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        dmgPopupTrans.transform.localPosition = new Vector3(-600, 0, 0);
        dmgPopupTrans.GetComponent<DamagePopup>().SetupTextColor(_manaColor);
        dmgPopupTrans.SetText(getMpValue.ToString());
    }

    private void DoEnemyHealPopup(int heal, Transform trans)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgEnemyPrefab, trans);
        dmgPopupTrans.GetComponent<DamagePopup>().SetupTextColor(_healEnemyColor);
        dmgPopupTrans.SetText(heal.ToString());
    }

    private void DoPopupPlayerHeal(int healVal)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        dmgPopupTrans.GetComponent<DamagePopup>().SetupTextColor(_healPlayerColor);
        dmgPopupTrans.SetText(healVal.ToString());
    }

    private void DoPlayerDamagePopup(int damage)
    {
        if (damage <= 0) return;
        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        dmgPopupTrans.SetText(damage.ToString());
    }


    public void DoEnemyDamagePopup(int damage, Transform trans)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgEnemyPrefab, trans);
        dmgPopupTrans.SetText(damage.ToString());
    }


}
