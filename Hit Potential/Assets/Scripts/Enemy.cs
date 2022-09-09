using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //TODO: Make parent class to both a melee and ranged enemy
    //TODO: Detect Player that is in combat or not disguised when in view
    //TODO: Attack detected Player or get in range to do so
    //TODO: After x time lose player and go back to patrol
    //TODO: Search for Player on viewing of corpse
    protected enum state
    {
        Patrol,
        Follow,
        Search
    }

    public GameObject nodePrefab;
    public Node node;
    [SerializeField] private state currentState;

    [SerializeField] private float speedMax = 2;
    [SerializeField] private float accelerationMax = 2;
    [SerializeField] private float turnRate = 10;
    [SerializeField] protected float visionDistance = 5;
    
    private float timer;
    private Vector2 lastSeenPosition;

    protected virtual Vector3 Velocity { get; set; }
    protected virtual Vector3 Acceleration { get; set; }

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

        if (currentState == state.Patrol)
        {
            MoveTowards(node.transform.position);
            if(player != null) 
            { 
                Stop(); 
                currentState = state.Follow; 
            }
            else if(corpse != null)
            {
                Stop();
                currentState = state.Search;
            }
        }
        else if(currentState == state.Follow)
        {
            if (player != null)
            {
                lastSeenPosition = player.transform.position;
                timer = 1;
                var diff = player.transform.position - transform.position;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            }

            MoveTowards(lastSeenPosition);

            if (player == null)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    currentState = state.Patrol;
                }
            }
        }
        else if(currentState == state.Search)
        {

        }
    }

    float yep = 1;

    private void LateUpdate()
    {
        Velocity += Acceleration * ((currentState == state.Follow) ? 1.0f : Time.deltaTime);
        Velocity = Vector3.ClampMagnitude(Velocity, speedMax);
        transform.position += Velocity * Time.deltaTime;

        if (Velocity.normalized.magnitude > 0.1f)
        {
            if (currentState != state.Follow)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg);
            }
        }

        Acceleration = Vector3.zero;
    }

    public void MoveTowards(Vector3 target)
    {
        ApplyForce((target - transform.position).normalized * accelerationMax);
    }
    public void ApplyForce(Vector3 force)
    {
        Acceleration += force;
        Acceleration = Vector3.ClampMagnitude(Acceleration, accelerationMax);
    }
    public void Stop()
    {
        Velocity = Vector3.zero;
    }


    

    protected virtual GameObject[] GetGameObjects() { return null; }

    protected virtual void Attack() { }
}
