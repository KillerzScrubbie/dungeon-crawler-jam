using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform locationListParent;

    public event Action OnChase;

    private List<Transform> locationList = new();

    private Seeker seeker;
    private EnemyPathfindingAI ai;
    private EnemyData enemyData;
    private float speed = 0.2f;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        ai = GetComponent<EnemyPathfindingAI>();
        enemyData = transform.parent.gameObject.GetComponent<EnemyData>();
        enemyData.OnInitializeSpeed += SetSpeed;
    }

    private void Start()
    {
        foreach (Transform location in locationListParent)
        {
            locationList.Add(location);
        }

        FindRandomPath();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
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

    private void OnDestroy()
    {
        enemyData.OnInitializeSpeed -= SetSpeed;
    }
}
