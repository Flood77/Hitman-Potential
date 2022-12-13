using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject howToPlayMenu;

    //In the following functions, different UI elements are created
    //based on the button selected and this UI object is deleted
    public void Play()
    {
        Instantiate(levelSelectMenu);

        Destroy(this.gameObject);
    }

    public void HowtoPlay()
    {
        Instantiate(howToPlayMenu);

        Destroy(this.gameObject);
    }

    public void Options()
    {
        Instantiate(optionsMenu);

        Destroy(this.gameObject);
    }

    public void Credits()
    {
        Instantiate(creditsMenu);

        Destroy(this.gameObject);
    }

    //Quits the game, I think
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
