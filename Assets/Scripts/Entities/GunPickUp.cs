using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : WeaponPickUp
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().EnableHighSpeedShooting();
            base.PickMe(collision.gameObject.GetComponent<Character>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
