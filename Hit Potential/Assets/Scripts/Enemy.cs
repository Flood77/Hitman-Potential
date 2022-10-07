using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private float timer;
    private eState currentState;
    private Vector2 lastSeenPosition;
    private Vector2 posDiff;

    public Node Node { get { return node; } set { node = value; } } 
    private Vector3 Velocity { get; set; }
    private Vector3 Acceleration { get; set; }
    #endregion

    private void Update()
    {
        GameObject player = null;
        GameObject corpse = null;
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
            MoveTowards(node.transform.position);
            if (player != null)
            {
                Stop();
                currentState = eState.Follow;
            }
            else if (corpse != null)
            {
                Stop();
                currentState = eState.Search;
            }
        }
        else if (currentState == eState.Follow)
        {
            if (player != null)
            {
                lastSeenPosition = player.transform.position;
                timer = 2;
                posDiff = player.transform.position - transform.position;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg);
            }

            MoveTowards(lastSeenPosition);
            Timers();
            Attack();
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
            var a = Random.Range(-180, 180);

            //TODO: Fix Search Look Rotation to turn randomly

            lastSeenPosition += posDiff;
            var rot = Quaternion.Euler(0f, 0f, Mathf.Atan2(posDiff.y, posDiff.x) * Mathf.Rad2Deg);
            rot.z += 45;
            transform.rotation = rot;
            MoveTowards(lastSeenPosition);

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
        Velocity += Acceleration * ((currentState == eState.Follow) ? 1.0f : Time.deltaTime);
        Velocity = Vector3.ClampMagnitude(Velocity, speedMax);
        transform.position += Velocity * Time.deltaTime;

        if (Velocity.normalized.magnitude > 0.1f)
        {
            if (currentState != eState.Follow)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg);
            }
        }

        Acceleration = Vector3.zero;
    }
    public void MoveTowards(Vector3 target)
    {
        Acceleration += (target - transform.position).normalized * accelerationMax;
        Acceleration = Vector3.ClampMagnitude(Acceleration, accelerationMax);
    }
    public void Stop()
    {
        Velocity = Vector3.zero;
    }
    #endregion

    #region virtuals
    protected virtual GameObject[] GetGameObjects() { return null; }
    protected virtual void Attack() { }
    #endregion

    public void Die()
    {
        Instantiate(deadBody, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

    protected virtual void Timers() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Knife")
        {
            Die();
        }
    }
}
