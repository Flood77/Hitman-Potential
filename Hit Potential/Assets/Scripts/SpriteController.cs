using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    #region Variables
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private int current = 0;
    #endregion

    //Switch current sprite
    public void Switch(int choice)
    {
        //Confirm choice is valid
        if (choice < sprites.Length)
        {
            //Change current sprite
            spriteRenderer.sprite = sprites[choice];
            current = choice;
        }
    }

    //Returns int representing current sprite selection
    public int GetCurrent()
    {
        return current;
    }

    //Enables/Disables spriterenderer
    public void Activate(bool on)
    {
        if (on)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
