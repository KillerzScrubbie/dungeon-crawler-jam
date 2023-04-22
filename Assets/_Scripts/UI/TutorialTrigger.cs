using System;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private ObjTutorial tutorial;

    public static event Action<ObjTutorial> OnTutorialStarted;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PlayerMovement playerMovement)) { return; }

        OnTutorialStarted?.Invoke(tutorial);
        gameObject.SetActive(false);
    }
}
