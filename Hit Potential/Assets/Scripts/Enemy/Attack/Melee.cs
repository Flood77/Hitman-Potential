using UnityEngine;

public class Melee : EnemyAttack
{
    [SerializeField] private Animator knifeSlash;
    [SerializeField] private BoxCollider2D weaponHitBox;

    protected float knifeTimer;
    private bool attacking = false;
    //AttackTimer = .75f

    public override void Timers()
    {
        //If doing attack animation run timer, till end of animation
        if (attacking)
        {
            knifeTimer -= Time.deltaTime;
            if (knifeTimer <= 0)
            {
                weaponHitBox.enabled = false;
                attacking = false;
            }
        }
    }

    public override bool Attack(float distance)
    {
        //If within distance then check timer
        if (distance <= .75)
        {
            //play animation & enable collision
            attacking = true;
            knifeTimer = .45f;
            weaponHitBox.enabled = true;
            knifeSlash.SetTrigger("Attack");

            return true;
        }

        return false;
    }
}
