using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonMenuDestruction : MonoBehaviour
{
    public GameObject MainMenu;
    public void Back()
    {
        Instantiate<GameObject>(MainMenu);

        Destroy(this.gameObject);
    }
}
