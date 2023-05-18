using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DamageNumbersPro;
using System.Linq;

public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] TMP_Text _dmgEnemyPrefab;
    [SerializeField] TMP_Text _dmgPlayerPrefab;
    [SerializeField] Transform _mainCombatCanvas;


    [SerializeField] DamageNumber _PlayerGetMana;
    [SerializeField] DamageNumber _PlayerGetBlockDamage;
    [SerializeField] DamageNumber _PlayerGetDamage;
    [SerializeField] DamageNumber _PlayerGetHeal;

    [SerializeField] DamageNumber _EnemyGetDamage;
    [SerializeField] DamageNumber _EnemyGetHeal;

    [SerializeField] Transform _PlayerHpTrans;
    [SerializeField] Transform _PlayerBlockTrans;
    [SerializeField] Transform _PlayerMpTrans;

    List<DamageNumber> _damageNumList = new List<DamageNumber>();

    void OnEnable()
    {
        EnemyCombat.OnEnemyTakeDamage += DoEnemyDamagePopup;
        EnemyCombat.OnEnemyHeal += DoEnemyHealPopup;
        PlayerHp.OnPlayerTakeDamage += DoPlayerDamagePopup;
        PlayerHp.OnPlayerHeal += DoPopupPlayerHeal;
        PlayerMana.OnPlayerGetMP += DoPopupPlayerGetMP;
        PlayerBlock.OnUseBlockValue += DoPlayerTakeBlockDamage;
        PlayerCombatState.OnPlayerCombatState += ClearOldPopup;

    }
    void OnDisable()
    {
        EnemyCombat.OnEnemyTakeDamage -= DoEnemyDamagePopup;
        EnemyCombat.OnEnemyHeal -= DoEnemyHealPopup;
        PlayerHp.OnPlayerTakeDamage -= DoPlayerDamagePopup;
        PlayerHp.OnPlayerHeal -= DoPopupPlayerHeal;
        PlayerMana.OnPlayerGetMP -= DoPopupPlayerGetMP;
        PlayerBlock.OnUseBlockValue -= DoPlayerTakeBlockDamage;
        PlayerCombatState.OnPlayerCombatState -= ClearOldPopup;
    }

    private void ClearOldPopup()
    {
        _damageNumList = _damageNumList.Where(item => item != null).ToList();
        foreach (DamageNumber damageTxt in _damageNumList) Destroy(damageTxt.gameObject);

    }


    private void DoPopupPlayerHeal(int healVal)
    {
        DamageNumber healText = _PlayerGetHeal.Spawn(Vector3.zero, healVal);
        healText.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));
        _damageNumList.Add(healText);
    }

    private void DoPlayerDamagePopup(int damage)
    {
        if (damage <= 0) return;

        DamageNumber playerDmgTxt = _PlayerGetDamage.Spawn(Vector3.zero, damage);
        playerDmgTxt.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));

        _damageNumList.Add(playerDmgTxt);

    }

    private void DoPlayerTakeBlockDamage(int blockUsed)
    {
        if (blockUsed <= 0) return;

        DamageNumber playerBlockTxt = _PlayerGetBlockDamage.Spawn(Vector3.zero, blockUsed);
        playerBlockTxt.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));

        _damageNumList.Add(playerBlockTxt);
    }

    private void DoPopupPlayerGetMP(int getMpValue)
    {
        DamageNumber playerMP = _PlayerGetMana.Spawn(Vector3.zero, getMpValue);
        playerMP.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));

        _damageNumList.Add(playerMP);
    }

    public void DoEnemyDamagePopup(int damage, Transform trans)
    {
        DamageNumber enemyDmg = _EnemyGetDamage.Spawn(Vector3.zero, damage);
        enemyDmg.SetAnchoredPosition(trans, new Vector2(0, 0));

        _damageNumList.Add(enemyDmg);
    }

    private void DoEnemyHealPopup(int heal, Transform trans)
    {
        DamageNumber enemyHeal = _EnemyGetHeal.Spawn(Vector3.zero, heal);
        enemyHeal.SetAnchoredPosition(trans, new Vector2(0, 0));

        _damageNumList.Add(enemyHeal);

    }



}
