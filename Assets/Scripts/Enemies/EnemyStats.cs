using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyStats;

    //enemy's current stats
    [HideInInspector]
    public float currentMovementSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    void Awake()
    {
        //before start each enemy is assigned their regular values
        //doing it this way in case something changes an enemy's stats which I'll handle in update()
        currentMovementSpeed = enemyStats.MovementSpeed;
        currentHealth = enemyStats.MaxHealth;
        currentDamage = enemyStats.Damage;
    }

    public void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;

        if(currentHealth <= 0)
        {
            KillThisEnemy();
        }
    }

    public void KillThisEnemy()
    {
        //add some flair here later

        Destroy(this.gameObject);
    }

    //Deal damage to the player while in contact
    private void OnCollisionStay2D(Collision2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collider.gameObject.GetComponent<PlayerStats>();
            // Check if the player object and the PlayerStats component are not null
            if (player != null)
            {
                player.TakeDamage(currentDamage);
            }
            else
            {
                Debug.LogWarning("PlayerStats component not found on the player object.");
            }
        }
    }
}
