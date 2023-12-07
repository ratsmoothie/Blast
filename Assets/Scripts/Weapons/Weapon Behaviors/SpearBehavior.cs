using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBehavior : ProjectileController
{

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //sends the spear in the specified direction at a set speed
        transform.position += projectileDirection * weaponStats.speed * Time.deltaTime;
    }
}
