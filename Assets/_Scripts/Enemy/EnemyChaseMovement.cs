using Pathfinding;
using System;
using UnityEngine;

public class EnemyChaseMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private EnemyCombatDetector detector;

    public static event Action<EnemyChaseMovement> OnCombatEntered;
    public event Action OnPatrolStarted;

    private Seeker seeker;
    private EnemyPathfindingAI ai;

    private bool isChasing = false;
    private bool reachedEndOfPath = false;
    private bool isInState = false;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<EnemyPathfindingAI>();
    }

    private void Start()
    {
        detector.OnCombatDetected += HandleCombatEntered;
    }

    public void OnChaseEntered()
    {
        ai.speed = speed;
        isChasing = true;
        isInState = true;
    }

    public void OnChaseExited()
    {
        ai.speed = 0f;
        isChasing = false;
        isInState = false;
    }

    public void UpdateChaseMovement()
    {
        if (!isChasing) { return; }

        if (!reachedEndOfPath) { return; }

        OnPatrolStarted?.Invoke();
    }

    public void ReachEndOfPath()
    {
        reachedEndOfPath = true;
    }

    private void SetPath(Vector3 targetPos)
    {
        seeker.StartPath(transform.position, targetPos);
        reachedEndOfPath = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isInState) { return; }

        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        isChasing = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isInState) { return; }

        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        SetPath(playerMovement.transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isInState) { return; }

        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        isChasing = false;
    }
    
    private void HandleCombatEntered()
    {
        OnCombatEntered?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        detector.OnCombatDetected -= HandleCombatEntered;
    }
}
