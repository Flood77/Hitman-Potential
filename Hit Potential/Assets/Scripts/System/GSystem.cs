using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSystem : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject soundIndicator;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Player player;

    private static bool gameIsPaused = false;
    private GameObject currentPauseMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameIsPaused)
            {
                MenuResume();
            }
            else
            {
                MenuPause();
            }
        }
    }

    //Create sound indicator objective
    public void CreateSoundIndicator(GameObject parent, int size, bool fromPlayer)
    {
        if (soundIndicator)
        {
            //Instantiate object and set if it was made by player or not
            var sound = Instantiate(soundIndicator, parent.transform);
            sound.GetComponent<SoundIndicator>().fromPlayer = fromPlayer;
            Destroy(sound, size);
        }
    }

    public void MenuPause()
    {
        currentPauseMenu = Instantiate(pauseMenuUI);
        Time.timeScale = 0f;
        gameIsPaused = true;
        player.isPaused = true;
    }

    public void MenuResume()
    {
        Destroy(currentPauseMenu);
        Time.timeScale = 1f;
        gameIsPaused = false;
        player.isPaused = false;
    }

    //Load main menu
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    //Restart current seen
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Instantiate the Gamover UI
    public void GameOver()
    {
        Instantiate(gameOverUI);
    }

}
