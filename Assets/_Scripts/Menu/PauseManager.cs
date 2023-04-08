using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;

    private bool isPaused = false;

    private PlayerInputController controller;

    private void Awake()
    {
        controller = FindObjectOfType<PlayerInputController>();
    }

    private void Start()
    {
        PlayerInputController.OnPause += HandlePause;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus) { return; }

        Pause();
    }

    private void HandlePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        controller.DisableMovement();

        isPaused = !isPaused;
    }

    public void Resume()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        controller.EnableMovement();

        isPaused = !isPaused;
    }

    private void OnDestroy()
    {
        PlayerInputController.OnPause -= HandlePause;
    }
}
