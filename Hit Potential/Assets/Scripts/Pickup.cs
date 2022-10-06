using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour 
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Sprite[] disguises;

    public bool isDisguise;
    public int index;

    public void Switch()
    {
        renderer.sprite = disguises[index];
    }
}

