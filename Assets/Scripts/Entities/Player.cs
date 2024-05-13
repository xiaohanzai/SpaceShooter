using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Character
{
    [SerializeField] private int initHealthPoints = 10;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Weapon playerWeapon;
    private Weapon _playerWeapon;

    private Queue<NukePickUp> nukes = new Queue<NukePickUp>(); // TODO: I think it's gonna be empty... but I don't want to deal with it

    public UnityEvent OnPlayerDied;
    public UnityEvent OnHealthChanged;
    public UnityEvent OnNukesChanged;
    public UnityEvent OnNukeHappened;
    public UnityEvent OnHighSpeedShootingEnabled;
    public UnityEvent OnHighSpeedShootingDisabled;

    private Coroutine co_healthIncrease;

    private bool canDoHighSpeedShooting;

    protected override void Start()
    {
        base.Start();
        healthPoints = new Health(initHealthPoints);
        healthPoints.OnHealthDamaged.AddListener(ChangeHealth);
        healthPoints.OnHealthIncreased.AddListener(ChangeHealth);
        co_healthIncrease = StartCoroutine(GraduallyIncreaseHealth());
        _playerWeapon = playerWeapon;
    }

    private void Update()
    {

    }
    
    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        playerWeapon = _playerWeapon;
        healthPoints.currentHealth = initHealthPoints;
        nukes.Clear();
        GetComponent<PlayerInput>().Reset();
        co_healthIncrease = StartCoroutine(GraduallyIncreaseHealth());
    }

    public override void Attack()
    {
        playerWeapon.Shoot(bulletSpawnPoint.position, bulletSpawnPoint.rotation, "Enemy");
    }

    public override void Die()
    {
        OnPlayerDied.Invoke();
        PlayAudio(killedAudio);
        if (ParticleManager.Instance != null)
        {
            ParticleManager.Instance.GetKilledParticles(transform.position);
        }
        StopCoroutine(co_healthIncrease);
        co_healthIncrease = null;

        gameObject.SetActive(false);
    }

    public override void ReceiveDamage()
    {
        healthPoints.DecreaseLife();
    }

    public override void Move(Vector2 direction, float angle)
    {
        base.Move(direction, angle);
    }

    public override void ChangeHealth()
    {
        OnHealthChanged.Invoke();
        if (healthPoints.currentHealth <= 0)
        {
            Die();
        }
    }

    public override void ChangeWeapon(Weapon weapon)
    {
        playerWeapon = weapon;
    }

    IEnumerator GraduallyIncreaseHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(3);
            healthPoints.IncreaseLife(1);
        }
    }

    public int GetCurrentHealth()
    {
        return healthPoints.currentHealth;
    }

    public int GetNumOfNukes()
    {
        return nukes.Count;
    }

    public void AddNuke(NukePickUp nuke)
    {
        nukes.Enqueue(nuke);
        OnNukesChanged.Invoke();
    }

    private void UseOneNuke()
    {
        nukes.Dequeue();
        OnNukesChanged.Invoke();
    }

    public void Nuke()
    {
        if (nukes.Count > 0)
        {
            UseOneNuke();
            OnNukeHappened.Invoke();
        }
    }

    public void EnhancedAttack()
    {
        if (canDoHighSpeedShooting)
        {
            Attack();
        }
    }

    public void EnableHighSpeedShooting()
    {
        canDoHighSpeedShooting = true;
        OnHighSpeedShootingEnabled.Invoke();
        GetComponent<PlayerInput>().ResetHighSpeedShootingTimer();
    }

    public void DisableHighSpeedShooting()
    {
        canDoHighSpeedShooting = false;
        OnHighSpeedShootingDisabled.Invoke();
    }

    public bool IsHighSpeedShootingEnabled()
    {
        return canDoHighSpeedShooting;
    }
}
