using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    [SerializeField] private GameObject clothes;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isNaked = false;

    //TODO setup player connections to these functions
    public void takeClothes()
    {
        if (isNaked) return;

        //TODO create clothes pickup

        //TODO setup correct sprite
        //Set to naked sprite
        spriteRenderer.sprite = Resources.Load<Sprite>("corpse_naked");
        isNaked = true;
    }

    //TODO set up move logic
    public void move()
    {

    }
}
