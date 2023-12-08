using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//spawns and orients the spear
public class SpearController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedSpear = Instantiate(weaponStats.WeaponPrefab);
        spawnedSpear.transform.position = transform.position; //ensure position is on the player
        spawnedSpear.GetComponent<SpearBehavior>().ProjectileDirectionFixer(playerController.directionLastMoved); //set the projectile's direction to the player's movement vector
    }
}
