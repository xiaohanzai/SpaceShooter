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

    public void Shoot(Vector2 position, Quaternion rotation, string tag)
    {
        Bullet tempBullet = GameObject.Instantiate(bulletReference, position, rotation);
        tempBullet.SetUpBullet(tag, damage); 
    }
}
