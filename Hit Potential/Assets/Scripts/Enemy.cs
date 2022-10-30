using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected enum eState
    {
        Patrol,
        Follow,
        Search
    }

    #region Properties and Fields

    [SerializeField] private Node node;
    [SerializeField] private float speedMax = 2;
    [SerializeField] private float accelerationMax = 2;
    [SerializeField] private GameObject deadBody;
    [SerializeField] protected float visionDistance = 5;
    [SerializeField] protected float turnRate = 1;
    
    private float timer;
    private eState currentState;
    private Vector2 posDiff;
    private Vector3 Velocity;
    private Vector3 Acceleration;

    protected NavMeshAgent nav;
    protected float attackTimer;
    protected Vector2 lastSeenPosition;

    public Node Node { get { return node; } set { node = value; } }

    #endregion

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.updateUpAxis = false;
    }

    private void Update()
    {
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
        if (nav.isStopped) Resume();

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
            //Rotate to look at player
            if (player != null)
            {
                NavMoveTowards(player.transform.position);
                timer = 2;
                lastSeenPosition = player.transform.position;
                posDiff = player.transform.position - transform.position;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(nav.velocity.y, nav.velocity.x) * Mathf.Rad2Deg);
            }

            //Run attack countdowns and attack logic
            Timers();
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
            //TODO: Fix yes...
            var a = Random.Range(-180, 180);

            //TODO: Fix Search Look Rotation to turn randomly

            lastSeenPosition += posDiff;
            var rot = Quaternion.Euler(0f, 0f, Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg);
            rot.z += 45;
            transform.rotation = rot;
            NavMoveTowards(lastSeenPosition);

            timer -= Time.deltaTime;
            if(player != null)
            {
                timer = 2;
                currentState = eState.Follow;
            }
            else if(timer <= 0)
            {
                currentState = eState.Patrol;
            }
        }
    }

    #region Movement Logic
    private void LateUpdate()
    {
        //Navmesh Rotation
        if(nav.velocity.normalized.magnitude > 0.1f)
        {
            if(currentState == eState.Patrol)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(nav.velocity.y, nav.velocity.x) * Mathf.Rad2Deg);
            }
        }

        //Manual Movement 
        { 
            /*
            Velocity += Acceleration * ((currentState == eState.Follow) ? 1.0f : Time.deltaTime);
            Velocity = Vector3.ClampMagnitude(Velocity, speedMax);
            transform.position += Velocity * Time.deltaTime;

            if (Velocity.normalized.magnitude > 0.1f)
            {
                if (currentState == eState.Patrol)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg);
                }
            }

            Acceleration = Vector3.zero;
            */
        }
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

    //Manual Movement Processes
    public void ManualMoveTowards(Vector3 target)
    {
        Acceleration += (target - transform.position).normalized * accelerationMax;
        Acceleration = Vector3.ClampMagnitude(Acceleration, accelerationMax);
    }
    public void ManualStop()
    {
        Velocity = Vector3.zero;
    }
    #endregion

    #region virtuals
    protected virtual GameObject[] GetGameObjects() { return null; }
    protected virtual void Attack() { }
    protected virtual void Timers() { }
    #endregion

    public void Die()
    {
        Instantiate(deadBody, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
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
}
