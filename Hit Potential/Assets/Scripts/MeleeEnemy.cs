using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public float angle = 15;
    [Min(2)] public int numRaycast = 6;

    [SerializeField] private BoxCollider2D weaponHitBox;
    [SerializeField] private Animation knifeSlash;
    [SerializeField] private Animator knifeAnim;
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

    private bool canAttack = false;
    protected override void Timers()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
        {
            canAttack = true;
        }

        if (!knifeAnim.GetCurrentAnimatorStateInfo(0).IsName("EnemyKnifeSlice"))
        {
            weaponHitBox.enabled = false;
        }

    }

    protected override void Attack()
    {
        var playerPosition = new Vector3(nav.velocity.x, nav.velocity.y, 0);
        var distance = Vector3.Distance(playerPosition, gameObject.transform.position);

        if (distance <= 0.1)
        {
            if (canAttack)
            {
                attackTimer = .75f;
                weaponHitBox.enabled = true;
                canAttack = false;

                knifeSlash.Play("EnemyKnifeSlice");
            }
        }
    }
}
