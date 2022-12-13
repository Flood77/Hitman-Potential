using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonMenuDestruction : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelectMenu;
    public void Back()
    {
        Instantiate<GameObject>(mainMenu);

        Destroy(this.gameObject);
    }

    public void XButtonLevelPreview()
    {
        Instantiate<GameObject>(levelSelectMenu);

        Destroy(this.gameObject);
    }


}
