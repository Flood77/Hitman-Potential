using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public GameObject levelPreviewMenu;

    public void Level1Selected()
    {
        Instantiate(levelPreviewMenu);
    }
}
