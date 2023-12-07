using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulwarkController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject spawnedBulwark = Instantiate(weaponStats.weaponPrefab);
        spawnedBulwark.transform.position = transform.position; //stick to the player
        spawnedBulwark.transform.parent = transform;
    }
}
