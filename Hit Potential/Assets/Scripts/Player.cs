using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private Weapons weapons;
    [SerializeField] private SpriteController sprCtrl;

    private int activeWeapon;
    private int stage = 0;
    //0 - normal, 1 - combat
    private int look = 0;
    //0 - base, 1 - mafia, 2 - police

    private void Start()
    {
        //Check for first active weapon and equip it
        if (weapons.IsActive(1)) { activeWeapon = 1; }
        else if (weapons.IsActive(2)) { activeWeapon = 2; }
    }

    void Update()
    {
        //move player based on wasd
        movement();
        //Rotate player to where the mouse is 
        rotation();
        //Change selected weapon
        changeWeapon();
        //Attack based on selected weapon
        attack();
    }

    private void movement()
    {
        //Based on key pressed move character
        //in selected direction
        if (Input.GetKey(KeyCode.W))
        {
            var temp = transform.position;
            temp.y += speed;
            transform.position = temp;
        }
        if (Input.GetKey(KeyCode.S))
        {
            var temp = transform.position;
            temp.y -= speed;
            transform.position = temp;
        }
        if (Input.GetKey(KeyCode.D))
        {
            var temp = transform.position;
            temp.x += speed;
            transform.position = temp;
        }
        if (Input.GetKey(KeyCode.A))
        {
            var temp = transform.position;
            temp.x -= speed;
            transform.position = temp;
        }
    }

    private void rotation()
    {
        //Get mouseposition on the screen
        var mp = Input.mousePosition;
        mp = Camera.main.ScreenToWorldPoint(mp);

        //set character facing direction
        //to where the mouse is
        var direction = new Vector2(mp.x - transform.position.x, mp.y - transform.position.y);
        transform.up = direction;
    }

    private void attack()
    {
        //Attack based on if it physical or projectile
        if(activeWeapon == 0)
        {
            //Figure out melee attack
        }
        else
        {
            //spawn projectile(s)
            if(activeWeapon == 3)
            {
                //shotgun - multiple projectiles
            }
            else
            {
                //pistols - one projectile
            }

            //Sound wave if not silenced
            if(activeWeapon != 1)
            {

            }
        }
    }

    private void changeWeapon()
    {
        //Checks if weapon is available,
        //then changes selected weapon and
        //current character sprite
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weapons.IsActive(0)) activeWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapons.IsActive(1)) activeWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weapons.IsActive(2)) activeWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (weapons.IsActive(3)) activeWeapon = 3;
        }

        //Change combat sprite
        if(stage == 1)
        {

        }
    }

    //Called by enemy when seen
    public bool looksFriendly(bool isMafia)
    {
        //return true if disguise matches faction
        if (isMafia && look == 1) return true;
        else if(!isMafia && look == 2) return true;

        //else turn player to combat sprite and return false
        stage = 1;

        return false;
    }
}
