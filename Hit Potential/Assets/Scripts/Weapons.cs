using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    /*
     0 - knife
     1 - silenced pistol
     2 - pistol
     3 - shotgun
     */
    [SerializeField] private bool[] actives = new bool[4];

    //Returns if selected weapon is active
    public bool IsActive(int selected) { return actives[selected]; }

    //Returns actives for visual board
    public bool[] Actives() { return actives; }

    public void SetActive(int selected) { actives[selected] = true; }
}
