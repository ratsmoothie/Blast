using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Components")]
    //public EnemyScriptableObject enemyStats;
    EnemyStats enemy;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Move towards the player at set speed
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMovementSpeed * Time.deltaTime);
    }
}
