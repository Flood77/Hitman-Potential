using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public bool bullet;
    public float speed;
    public float fall;

    [SerializeField] private Sprite corpse;
    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        //set sprite based on if its a bullet or a knife
        gameObject.GetComponent<SpriteRenderer>().sprite = bullet ? sprites[0] : sprites[1];

        //Add Force 
        var rigid = gameObject.GetComponent<Rigidbody2D>();
        rigid.AddForce(rigid.transform.up * speed);

        //destroy bullet when it would hit the ground
        Destroy(gameObject, fall);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Kill ememy if player bullet
        if(collision.collider.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Die();

            Destroy(gameObject);
        }
        
        //Kill player if enemy bullet
        if(collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().Damage();

            Destroy(gameObject);
        }

        //Damage wall on hit
        if(collision.collider.tag == "Wall")
        {
            //Change Wall Sprite to show damage
            var spr = collision.gameObject.GetComponent<SpriteController>();
            if (spr)
            {
                spr.Switch(spr.GetCurrent() + 1);
                Destroy(gameObject);
            }
        }

        //Destroy bullet on hit
        Destroy(gameObject);
    }
}
