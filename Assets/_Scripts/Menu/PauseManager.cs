using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject confirmQuitPanel;

    private bool isPaused = false;
    private bool isInCombat = false;

    private PlayerInputController controller;

    private void Awake()
    {
        controller = FindObjectOfType<PlayerInputController>();
    }

    private void Start()
    {
        PlayerInputController.OnPause += HandlePause;
        CombatManager.OnCombatStateChanged += HandleCombatState;
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

    private void HandleCombatState(CombatState state)
    {
        switch (state)
        {
            case CombatState.StartCombat:
                isInCombat = true;
                break;
            case CombatState.NotInCombat:
                isInCombat = false; 
                break;
        }
    }

    private void Pause()
    {
        pauseCanvas.SetActive(true);
        isPaused = !isPaused;

        if (isInCombat) { return; }

        Time.timeScale = 0f;
        controller.DisableMovement();  
    }

    public void Resume()
    {
        optionsPanel.SetActive(false);
        pauseCanvas.SetActive(false);
        confirmQuitPanel.SetActive(false);
        isPaused = !isPaused;

        if (isInCombat) { return; }

        Time.timeScale = 1f;
        controller.EnableMovement(); 
    }

    private void OnDestroy()
    {
        PlayerInputController.OnPause -= HandlePause;
        CombatManager.OnCombatStateChanged -= HandleCombatState;
    }
}
