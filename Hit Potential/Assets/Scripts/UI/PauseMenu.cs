using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenu;

    private static bool gameIsPaused = false;
    private GameObject currentPauseMenu;

    //Checks for player pausing game
    void Update()
    {
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

    //continue game time and destroy menu UI
    public void Resume()
    {
        Destroy(currentPauseMenu);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    //stops game time, creates and stores menu ui
    public void Pause()
    {
        currentPauseMenu = Instantiate(pauseMenuUI);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadOptionsMenu()
    {
        Instantiate(optionsMenu);
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
