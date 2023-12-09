using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components")]
    //public EnemyScriptableObject enemyStats;
    EnemyStats enemy;
    Transform player;
    SpriteRenderer enemySprite;

    private Vector3 previousPosition;
    private bool runningLeft;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerController>().transform;

        previousPosition = transform.position;

        // Assign the SpriteRenderer component
        enemySprite = GetComponent<SpriteRenderer>();        
        
        // Check if the SpriteRenderer component is missing
        if (enemySprite == null)
        {
            Debug.LogError("SpriteRenderer component not found on the enemy GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Move towards the player at set speed
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMovementSpeed * Time.deltaTime);

        //Flip the sprite if necessary
        CheckDirectionAndFlip();
    }

    void CheckDirectionAndFlip()
    {
        // Calculate the current direction of movement
        Vector3 currentDirection = transform.position - previousPosition;

        // Check if the enemy is moving to the right
        if (currentDirection.x > 0)
        {
            Flip(true);
        }
        // Check if the enemy is moving to the left
        else if (currentDirection.x < 0)
        {
            Flip(false);
        }

        // Update the previous position
        previousPosition = transform.position;
    }

    void Flip(bool flipX)
    {
        enemySprite.flipX = flipX;
        runningLeft = !flipX;
    }
}
