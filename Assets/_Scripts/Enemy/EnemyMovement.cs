using Pathfinding;
using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float nextWaypointDistance = 0.5f;

    public event Action OnChase;

    private Seeker seeker;
    private EnemyPathfindingAI ai;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<EnemyPathfindingAI>();
    }

    private void Start()
    {
        SetPath(targetPosition.position);
    }

    public void OnPatrolEntered()
    {
        ai.speed = speed;
    }

    public void OnPatrolExited()
    {
        ai.speed = 0f;
    }

    public void UpdatePatrolMovement()
    {
        //if (playerFinderCollider.) { }
    }

    private void SetPath(Vector3 targetPos)
    {
        seeker.StartPath(transform.position, targetPos);
    }

    public void FindRandomPath()
    {
        SetPath(targetPosition.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            OnChase?.Invoke();
        }
    }
}
