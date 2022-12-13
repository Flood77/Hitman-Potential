using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelfDestruct : MonoBehaviour
{
    public void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
