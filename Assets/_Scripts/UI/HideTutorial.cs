using System;
using UnityEngine;

public class HideTutorial : MonoBehaviour
{
    public static event Action OnTutorialCompleted;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        OnTutorialCompleted?.Invoke();
        gameObject.SetActive(false);
    }
}
