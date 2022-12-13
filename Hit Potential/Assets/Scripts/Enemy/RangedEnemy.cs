﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    #region Variables
    [SerializeField] private float angle = 15;
    [SerializeField] private int numRaycast = 6;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    #endregion

    //Returns a list of all GameObjects seen by the enemy
    protected override GameObject[] GetGameObjects()
    {
        var gameObjects = new List<GameObject>();

        //Produce raycasts through given values
        var angleOffset = (angle * 2) / (numRaycast - 1);
        for (int i = 0; i < numRaycast; i++)
        {
            //Adjust direction for needed rotation
            var rotation = Quaternion.AngleAxis(-angle + (angleOffset * i), Vector3.forward);
            Vector2 up = rotation * transform.right;

            //Produce raycast in front of the enemy
            var d = visionDistance;
            var ray = Physics2D.Raycast(transform.position, up, visionDistance);
            if (ray)
            {
                //Add gameobject to list if collided with raycast
                d = ray.distance;
                gameObjects.Add(ray.collider.gameObject);
            }
            Debug.DrawRay(transform.position, up * d, Color.red);
        }

        return gameObjects.ToArray();
    }

    //Adjusts timers based on time passed
    protected override void Timers()
    {
        //Adjust attackTimer and change canAttack accordingly
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            canAttack = true;
        }
    }

    protected override void Attack()
    {
        //Find distance between player and enemy
        var playerPosition = new Vector3(nav.velocity.x, nav.velocity.y, 0);
        var distance = Vector3.Distance(playerPosition, gameObject.transform.position);

        //If within distance then check timer
        if (distance <= 12 && canAttack)
        {
            //Reset timer
            attackTimer = .25f;
            canAttack = false;

            //Spawn bullet from spawn point
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

            //TODO Sound Indicators
        }
    }
}