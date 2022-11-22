using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Level 1");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("Insert Name of Scene");
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        
        Application.Quit();
        Debug.Log("Quit!");
    }
}
