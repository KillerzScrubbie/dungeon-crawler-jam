using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform locationListParent;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float nextWaypointDistance = 0.5f;

    public event Action OnChase;

    private List<Transform> locationList = new();

    private Seeker seeker;
    private EnemyPathfindingAI ai;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<EnemyPathfindingAI>();
    }

    private void Start()
    {
        foreach (Transform location in locationListParent)
        {
            locationList.Add(location);
            Debug.Log(location);
        }

        FindRandomPath();
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

    public void ReachEndOfPath()
    {
        FindRandomPath();
    }

    private void SetPath(Vector3 targetPos)
    {
        seeker.StartPath(transform.position, targetPos);
    }

    public void FindRandomPath()
    {
        Transform randomPath = locationList[UnityEngine.Random.Range(0, locationList.Count)];

        SetPath(randomPath.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement playerMovement))
        {
            OnChase?.Invoke();
        }
    }
}
