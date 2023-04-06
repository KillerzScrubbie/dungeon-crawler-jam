using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private GameObject combatCanvas;

    [Space]
    [Header("Slots List")]
    [SerializeField] private List<Image> enemySlot;
    
    [Space]
    [Header("Editing variables")]
    [SerializeField] private List<Image> actionsSlot;
    [SerializeField] private List<GameObject> equippedSlot;
    [SerializeField] private List<GameObject> potionSlot;

    private ObjEnemyGroup currentCombatGroup;
    private List<ObjEnemy> enemyList = new();

    private void Start()
    {
        PlayerMovement.OnCombatEntered += CombatEntered;
        EnemyCombatState.OnCombatEntered += SetupCombatScreen;
    }

    private void CombatEntered()
    {
        combatCanvas.SetActive(true);
    }

    private void CombatFinished()
    {
        combatCanvas.SetActive(false);
    }

    private void SetupCombatScreen(EnemyData enemyData)
    {
        currentCombatGroup = enemyData.EnemyGroup;
        enemyList.Clear();
        enemyList = currentCombatGroup.EnemyGroup;

        foreach (Image enemyImage in enemySlot)
        {
            enemyImage.sprite = null;
            enemyImage.gameObject.SetActive(false);
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            ObjEnemy enemy = enemyList[i];
            Image currentEnemy = enemySlot[i];

            currentEnemy.sprite = enemy.Image;
            currentEnemy.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        PlayerMovement.OnCombatEntered -= CombatEntered;
        EnemyCombatState.OnCombatEntered -= SetupCombatScreen;
    }
}
