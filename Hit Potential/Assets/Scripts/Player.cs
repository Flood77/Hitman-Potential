using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] private float speed = 10;
    [SerializeField] private float knifeTimer = 0; 
    [SerializeField] private float attackTimer = 0; 

    [SerializeField] private Tilemap walls;
    [SerializeField] private GSystem system;
    [SerializeField] private Weapons weapons;
    [SerializeField] private SpriteController sprCtrl;
    [SerializeField] private SpriteController weaponSprCtrl;
    [SerializeField] private BoxCollider2D weaponHitBox;
    [SerializeField] private Transform pistolBulletSpawn;
    [SerializeField] private Transform[] shotgunBulletSpawn;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Animator knifeSlash;

    //0 - base, 1 - mafia, 2 - police
    private int outfit = 0;
    private int currentHealth;
    private int health = 1;
    private int activeWeapon;
    private bool inCombat = false;
    private bool canAttack = false;
    private bool attacking = false;
    //Implement Ammo

    public string Health { get { return currentHealth + " / " + health; } }
    #endregion

    private void Start()
    {
        //set starting health
        currentHealth = health;

        //Check for first active weapon and equip it
        if (weapons.IsActive(0)) 
        { 
            activeWeapon = 0;
            weaponSprCtrl.Switch(activeWeapon);
        }
        else if (weapons.IsActive(1)) 
        { 
            activeWeapon = 1;
            weaponSprCtrl.Switch(activeWeapon);
        }
    }

    void Update()
    {
        //Attack Cooldown Timer
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer <= 0)
            {
                canAttack = true;
            }
        }

        //Knife Collision Uptime Timer
        if (attacking)
        {
            knifeTimer -= Time.deltaTime;
            if (knifeTimer <= 0)
            {
                weaponHitBox.enabled = false;
                attacking = false;
            }
        }

        //Call functions that take user input
        Movement();
        Rotation();
        ChangeWeapon();
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

    }

    #region Movement
    //Move player based on WASD controls
    private void Movement()
    {
        //Move up
        if (Input.GetKey(KeyCode.W))
        {
            //Take current position and cell position above player
            var temp = transform.position;
            var nextCell = (float)Math.Ceiling(temp.y);

            //Check distance to the next cell
            if (nextCell - temp.y <= 0.3)
            {
                //Check if next cell is a wall using its sprite
                var cell = walls.WorldToCell(new Vector3(temp.x, nextCell, temp.z));
                var sprite = walls.GetSprite(cell);
                if (!sprite)
                {
                    temp.y += speed;
                }
            }
            else
            {
                temp.y += speed;
            }

            //Set position to adjusted value
            transform.position = temp;
        }
        //Move down
        if (Input.GetKey(KeyCode.S))
        {
            //Take current position and bottom of current cell
            var temp = transform.position;
            var nextCell = (float)Math.Floor(temp.y);

            //Check distance to the bottom of the cell
            if (nextCell - temp.y >= -0.3)
            {
                //Check if next cell is a wall using its sprite
                var cell = walls.WorldToCell(new Vector3(temp.x, nextCell - 1, temp.z));
                var sprite = walls.GetSprite(cell);
                if (!sprite)
                {
                    temp.y -= speed;
                }
            }
            else
            {
                temp.y -= speed;
            }

            //Set position to adjusted value
            transform.position = temp;
        }
        //Move right
        if (Input.GetKey(KeyCode.D))
        {
            //Take current position and cell position to the right of the player
            var temp = transform.position;
            var nextCell = (float)Math.Ceiling(temp.x);

            //Check distance to the next cell
            if (nextCell - temp.x <= 0.3)
            {
                //Check if next cell is a wall using its sprite
                var cell = walls.WorldToCell(new Vector3(nextCell, temp.y, temp.z));
                var sprite = walls.GetSprite(cell);
                if (!sprite)
                {
                    temp.x += speed;
                }
            }
            else
            {
                temp.x += speed;
            }

            //Set position to adjusted value
            transform.position = temp;
        }
        //Move left
        if (Input.GetKey(KeyCode.A))
        {
            //Take current position and left wall of current cell
            var temp = transform.position;
            var nextCell = (float)Math.Floor(temp.x);

            //Check distance to the left wall of the cell
            if (nextCell - temp.x >= -0.3)
            {
                //Check if next cell is a wall using its sprite
                var cell = walls.WorldToCell(new Vector3(nextCell - 1, temp.y, temp.z));
                var sprite = walls.GetSprite(cell);
                if (!sprite)
                {
                    temp.x -= speed;
                }
            }
            else
            {
                temp.x -= speed;
            }

            //Set position to adjusted value
            transform.position = temp;
        }
    }

    //Rotate player based on mouse position
    private void Rotation()
    {
        //Get mouseposition on the screen
        var mp = Input.mousePosition;
        mp = Camera.main.ScreenToWorldPoint(mp);

        //set character facing direction
        //to where the mouse is
        var direction = new Vector2(mp.x - transform.position.x, mp.y - transform.position.y);
        transform.up = direction;
    }
    #endregion

    #region Weapons
    //Attack based on currently selected weapon
    private void Attack()
    {
        //Activate weapon if deactivated
        if(!inCombat)
        {
            ActivateWeapon(true);
        }
        //Knife Attack
        if(activeWeapon == 0)
        {
            if(canAttack)
            {
                //Reset Timer, enable collider, & play animation
                attackTimer = .5f;
                weaponHitBox.enabled = true;
                canAttack = false;

                attacking = true;
                knifeTimer = .45f;
                knifeSlash.SetTrigger("Attack");
            }
        }
        //Shooting Logic
        else
        {
            //Shotgun Attack
            if(activeWeapon == 3)
            {
                //Spawn bullet for each spawn point
                foreach(var a in shotgunBulletSpawn)
                {
                    Instantiate(Bullet, a.position, a.rotation);
                }
            }
            //Pistol Attack
            else
            {
                //Spawn bullet from spawn point
                Instantiate(Bullet, pistolBulletSpawn.position, pistolBulletSpawn.rotation);

                //Sound wave if not silenced
                if (activeWeapon != 1)
                {
                    system.CreateSoundIndicator(this.gameObject, 3, true);
                }
                else
                {
                    system.CreateSoundIndicator(this.gameObject, 1, true);
                }
            }
        }
    }

    //Enable/Disable weapon sprite
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

    //Change current weapon
    private void ChangeWeapon()
    {
        var changed = false;

        //Checks weapon if it is available, then set true
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

        //Reload Weapon
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
    #endregion

    #region External Dealings
    //Called by enemy when seen
    public bool LooksFriendly(bool isMafia)
    {
        bool temp = false;

        //Check if player outfit matches enemy faction
        if (isMafia && outfit == 1) temp = true;
        else if (!isMafia && outfit == 2) temp = true;

        //Check if player weapon is out
        if (inCombat) temp = false;
        
        //Equip weapon if seen as enemy
        if(!temp) ActivateWeapon(true);

        return temp;
    }
    
    //Called upon being to hit to inflict damage
    public void Damage()
    {
        //Decrement health and check for death
        health--;
        if(health == 0)
        {
            system.GameOver();
        }
    }

    //Collision Trigger Stay for sensing pickups
    private void OnTriggerStay2D(Collider2D collision)
    {
        //If collided object is a pickup
        var obj = collision.gameObject;
        if(obj.tag == "Pickup")
        {
            var comp = obj.GetComponent<Pickup>();
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Change player to proper outfit and change pickup to previous outfit
                if (comp.isDisguise)
                {
                    var current = sprCtrl.GetCurrent();

                    sprCtrl.Switch(comp.index);

                    comp.index = current;
                    comp.Switch();
                }
                //Activate weapon and destoy pickup
                else
                {
                    weapons.SetActive(comp.index);
                    Destroy(obj);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyKnife") Damage();
        else if (collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            Damage();
        }
    }
    #endregion
}
