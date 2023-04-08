using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement player)) { return; }

        SceneManager.LoadScene(2);
    }
}
