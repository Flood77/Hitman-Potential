using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    public Transform raycastTransform;
    public float angle = 15;
    [Min(2)] public int numRaycast = 6;

    protected override GameObject[] GetGameObjects()
    {
        var gameObjects = new List<GameObject>();

        var angleOffset = (angle * 2) / (numRaycast - 1);
        for (int i = 0; i < numRaycast; i++)
        {
            var rotation = Quaternion.AngleAxis(-angle + (angleOffset * i), Vector3.forward);
            Vector2 up = rotation * transform.right;

            var d = visionDistance;
            var ray = Physics2D.Raycast(transform.position, up, visionDistance);
            if (ray)
            {
                d = ray.distance;
                gameObjects.Add(ray.collider.gameObject);
            }
            Debug.DrawRay(transform.position, up * d, Color.red);
        }

        return gameObjects.ToArray();
    }

    protected override void Attack()
    {

    }
}