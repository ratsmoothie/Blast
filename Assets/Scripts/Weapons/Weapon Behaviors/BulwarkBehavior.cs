using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulwarkBehavior : MeleeController
{
    List<GameObject> enemiesTagged;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemiesTagged = new List<GameObject>();
    }

    //overrides our standard melee attack because this is not just a regular swing, it is a persistent aoe that should only hit each enemy once
    //Once our bulwark connects with an enemy call their TakeDamage() function to hit them for the weapon's current damage
    //Tag them and add them to a list, if said enemy is found in a list we cannot hit them again with the same instance of our bulwark
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        float printWeaponDamage = GetCurrentDamage();

        if(col.CompareTag("Enemy") && !enemiesTagged.Contains(col.gameObject))
        {
            base.hitSound.Play();
            EnemyStats target = col.GetComponent<EnemyStats>();
            target.TakeDamage(GetCurrentDamage());           
            //Debug.Log($"<color=cyan>{this.gameObject.name}</color>  hit <color=red>{col.gameObject.name}</color> for <color=yellow>{printWeaponDamage}</color> damage!");

            enemiesTagged.Add(col.gameObject);
        }
        else if(col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakableProp))
            {
                base.hitSound.Play();
                breakableProp.TakeDamage(GetCurrentDamage());
                //Debug.Log($"<color=cyan>{this.gameObject.name}</color> hit <color=red>{col.gameObject.name}</color> for <color=yellow>{printWeaponDamage}</color> damage!");
                
                enemiesTagged.Add(col.gameObject);
            }
        }
    }
}
