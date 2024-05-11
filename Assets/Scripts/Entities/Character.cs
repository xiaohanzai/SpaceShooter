using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemies and Players will derive from this class because all of them will have these characteristics in common
public abstract class Character : MonoBehaviour, IDamageable
{
    [SerializeField] private float force = 2f;

    private float speed;
    private int strength;
    protected Health healthPoints;

    protected Rigidbody2D rb;

    [SerializeField] protected AudioSource killedAudio;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public abstract void Attack();

    public abstract void Die();

    public virtual void Move(Vector2 direction, float angle)
    {
        rb.AddForce(force * Time.deltaTime * direction.normalized * 500f);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public abstract void ReceiveDamage();

    public void ReceiveDamage(int damage)
    {
        healthPoints.DecreaseLife(damage);
    }

    public abstract void ChangeHealth();

    public virtual void ChangeWeapon(Weapon weapon) { }

    protected void PlayAudio(AudioSource audioSource)
    {
        AudioSource audio = Instantiate(audioSource, transform.position, Quaternion.identity);
        audio.Play();
        Destroy(audio.gameObject, 3f);
    }
}
