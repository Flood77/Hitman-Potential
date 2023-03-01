using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour 
{
    [SerializeField] public float attackTimer { get; }
    [SerializeField] public float attackDistance { get; }

    public virtual void Timers() { }
    public virtual bool Attack(float distance) { return false; }
}
