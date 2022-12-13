using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadLevel : MonoBehaviour
{
    //Load selected scene
    public void LoadSelectedLevel(int lvl)
    {
        SceneManager.LoadScene("Level " + lvl);
    }
}
