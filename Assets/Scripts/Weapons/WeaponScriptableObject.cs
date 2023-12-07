using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject weaponPrefab;

    public float damage;
    public float speed;
    public float cooldownTotal;
    public int cleaveNum; //amount of enemies our weapon will be able to hit in one attack before disappearing
}
