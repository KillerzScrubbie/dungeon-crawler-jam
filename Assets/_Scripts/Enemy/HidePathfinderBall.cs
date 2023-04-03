using UnityEngine;

public class HidePathfinderBall : MonoBehaviour
{
    [SerializeField] private GameObject pathfinderSphere;

    private void Start()
    {
        pathfinderSphere.SetActive(false);
    }
}
