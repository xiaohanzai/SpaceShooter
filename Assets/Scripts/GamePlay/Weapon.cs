using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Weapon")]
public class Weapon : ScriptableObject
{
    //[SerializeField] private string weaponName;
    //[SerializeField] private Sprite icon;
    [SerializeField] private Bullet bulletReference;
    [SerializeField] private int damage;
    [SerializeField] private AudioSource shootAudio;

    public void Shoot(Vector2 position, Quaternion rotation, string tag)
    {
        Bullet tempBullet = GameObject.Instantiate(bulletReference, position, rotation);
        tempBullet.SetUpBullet(tag, damage);
        AudioSource audio = Instantiate(shootAudio, tempBullet.transform.position, Quaternion.identity);
        audio.Play();
        Destroy(audio.gameObject, 5f);
    }
}
