using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Create new Encounter")]
public class ObjEnemyGroup : ScriptableObject
{
    [SerializeField] private List<ObjEnemy> enemyGroup;

    public List<ObjEnemy> EnemyGroup { get { return enemyGroup; } }
}
