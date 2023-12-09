using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    [Header("Components")]
    public WeaponScriptableObject weaponStats;
    PlayerStats player;
    public AudioSource hitSound;

    public float durationUntilDestroy;

    //weapon's current stats
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownTotal;
    protected int currentCleaveNum;

    //set all stats of the weapon to defaults before start()
    //we can change them during play as the weapons level
    private void Awake()
    {
        currentDamage = weaponStats.Damage;
        currentSpeed = weaponStats.Speed;
        currentCooldownTotal = weaponStats.CooldownTotal;
        currentCleaveNum = weaponStats.CleaveNum;

        player = FindObjectOfType<PlayerStats>();
        hitSound = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, durationUntilDestroy);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            hitSound.Play();
            EnemyStats target = col.GetComponent<EnemyStats>(); //get the stats of the enemy we hit
            target.TakeDamage(GetCurrentDamage()); //cal the target's TakeDamage() function and pass in our current weapon damage
            Debug.Log($"<color=cyan>{this.gameObject.name}</color>  hit <color=red>{col.gameObject.name}</color> for <color=yellow>{GetCurrentDamage()}</color> damage!");
            ReduceCleaveNum();
        }
        else if(col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakableProp))
            {
                hitSound.Play();
                breakableProp.TakeDamage(GetCurrentDamage());
                Debug.Log($"<color=cyan>{this.gameObject.name}</color> hit <color=red>{col.gameObject.name}</color> for <color=yellow>{GetCurrentDamage()}</color> damage!");
                ReduceCleaveNum();
            }
        }
    }

    void ReduceCleaveNum() //Ends our attack once it can cleave no more enemies
    {
        currentCleaveNum--;

        if(currentCleaveNum <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= player.currentAttackPower;
    }

}
