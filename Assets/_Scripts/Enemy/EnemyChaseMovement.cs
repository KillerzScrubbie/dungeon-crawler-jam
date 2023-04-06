using Pathfinding;
using System;
using UnityEngine;

public class EnemyChaseMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float nextWaypointDistance = 0.5f;

    public static event Action<EnemyChaseMovement> OnCombatEntered;
    public event Action OnPatrolStarted;

    private Seeker seeker;
    private EnemyPathfindingAI ai;

    private bool isChasing = false;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<EnemyPathfindingAI>();

    }

    public void OnChaseEntered()
    {
        ai.speed = speed;
        isChasing = true;
    }

    public void OnChaseExited()
    {
        ai.speed = 0f;
        isChasing = false;
    }

    public void UpdateChaseMovement()
    {
        if (!isChasing) { return; }

        // Check if enemy reaches last seen location

        OnPatrolStarted?.Invoke();
    }

    private void SetPath(Vector3 targetPos)
    {
        seeker.StartPath(transform.position, targetPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        isChasing = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        SetPath(playerMovement.transform.position);

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        isChasing = false;

    }
    // If player in combat, also call this. OnCombatEntered?.Invoke(this);
}
