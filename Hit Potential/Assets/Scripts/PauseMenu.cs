using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuObject;
   
    void Update()
    {
        Debug.Log(Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Instantiate<GameObject>(pauseMenuUI);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        Instantiate<GameObject>(pauseMenuUI);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadOptionsMenu()
    {
        Instantiate<GameObject>(optionsMenuObject);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
