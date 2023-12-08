using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Referenced https://www.youtube.com/watch?v=dKEjJZ3Sdeo&t=4s for the scriptable object organization

//Stats for all weapons
[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    //components
    [SerializeField]
    GameObject weaponPrefab;
    //WeaponPrefab property for ^
    public GameObject WeaponPrefab { get => weaponPrefab; private set => weaponPrefab = value; }

    [Header("Weapon Stats")]
    [SerializeField]
    float damage;
    public float Damage { get => damage; private set => damage = value; }

    [SerializeField]
    float speed;
    public float Speed { get => speed; private set => speed = value; }

    [SerializeField]
    float cooldownTotal;
    public float CooldownTotal { get => cooldownTotal; private set => cooldownTotal = value; }

    [SerializeField]
    int cleaveNum; //amount of enemies our weapon will be able to hit in one attack before disappearing
    public int CleaveNum { get => cleaveNum; private set => cleaveNum = value; }
}
