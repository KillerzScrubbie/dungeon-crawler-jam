using Pathfinding;
using UnityEngine;

public class EnemyPathfindingAI : AILerp
{
    [Space]
    [Header("Custom Fields")]
    [SerializeField] private EnemyMovement enemyMovement;

    public override void OnTargetReached()
    {
        enemyMovement.FindRandomPath();
    }
}
