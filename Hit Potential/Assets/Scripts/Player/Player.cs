using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float knifeTimer = 0; 
    [SerializeField] private float attackTimer = 0;

    [SerializeField] private Tilemap walls;
    [SerializeField] private GSystem system;
    [SerializeField] private Weapons weapons;
    [SerializeField] private SpriteController sprCtrl;
    [SerializeField] private SpriteController weaponSprCtrl;
    [SerializeField] private BoxCollider2D weaponHitBox;
    [SerializeField] private Transform pistolBulletSpawn; // TODO change to array, so that shotgun can be implemented

    [SerializeField] private GameObject bullet;
    [SerializeField] private Animator knifeSlash;

    //0 - base, 1 - mafia, 2 - police TODO change to enum class
    private int outfit = 0;
    private int currentHealth;
    private int health = 1;
    private int activeWeapon;
    private bool isArmed = false;
    private bool canAttack = false;
    private bool attacking = false;
    private GameObject hoveredObj = null;
    //TODO Implement Ammo

    public bool isPaused = false;

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
        if (!isPaused)
        {
            //Attack Cooldown Timer
            if (!canAttack)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                    canAttack = true;
            }

            // Knife Collision Uptime Timer
            if (attacking && activeWeapon == 0)
            {
                knifeTimer -= Time.deltaTime;
                if (knifeTimer <= 0)
                {
                    weaponHitBox.enabled = false;
                    attacking = false;
                }
            }

            // Process user input
            Rotation();
            if (Input.GetKey(KeyCode.W)) Move(true, true); // Move Up
            if (Input.GetKey(KeyCode.S)) Move(true, false); // Move Down
            if (Input.GetKey(KeyCode.D)) Move(false, true); // Move Right
            if (Input.GetKey(KeyCode.A)) Move(false, false); // Move left

            if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(0); // Change Weapon: Knife
            else if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(1); // Change Weapon: Pistol
            else if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(2); // Change Weapon: Silenced Pistol
            else if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeWeapon(3); // Change Weapon: Shotgun
            else if (Input.GetKeyDown(KeyCode.E)) { } //TODO cycle through available weapons

            if (Input.GetKey(KeyCode.R)) Reload(); // Reload Weapon
            if (Input.GetKey(KeyCode.Q)) ActivateWeapon(false); // Put Away Weapon
            if (Input.GetMouseButtonDown(0) && canAttack) Attack(); // Attack... Duh

            if (Input.GetKeyDown(KeyCode.F) && hoveredObj) PickupHovered();
        }
    }

    #region Movement
    // Processes movement commands
    private void Move(bool isVertical, bool isPositive)
    {
        bool isUpdated = true;
        var pos = transform.position;
        var speedAdjustment = speed / ((isPositive) ? 1000 : -1000);
        var currCell = (isVertical) ? ((isPositive) ? (float)Math.Ceiling(pos.y) : (float)Math.Floor(pos.y)) : 
            ((isPositive) ? (float)Math.Ceiling(pos.x) : (float)Math.Floor(pos.x));

        // Check if the player's relative position is moving toward a new cell
        var relPos = currCell - ((isVertical) ? pos.y : pos.x);
        if((isPositive && relPos <= 0.3) || (!isPositive && relPos >= -0.3))
        {
            // Get the coordinates for the next cell
            Vector3 sub = new Vector3(0.0f, 0.0f, pos.z);
            if (isVertical)
            {
                sub.y = ((isPositive) ? currCell : currCell - 1);
                sub.x = pos.x;
            }
            else
            {
                sub.y = pos.y;
                sub.x = ((isPositive) ? currCell : currCell - 1);
            }

            // Check if the next cell is wall, if so don't move
            var cell = walls.WorldToCell(sub);
            var sprite = walls.GetSprite(cell);
            if (sprite)
            {
                isUpdated = false;
            }
        }

        // Add the movement
        if(isUpdated)
        {
            if (isVertical) 
                pos.y += speedAdjustment;
            else 
                pos.x += speedAdjustment;
        }

        //Set position to adjusted value
        transform.position = pos;
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
        if(!isArmed)
        {
            ActivateWeapon(true);
        }

        // Knife Attack Logic
        if(activeWeapon == 0)
        {
            // Set attack variables
            attackTimer = .5f;
            canAttack = false;

            // Set specific variables
            attacking = true;
            knifeTimer = .45f;
            weaponHitBox.enabled = true;
            knifeSlash.SetTrigger("Attack");
        }
        // Shooting Logic
        else
        {
            // Shotgun Attack
            if(activeWeapon == 3)
            {
                //Spawn bullet for each spawn point
                //TODO spawn bullets at different angles from bulletspawn
                /*foreach(var a in shotgunBulletSpawn)
                {
                    Instantiate(Bullet, a.position, a.rotation);
                }*/
            }
            // Pistol Attack
            else
            {
                //Spawn bullet from spawn point
                Instantiate(bullet, pistolBulletSpawn.position, pistolBulletSpawn.rotation);

                //TODO set canAttack and attackTimer

                // Sound wave creation
                if (activeWeapon != 1)
                    system.CreateSoundIndicator(this.gameObject, 3, true); // Not Silenced
                else
                    system.CreateSoundIndicator(this.gameObject, 1, true); // Silenced
            }
        }
    }

    // Enable/Disable the weapon sprite and armed state
    public void ActivateWeapon(bool Activate)
    {
        isArmed = Activate;
        weaponSprCtrl.Activate(Activate);
    }

    //Change current weapon
    private void ChangeWeapon(int choice)
    {
        // Check if weapon choice is available
        if(weapons.IsActive(choice))
        {
            // Set active weapon sprite
            activeWeapon = choice;
            weaponSprCtrl.Switch(activeWeapon);
        }
    }

    private void Reload()
    {
        //TODO: Implement Reloading Mechanics
        //TODO: Add Reload Animation
    }
    #endregion

    #region External Dealings
    //Called by enemy when seen
    //TODO rework with new external faction enum
    public bool LooksFriendly(bool isMafia)
    {
        bool temp = false;

        //Check if player outfit matches enemy faction
        if (isMafia && outfit == 1) temp = true;
        else if (!isMafia && outfit == 2) temp = true;

        //Check if player weapon is out
        if (isArmed) temp = false;
        
        //Equip weapon if seen as enemy
        if(!temp) ActivateWeapon(true);

        return temp;
    }
    
    // Pickup Interaction
    private void PickupHovered()
    {
        // Check if disguise or weapon
        var comp = hoveredObj.GetComponent<Pickup>();
        if (comp.IsDisguise)
        {
            // Switch sprites
            var current = sprCtrl.GetCurrent();
            sprCtrl.Switch(comp.Index);
            comp.Switch(current);
        }
        else
        {
            // Activate wepon choice
            weapons.SetActive(comp.Index);
            Destroy(hoveredObj);
            hoveredObj = null;
        }
    }

    // Called upon being to hit to inflict damage
    public void Damage()
    {
        // Decrement health and check for death
        currentHealth--;
        if(currentHealth == 0)
        {
            system.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check for collision with weapons
        if (collision.tag == "EnemyKnife") Damage();
        else if (collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            Damage();
        }
        else if (collision.tag == "Pickup")
        {
            hoveredObj = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Pickup")
        {
            hoveredObj = null;
        }
    }
    #endregion
}
