using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    /*
    //Pass in PlayerController class
    PlayerController playerController;

    private float attackSpeed;
    private float attackRange;
    private float damage;
    private float attackForce;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform pointOfFire;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeAttackStats()
    {
        // Initialize player stats from PlayerController
        attackSpeed = playerController.attackSpeed;
        attackRange = playerController.attackRange;
        damage = playerController.damage;
        attackForce = playerController.attackForce;
    }

    void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, pointOfFire.position, pointOfFire.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(pointOfFire.up * attackForce, ForceMode2D.Impulse);
    }*/
}
