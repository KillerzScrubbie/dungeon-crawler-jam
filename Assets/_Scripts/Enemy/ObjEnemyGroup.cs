using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Create new Encounter")]
public class ObjEnemyGroup : ScriptableObject
{
    [SerializeField] private List<ObjEnemy> enemyGroup;
    [SerializeField] private bool boss;

    private List<ObjEnemy> SetEnemyGroup()
    {
        List<ObjEnemy> currentEnemyGroup = new();

        foreach (var enemy in enemyGroup)
        {
            currentEnemyGroup.Add(enemy);
        }

        return currentEnemyGroup;
    }

    public List<ObjEnemy> EnemyGroup { get { return SetEnemyGroup(); } }
    public bool IsBoss { get { return boss; } }
}
