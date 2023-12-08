using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Components")]
    public WeaponScriptableObject weaponStats;

    //projectile orientation and how long they exist for
    protected Vector3 projectileDirection;
    public float durationUntilDestroy;

    //current weapon stats
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
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, durationUntilDestroy);
        //weaponStats = FindObjectOfType<WeaponScriptableObject>();
    }

    public void ProjectileDirectionFixer(Vector3 direction)
    {
        projectileDirection = direction;

        float directionX = projectileDirection.x;
        float directionY = projectileDirection.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        //default direction is right so no need to orient it for that one
        if (directionX < 0 && directionY == 0) //face left
        {
            scale.x *= -1;
            scale.y *= -1;
        }
        else if (directionX > 0 && directionY == 0) //face right
        {
            //default
        }
        else if (directionX == 0 && directionY < 0) //down
        {
            scale.y *= -1;
        }
        else if (directionX == 0 && directionY > 0) //up
        {
            scale.x *= -1;
        }
        else if (directionX > 0 && directionY > 0) //up-right
        {
            rotation.z = 0f;
        }
        else if (directionX > 0 && directionY < 0) //down-right
        {
            rotation.z = -90f;
        }
        else if (directionX < 0 && directionY > 0) //up-left
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = -90f;
        }
        else if (directionX < 0 && directionY < 0) //down-left
        {
            scale.x *= -1;
            scale.y *= -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation); //conversion
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Enemy"))
        {
            EnemyStats target = col.GetComponent<EnemyStats>(); //get the stats of the enemy we hit
            target.TakeDamage(currentDamage); //cal the target's TakeDamage() function and pass in our current weapon damage
            Debug.Log($"<color=cyan>{this.gameObject.name}</color> hit <color=red>{col.gameObject.name}</color> for <color=yellow>{currentDamage}</color> damage!");
            ReduceCleaveNum();
        }
        else if(col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out BreakableProps breakableProp))
            {
                breakableProp.TakeDamage(currentDamage);
                Debug.Log($"<color=cyan>{this.gameObject.name}</color> hit <color=red>{col.gameObject.name}</color> for <color=yellow>{currentDamage}</color> damage!");
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
}
