using UnityEngine;

public class EnemySetGridPosition : MonoBehaviour
{
    [SerializeField] private Transform pathfinderBall;

    private readonly float yOffset = 0.5f;

    private void Update()
    {
        Vector3 position = pathfinderBall.position;
        transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + yOffset, Mathf.RoundToInt(position.z));
    }
}
