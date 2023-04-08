using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    public void GoToString(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1.0f;
    }

    public void GoToIndex(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1.0f;
    }

    public void RestartThisScene()
    {
        GoToIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
