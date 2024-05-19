using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour 
{
    [SerializeField] private SpriteRenderer ren;
    [SerializeField] private Sprite[] disguises;
    [SerializeField] private bool isDisguise;
    [SerializeField] private int index;

    public bool IsDisguise { get { return isDisguise; } }
    public int Index { get { return index; } }

    //Switches sprite to new disguise
    public void Switch(int index)
    {
        this.index = index;
        ren.sprite = disguises[index];
    }
}

