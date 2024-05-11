using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPoint;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        FaceTarget();
    }

    public override void Attack()
    {
        float angle = Random.Range(-30f, 30f);
        weapon.Shoot(bulletSpawnPoint.position, bulletSpawnPoint.rotation * Quaternion.Euler(0, 0, angle), "Player");
    }
}
