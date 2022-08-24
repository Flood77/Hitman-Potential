using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    private int current = 0;

    public void Switch(int choice)
    {
        if (choice < sprites.Length)
        {
            spriteRenderer.sprite = sprites[choice];
            current = choice;
        }
    }

    public int GetCurrent()
    {
        return current;
    }

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
