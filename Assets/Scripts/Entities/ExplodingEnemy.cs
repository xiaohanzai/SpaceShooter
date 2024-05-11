using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    public int damage;
    public float rangeOfExplosion;

    protected override void Start()
    {
        base.Start();
    }

    public override void Attack()
    {
        if (Vector2.Distance(target.transform.position, transform.position) < rangeOfExplosion)
        {
            target.ReceiveDamage(damage);
        }
        Die();
    }
}
