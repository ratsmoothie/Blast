using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatswordController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedGreatsword = Instantiate(weaponStats.WeaponPrefab);
        spawnedGreatsword.transform.position = transform.position; //ensure position is on the player
        spawnedGreatsword.GetComponent<GreatswordBehavior>().ProjectileDirectionFixer(transform.up); //throw it up, gravity will pull it down
    }
}
