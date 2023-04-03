using UnityEngine;

public class EnemySetGridPosition : MonoBehaviour
{
    [SerializeField] private Transform pathfinderBall;

    private readonly float yOffset = 0.5f;
    private readonly float stairsOffset = 1f;

    private void Update()
    {
        Vector3 position = pathfinderBall.position;

        float offset = position.y % 1 >= 0.134f & position.y % 1 <= 0.5f ? stairsOffset : yOffset ;

        transform.position = new Vector3(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y) + offset, Mathf.RoundToInt(position.z));
    }
}
