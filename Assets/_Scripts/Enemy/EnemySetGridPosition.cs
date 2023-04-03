using UnityEngine;

public class EnemySetGridPosition : MonoBehaviour
{
    [SerializeField] private Transform pathfinderBall;

    private void Update()
    {
        Vector3 position = pathfinderBall.position;
        transform.position = new Vector3(Mathf.RoundToInt(position.x), position.y, Mathf.RoundToInt(position.z));
    }
}
