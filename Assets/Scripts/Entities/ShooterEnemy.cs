using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private AudioSource laserAudio;

    private bool isPreparingToShoot;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (target != null && target.gameObject.activeInHierarchy)
        {
            FaceTarget();
            if (Vector2.Distance(target.transform.position, transform.position) < attackDistance)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, bulletSpawnPoint.position);
                lineRenderer.SetPosition(1, target.transform.position);
                if (!isPreparingToShoot)
                {
                    laserAudio.Play();
                    isPreparingToShoot = true;
                }
            }
            else
            {
                lineRenderer.enabled = false;
                isPreparingToShoot = false;
            }
        }
    }

    public override void Attack()
    {
        weapon.Shoot(bulletSpawnPoint.position, bulletSpawnPoint.rotation, "Player");
        isPreparingToShoot = false;
    }
}
