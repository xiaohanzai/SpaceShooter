using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    public int damage;
    public float rangeOfExplosion;
    [SerializeField] private AudioSource explodeAudio;

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
        PlayAudio(explodeAudio);
        if (ParticleManager.Instance != null)
        {
            ParticleManager.Instance.GetExplodeParticles(transform.position);
        }
        Die();
    }
}
