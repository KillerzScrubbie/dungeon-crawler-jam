using UnityEngine;
using TMPro;
using System;
using DamageNumbersPro;

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



    private void DoPopupPlayerHeal(int healVal)
    {
        DamageNumber healText = _PlayerGetHeal.Spawn(Vector3.zero, healVal);
        healText.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));
    }

    private void DoPlayerDamagePopup(int damage)
    {
        if (damage <= 0) return;
        
        DamageNumber playerDmgTxt = _PlayerGetDamage.Spawn(Vector3.zero, damage);
        playerDmgTxt.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));
        
    }

    private void DoPlayerTakeBlockDamage(int blockUsed)
    {
        if (blockUsed <= 0) return;
        
        DamageNumber playerBlockTxt = _PlayerGetBlockDamage.Spawn(Vector3.zero, blockUsed);
        playerBlockTxt.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));
    }

    private void DoPopupPlayerGetMP(int getMpValue)
    {
        DamageNumber playerMP = _PlayerGetMana.Spawn(Vector3.zero, getMpValue);
        playerMP.SetAnchoredPosition(_PlayerHpTrans, new Vector2(0, 0));
    }

    public void DoEnemyDamagePopup(int damage, Transform trans)
    {
        DamageNumber enemyDmg = _EnemyGetDamage.Spawn(Vector3.zero, damage);
        enemyDmg.SetAnchoredPosition(trans, new Vector2(0, 0));
    }

    private void DoEnemyHealPopup(int heal, Transform trans)
    {
        DamageNumber enemyHeal = _EnemyGetHeal.Spawn(Vector3.zero, heal);
        enemyHeal.SetAnchoredPosition(trans, new Vector2(0, 0));
    }
    
    

}
