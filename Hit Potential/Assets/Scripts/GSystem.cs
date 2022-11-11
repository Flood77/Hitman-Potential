using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GSystem : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject SoundIndicator;  

    void Start()
    {

    }

    void Update()
    {

    }

    //Create sound indicator objective
    public void CreateSoundIndicator(GameObject parent, int size, bool fromPlayer)
    {
        if (SoundIndicator)
        {
            //Instantiate object and set if it was made by player or not
            var sound = Instantiate(SoundIndicator, parent.transform);
            sound.GetComponent<SoundIndicator>().fromPlayer = fromPlayer;
            Destroy(sound, size);
        }
    }

    //Load main menu
    public void MainMenu()
    {
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
