using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private GameObject levelPreviewMenu;

    //Create Level Preview 
    public void Level1Selected()
    {
        Instantiate<GameObject>(levelPreviewMenu);
    }
}
