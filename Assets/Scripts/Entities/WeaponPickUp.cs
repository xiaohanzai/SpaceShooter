using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : PickUp
{
    [SerializeField] protected Weapon newWeapon;

    protected override void PickMe(Character character)
    {
        character.ChangeWeapon(newWeapon);
        base.PickMe(character);
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
