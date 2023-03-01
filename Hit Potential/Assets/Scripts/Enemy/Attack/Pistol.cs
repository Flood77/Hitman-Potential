using UnityEngine;

public class Pistol : EnemyAttack
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawn;
    //attackTimer = .25f

    public override void Timers() { base.Timers(); }
    public override bool Attack(float distance)
    {
        //if within distance then check timer
        if (distance <= 12)
        {
            //Spawn bullet from spawn point
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);

            //TODO Sound Indicators

            return true;
        }

        return false;
    }
}
