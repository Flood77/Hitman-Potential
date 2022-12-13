using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public GameObject LevelPreviewMenu;

    public void Level1Selected()
    {
        Instantiate<GameObject>(LevelPreviewMenu);
    }
}
