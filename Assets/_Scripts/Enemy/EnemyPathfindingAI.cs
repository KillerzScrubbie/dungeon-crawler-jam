using Pathfinding;
using UnityEngine;

public class EnemyPathfindingAI : AILerp
{
    [Space]
    [Header("Custom Fields")]
    [SerializeField] private EnemyStateManager enemyStateManager;

    public override void OnTargetReached()
    {
        enemyStateManager.HandlePathingFinished();
    }
}
