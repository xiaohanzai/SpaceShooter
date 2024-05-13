using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Character
{
    [SerializeField] protected Weapon weapon;
    [SerializeField] private int health;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float attackTimer;
    private float timer;
    protected Player target;

    [SerializeField] protected PickUp pickUpPrefab;

    public UnityEvent<Enemy> OnEnemyDied;
    public UnityEvent OnEnemyKilled;

    protected override void Start()
    {
        base.Start();
        timer = 0;
        target = FindObjectOfType<Player>();
        healthPoints = new Health(health);
        healthPoints.OnHealthDamaged.AddListener(ChangeHealth);
    }

    protected virtual void FixedUpdate()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            Vector2 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Move(direction, angle);
        }
    }

    public void FaceTarget()
    {
        if (target != null && target.gameObject.activeInHierarchy)
        {
            // Get the direction from this object to the target
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize(); // Make sure the direction is normalized

            // Calculate the angle in radians
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate this object to face the target
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    public override void Attack()
    {
        target.ReceiveDamage();
    }

    public override void Die()
    {
        OnEnemyDied.Invoke(this);
        if (healthPoints.currentHealth <= 0)
        {
            if (pickUpPrefab != null)
            {
                Instantiate(pickUpPrefab, transform.position, Quaternion.identity);
            }
            PlayAudio(killedAudio);
            OnEnemyKilled.Invoke();
            if (ParticleManager.Instance != null)
            {
                ParticleManager.Instance.GetKilledParticles(transform.position);
            }
        }
        Destroy(gameObject);
    }

    public override void ReceiveDamage()
    {
        healthPoints.DecreaseLife();
    }

    public override void Move(Vector2 direction, float angle)
    {
        if (Vector2.Distance(target.transform.position, transform.position) > attackDistance)
        {
            base.Move(direction, angle);
            timer = 0;
        }
        else
        {
            //rb.velocity = Vector2.zero;
            if (timer < attackTimer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                Attack();
                timer = 0;
            }
        }
    }

    public override void ChangeHealth()
    {
        if (healthPoints.currentHealth <= 0)
        {
            Die();
        }
    }
}
