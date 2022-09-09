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

    [SerializeField] public Node node;
    [SerializeField] private state currentState;

    public float speedMax = 2;
    public float accelerationMax = 2;
    public float turnRate = 10;
    public float visionDistance = 1;

    public virtual Vector3 Velocity { get; set; }
    public virtual Vector3 Acceleration { get; set; }

    private void Update()
    {
        GameObject player = null;
        foreach (var g in GetGameObjects())
        {
            if (g.CompareTag("Player"))
            {
                player = gameObject;
                break;
            }
        }

        if (currentState == state.Patrol)
        {
            MoveTowards(node.transform.position);
            if(player != null) { currentState = state.Follow; }
        }
        else if(currentState == state.Follow)
        {
            Stop();
        }
        else if(currentState == state.Search)
        {

        }
    }

    private void LateUpdate()
    {
        Velocity += Acceleration * Time.deltaTime;
        Velocity = Vector3.ClampMagnitude(Velocity, speedMax);
        transform.position += Velocity * Time.deltaTime;

        if (Velocity.normalized.magnitude > 0.1f)
        {
            var r = GetComponent<Rigidbody2D>();
            var angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
            r.MoveRotation(angle);
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
