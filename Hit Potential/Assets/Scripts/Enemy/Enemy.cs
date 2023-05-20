using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    protected enum eState
    {
        Patrol,
        Follow,
        Search
    }

    #region Properties and Fields
    [SerializeField] private float speedMax = 2;
    [SerializeField] protected float turnRate = 1;
    [SerializeField] private float accelerationMax = 2;
    [SerializeField] protected float visionDistance = 5;

    [SerializeField] private float angle = 15;
    [SerializeField] private int numRaycast = 6;

    [SerializeField] protected NavMeshAgent nav;
    [SerializeField] private EnemyAttack ea;
    [SerializeField] private Movement move;

    [SerializeField] private Node node;
    [SerializeField] private GameObject deadBody;
    
    private float timer;
    private eState currentState;
    private Vector2 posDiff;

    protected bool canAttack = false;
    protected float attackTimer;
    protected Vector2 lastSeenPosition;

    public Node Node { get { return node; } set { node = value; } }

    #endregion

    private void Start()
    {
        //Keeps nav from rotating agent to the wrong dimension
        nav.updateUpAxis = false;
    }

    private void Update()
    {
        //Set nav settings
        nav.speed = speedMax;
        nav.angularSpeed = turnRate;

        GameObject player = null;
        GameObject corpse = null;
        //Check Sight Line for player or corpse
        foreach (var g in GetGameObjects())
        {
            if (g.CompareTag("Player"))
            {
                player = g;
                break;
            }
            else if (g.CompareTag("Corpse"))
            {
                corpse = g;
            }
        }

        if (currentState == eState.Patrol)
        {
            //Follow nodes
            NavMoveTowards(node.transform.position);

            //Change state if finding player or corpse
            if (player != null)
            {
                NavStop();
                currentState = eState.Follow;
            }
            else if (corpse != null)
            {
                NavStop();
                currentState = eState.Search;
            }
        }
        else if (currentState == eState.Follow)
        {
            FollowRotate(player);

            Attack();

            //Count down to Search mode
            if (player == null)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 5;
                    currentState = eState.Search;
                }
            }
        }
        else if (currentState == eState.Search)
        {
            SearchRotate();

            timer -= Time.deltaTime;
            if(player != null)
            {
                timer = 2;
                currentState = eState.Follow;
            }
            else if(timer <= 0)
            {
                currentState = eState.Patrol;
                Resume();
            }
        }
    }
      
    #region Movement Logic
    //Rotation Processes
    private void LateUpdate()
    {
        //Navmesh Rotation
        if(nav.velocity.normalized.magnitude > 0.1f)
        {
            //Rotates to face movement direction when on Patrol
            if(currentState == eState.Patrol)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(nav.velocity.y, nav.velocity.x) * Mathf.Rad2Deg);
            }
        }
    }
    private void FollowRotate(GameObject player)
    {
        //Rotate to look at player
        if (player != null)
        {
            NavMoveTowards(player.transform.position);
            timer = 2;
            lastSeenPosition = player.transform.position;
            posDiff = player.transform.position - transform.position;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(nav.velocity.y, nav.velocity.x) * Mathf.Rad2Deg);
        }
    }
    private void SearchRotate()
    {
        //TODO: Fix yes...
        var a = Random.Range(-180, 180);

        //TODO: Fix Search Look Rotation to turn randomly

        lastSeenPosition += posDiff;
        var rot = Quaternion.Euler(0f, 0f, Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg);
        rot.z += 45;
        transform.rotation = rot;
        NavMoveTowards(lastSeenPosition);
    }

    //Navmesh Movement Processes
    public void NavMoveTowards(Vector3 target)
    {
        nav.SetDestination(target);
    }
    public void NavStop()
    {
        nav.isStopped = true;
    }
    public void Resume()
    {
        nav.isStopped = false;
    }
    #endregion

    #region World Interaction
    //Returns a list of all GameObjects seen by the enemy
    private GameObject[] GetGameObjects()
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

    //Runs all processes related to given EnemyAttack class
    private void Attack()
    {
        //Adjust attackTimer and change canAttack accordingly
        ea.Timers();
        if (!canAttack)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                canAttack = true;
            }
        }
        else
        {
            //Find distance between player and enemy
            var distance = Vector3.Distance(new Vector3(nav.velocity.x, nav.velocity.y, 0), gameObject.transform.position);

            if (ea.Attack(distance))
            {
                canAttack = false;
                attackTimer = ea.attackTimer;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Death by Knife
        if(collision.tag == "Knife")
        {
            Die();
        }
        //Collision with Sound Indicator
        else if(collision.tag == "Sound")
        {
            var playerMade = collision.gameObject.GetComponent<SoundIndicator>().fromPlayer;

        }
    }
    #endregion

    //Destroy enemy and make dead body
    public void Die()
    {
        Instantiate(deadBody, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
}
