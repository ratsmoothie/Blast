using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent Class for all weapons
public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponStats;
    public GameObject weaponPrefab;
    private float cooldownCurrent;

    protected PlayerController playerController;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cooldownCurrent = weaponStats.cooldownTotal; //stops weapon attacking immediately upon getting it

        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        cooldownCurrent -= Time.deltaTime;
        if(cooldownCurrent <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        cooldownCurrent = weaponStats.cooldownTotal;
    }
}
