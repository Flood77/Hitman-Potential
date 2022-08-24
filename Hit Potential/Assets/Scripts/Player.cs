using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private Weapons weapons;
    [SerializeField] private SpriteController sprCtrl;
    [SerializeField] private SpriteController weaponSprCtrl;

    //0 - base, 1 - mafia, 2 - police
    private int outfit = 0;
    private int activeWeapon;
    private bool inCombat = false;
    //Implement Ammo

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

    //Attack based on if it physical or projectile
    private void attack()
    {
        //Activate weapon if deactivated
        if(!inCombat)
        {
            ActivateWeapon(true);
        }
        //melee attack logic
        if(activeWeapon == 0)
        {
            //TODO: Implement Melee Attack with Knife
        }
        //spawn projectile(s) logic
        else
        {
            //TODO: Generate projectile(s) based on current direction
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
                //TODO: Implement Sound Wave on Audio Shot
            }
        }
    }

    private void changeWeapon()
    {
        var changed = false;

        //Checks if weapon is available,
        //then changes selected weapon and
        //current character sprite
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weapons.IsActive(0))
            {
                activeWeapon = 0;
                changed = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapons.IsActive(1))
            {
                activeWeapon = 1;
                changed = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weapons.IsActive(2))
            {
                activeWeapon = 2;
                changed = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (weapons.IsActive(3))
            {
                activeWeapon = 3;
                changed = true;
            }
        }

        //Reload 
        if (Input.GetKeyDown(KeyCode.R))
        {
            //TODO: Implement Reloading Mechanics
            //TODO: Add Reload Animation
        }

        //Put away weapons
        if (Input.GetKey(KeyCode.R))
        {
            ActivateWeapon(false);
        }

        //Change combat sprite
        if(changed)
        {
            weaponSprCtrl.Switch(activeWeapon);
        }
    }

    //Called by enemy when seen
    public bool looksFriendly(bool isMafia)
    {
        //return true if disguise matches faction
        if (isMafia && outfit == 1) return true;
        else if(!isMafia && outfit == 2) return true;

        //else turn player to combat sprite and return false
        ActivateWeapon(true);

        return false;
    }

    public void ActivateWeapon(bool Activate)
    {
        if (Activate)
        {
            inCombat = true;
            weaponSprCtrl.Activate(true);
        }
        else
        {
            inCombat = false;
            weaponSprCtrl.Activate(false);
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        var obj = collision.gameObject;
        if(obj.tag == "pickup")
        {
            var comp = obj.GetComponent<Pickup>();
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (comp.isDisguise)
                {
                    sprCtrl.Switch(comp.number);
                    //TODO: Change Pickup to current outfit
                }
                else
                {
                    weapons.SetActive(comp.number);
                    Destroy(obj);
                }
            }
        }
    }
}
