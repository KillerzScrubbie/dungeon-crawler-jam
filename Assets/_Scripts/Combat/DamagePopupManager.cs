using UnityEngine;
using TMPro;
using System;

public class DamagePopupManager : MonoBehaviour
{
    [Header("Popup Positions")]
    [SerializeField] private RectTransform healPopupPosition;

    [Space]
    [SerializeField] TMP_Text _dmgEnemyPrefab;
    [SerializeField] TMP_Text _dmgPlayerPrefab;
    [SerializeField] Transform _mainCombatCanvas;

    [SerializeField] Color _healPlayerColor;
    [SerializeField] Color _blockColor;
    [SerializeField] Color _manaColor;

    [SerializeField] Color _healEnemyColor;

    [SerializeField] SOnumberPopup _soPlayerGetMana;
    [SerializeField] SOnumberPopup _soPlayerGetBlockDamage;
    [SerializeField] SOnumberPopup _soPlayerGetDamage;
    [SerializeField] SOnumberPopup _soPlayerGetHeal;

    [SerializeField] SOnumberPopup _soEnemyGetDamage;
    [SerializeField] SOnumberPopup _soEnemyGetHeal;


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
        dmgPopupTrans.GetComponent<DamagePopup>().SetupSO(_soPlayerGetBlockDamage);
        dmgPopupTrans.transform.localPosition = new Vector3(-200, 0, 0);
        dmgPopupTrans.SetText($"({blockUsed})");
    }

    private void DoPopupPlayerGetMP(int getMpValue)
    {

        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        dmgPopupTrans.transform.localPosition = healPopupPosition.anchoredPosition;
        dmgPopupTrans.GetComponent<DamagePopup>().SetupSO(_soPlayerGetMana);
        dmgPopupTrans.SetText($"+{getMpValue}");
    }

    private void DoEnemyHealPopup(int heal, Transform trans)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgEnemyPrefab, trans);
        dmgPopupTrans.GetComponent<DamagePopup>().SetupSO(_soEnemyGetHeal);

        dmgPopupTrans.SetText(heal.ToString());
    }

    private void DoPopupPlayerHeal(int healVal)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        dmgPopupTrans.transform.position = healPopupPosition.anchoredPosition;
        dmgPopupTrans.GetComponent<DamagePopup>().SetupSO(_soPlayerGetHeal);
        dmgPopupTrans.SetText(healVal.ToString());
    }

    private void DoPlayerDamagePopup(int damage)
    {
        if (damage <= 0) return;
        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        dmgPopupTrans.GetComponent<DamagePopup>().SetupSO(_soPlayerGetDamage);
        dmgPopupTrans.SetText(damage.ToString());
    }


    public void DoEnemyDamagePopup(int damage, Transform trans)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgEnemyPrefab, trans);
        dmgPopupTrans.GetComponent<DamagePopup>().SetupSO(_soEnemyGetDamage);
        dmgPopupTrans.SetText(damage.ToString());
    }
}
