using UnityEngine;
using TMPro;

public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] TMP_Text _dmgEnemyPrefab;
    [SerializeField] TMP_Text _dmgPlayerPrefab;
    [SerializeField] Transform _mainCombatCanvas;

    void OnEnable()
    {
        EnemyCombat.OnEnemyTakeDamage += DoEnemyDamagePopup;
        PlayerHp.OnPlayerTakeDamage += DoPlayerDamagePopup;
    }
    void OnDisable()
    {
        EnemyCombat.OnEnemyTakeDamage -= DoEnemyDamagePopup;
        PlayerHp.OnPlayerTakeDamage -= DoPlayerDamagePopup;
    }

    private void DoPlayerDamagePopup(int damage)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgPlayerPrefab, _mainCombatCanvas);
        // dmgPopupTrans.transform.localPosition = Vector3.zero;
        dmgPopupTrans.SetText(damage.ToString());
    }

    public void DoEnemyDamagePopup(int damage, Transform trans)
    {
        TMP_Text dmgPopupTrans = Instantiate(_dmgEnemyPrefab, trans);
        // dmgPopupTrans.transform.localPosition = Vector3.zero;
        dmgPopupTrans.SetText(damage.ToString());
    }


}
