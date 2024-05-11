using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    private string targetTag;
    private int damage;
    //[SerializeField] private AudioSource shootAudioSource;
    //[SerializeField] private AudioSource hitAudioSource;

    private void Start()
    {
        //shootAudioSource.Play();
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }

    public void SetUpBullet(string newTag, int newDamage)
    {
        targetTag = newTag;
        damage = newDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == targetTag)
        {
            collision.gameObject.GetComponent<IDamageable>().ReceiveDamage(damage);
            //hitAudioSource.Play();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == targetTag)
        {
            collision.GetComponent<IDamageable>().ReceiveDamage(damage);
            //hitAudioSource.Play();
            Destroy(gameObject);
        }
    }
}
