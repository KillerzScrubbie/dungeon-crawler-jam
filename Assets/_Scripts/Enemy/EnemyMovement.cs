using Pathfinding;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float nextWaypointDistance = 0.5f;

    private Seeker seeker;
    private AILerp ai;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<AILerp>();
    }

    private void Start()
    {
        seeker.StartPath(transform.position, targetPosition.position);
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
        
    }
}
