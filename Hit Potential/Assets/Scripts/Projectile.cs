using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Variables
    [SerializeField] private float speed;
    [SerializeField] private float fall; 
    [SerializeField] private Sprite corpse;
    [SerializeField] private Sprite[] sprites;
    #endregion

    void Start()
    {
        //Add Force to rigidbody
        var rigid = gameObject.GetComponent<Rigidbody2D>();
        rigid.AddForce(rigid.transform.up * speed);

        //Destroy bullet when it would hit the ground
        Destroy(gameObject, fall);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Kill ememy upon impact
        if(collision.collider.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Die();

            Destroy(gameObject);
        }
        
        //Damage player upon impact
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
