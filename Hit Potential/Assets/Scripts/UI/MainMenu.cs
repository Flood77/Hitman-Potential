using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LevelSelectMenu;
    public GameObject OpitionsMenu;
    public GameObject CreditsMenu;
    public GameObject HowToPlayMenu;
    public void Play()
    {
        //On play creates the the prefab of the UI level select object Assigned to the LevelSelectMenu GameObject
        Instantiate<GameObject>(LevelSelectMenu);

        Destroy(this.gameObject);

        //SceneManager.LoadScene("Level 1");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("Insert Name of Scene");
    }

    public void HowtoPlay()
    {
        Instantiate<GameObject>(HowToPlayMenu);

        Destroy(this.gameObject);
    }

    public void Options()
    {
        Instantiate<GameObject>(OpitionsMenu);

        Destroy(this.gameObject);
    }

    public void Credits()
    {
        Instantiate<GameObject>(CreditsMenu);

        Destroy(this.gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
