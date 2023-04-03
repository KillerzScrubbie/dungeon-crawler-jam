using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    private bool isPaused = false;

    private void Start()
    {
        PlayerInputController.OnPause += HandlePause;
    }

    private void HandlePause()
    {
        if (isPaused)
        {
            pauseCanvas.SetActive(false);
        }
        else
        {
            pauseCanvas.SetActive(true);
        }

        isPaused = !isPaused;
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false);
        isPaused = false;
    }



    private void OnDestroy()
    {
        PlayerInputController.OnPause -= HandlePause;
    }
}
