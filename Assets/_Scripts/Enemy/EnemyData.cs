using System;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField] private ObjEnemyGroup enemyGroup;
    [SerializeField] private SpriteRenderer enemyModel;

    public event Action<float> OnInitializeSpeed;

    private ObjEnemy enemy;

    private void Awake()
    {
        enemy = enemyGroup.EnemyGroup[0];
        enemyModel.sprite = enemy.Image;
    }

    private void Start()
    {
        OnInitializeSpeed?.Invoke(enemy.GetRandomSpeed());
    }

    public ObjEnemyGroup EnemyGroup { get { return enemyGroup; } }
}
