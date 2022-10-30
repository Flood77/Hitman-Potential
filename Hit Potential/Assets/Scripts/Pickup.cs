using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour 
{
    [SerializeField] private SpriteRenderer ren;
    [SerializeField] private Sprite[] disguises;

    public bool isDisguise;
    public int index;

    public void Switch()
    {
        ren.sprite = disguises[index];
    }
}

