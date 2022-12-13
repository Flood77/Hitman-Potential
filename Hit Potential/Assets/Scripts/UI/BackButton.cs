using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] private GameObject previousMenu;

    //Destroy current menu and instantiate previous menu
    public void Back()
    {
        Instantiate(previousMenu);

        Destroy(this.gameObject);
    }
}
