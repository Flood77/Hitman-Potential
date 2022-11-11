using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] private float speed = 10;
    [SerializeField] private float knifeTimer = 0; 
    [SerializeField] private float attackTimer = 0; 

    [SerializeField] private GSystem system; 
    [SerializeField] private Weapons weapons;
    [SerializeField] private SpriteController sprCtrl;
    [SerializeField] private SpriteController weaponSprCtrl;
    [SerializeField] private BoxCollider2D weaponHitBox;
    [SerializeField] private Transform pistolBulletSpawn;
    [SerializeField] private Transform[] shotgunBulletSpawn;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private Animation knifeSlash;
    [SerializeField] private Animator knifeAnim;

    //0 - base, 1 - mafia, 2 - police
    private int outfit = 0;
    private int currentHealth;
    private int health = 1;
    private int activeWeapon;
    private bool inCombat = false;
    private bool canAttack = false;
    //Implement Ammo

    public string Health { get { return currentHealth + " / " + health; } }
    #endregion

    private void Start()
    {
        //set starting health
        currentHealth = health;

        //Check for first active weapon and equip it
        if (weapons.IsActive(0)) { activeWeapon = 0; }
        else if (weapons.IsActive(1)) { activeWeapon = 1; }
    }

    void Update()
    {
        //Attack Timer and Knife Activation Timer
        attackTimer -= Time.deltaTime;
        knifeTimer -= Time.deltaTime;
        if(attackTimer <= 0)
        {
            canAttack = true;
        }
        if(knifeTimer <= 0)
        {
            weaponHitBox.enabled = false;
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
            var temp = transform.position;
            temp.y += speed;
            transform.position = temp;
        }
        //Move down
        if (Input.GetKey(KeyCode.S))
        {
            var temp = transform.position;
            temp.y -= speed;
            transform.position = temp;
        }
        //Move right
        if (Input.GetKey(KeyCode.D))
        {
            var temp = transform.position;
            temp.x += speed;
            transform.position = temp;
        }
        //Move left
        if (Input.GetKey(KeyCode.A))
        {
            var temp = transform.position;
            temp.x -= speed;
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

                knifeTimer = .45f;
                knifeSlash.Play();
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
        if (collision.tag == "EnemyKnife")
        {
            Damage();
        }
    }
    #endregion
}
