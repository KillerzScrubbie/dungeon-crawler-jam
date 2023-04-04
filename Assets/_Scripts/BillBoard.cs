using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Camera _cam;

    void Start()
    {
        // the blue arrow is where it will face camera, that is the only thing that need in prefab
        _cam = Camera.main;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward,
            _cam.transform.rotation * Vector3.up);
    }
}
