using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GSystem gameSys;

    public void Start()
    {
        gameSys = FindObjectOfType<GSystem>();
    }

    //continue game time and destroy menu UI
    public void Resume()
    {
        gameSys.MenuResume();
    }

    //stops game time, creates and stores menu ui
    public void Pause()
    {
        gameSys.MenuPause();
    }

    public void LoadOptionsMenu()
    {
        Instantiate(optionsMenu);
    }

    public void LoadMainMenu()
    {
        gameSys.MainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
