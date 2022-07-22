using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool friendly;
    private bool bullet;
    private float speed;
    private float fall;

    [SerializeField] private Sprite[] sprites;

    public Projectile(bool b, bool fr, float s, float f)
    {
        friendly = fr;
        bullet = b;
        speed = s;  
        fall = f;
    }


     void Start()
    {
        //set sprite based on if its a bullet or a knife
        gameObject.GetComponent<SpriteRenderer>().sprite = bullet ? sprites[0] : sprites[1];

        //destroy bullet when it would hit the ground
        Destroy(gameObject, fall);
    }

    void Update()
    {
        //Move in given position based on speed

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Kill ememy if player bullet
        if(collision.collider.tag == "Enemy" && friendly)
        {
            
        }
        
        //Kill player if enemy bullet
        if(collision.collider.tag == "Player" && !friendly)
        {

        }

        //Damage wall on hit
        if(collision.collider.tag == "Wall")
        {

        }

        //Destroy bullet on hit
        Destroy(gameObject);
    }
}
