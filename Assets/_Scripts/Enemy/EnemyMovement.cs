using Pathfinding;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float nextWaypointDistance = 0.5f;

    private Seeker seeker;
    private AILerp ai;
    private Collider playerFinderCollider;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<AILerp>();
        playerFinderCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        SetPath(targetPosition.position);
    }

    public void OnStateEntered()
    {
        ai.speed = speed;
    }

    public void OnStateExited()
    {
        ai.speed = 0f;
    }

    public void UpdateMovement()
    {
        //if (playerFinderCollider.) { }
    }

    private void SetPath(Vector3 targetPos)
    {
        seeker.StartPath(transform.position, targetPos);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            SetPath(playerMovement.transform.position);
        }
    }
}
