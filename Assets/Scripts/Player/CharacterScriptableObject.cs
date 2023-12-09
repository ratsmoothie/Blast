using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Referenced https://www.youtube.com/watch?v=qREiQ5vSAng

//mother of all selectable characters
[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "ScriptableObjects/Characters")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject startingWeapon;

    public GameObject StartingWeapon { get => startingWeapon; private set => startingWeapon = value; }

    [SerializeField]
    float maxHealth;

    public float MaxHealth { get => maxHealth; private set => maxHealth = value; }

    [SerializeField]
    float healthRegen;

    public float HealthRegen { get => healthRegen; private set => healthRegen = value; }

    [SerializeField]
    float movementSpeed;

    public float MovementSpeed { get => movementSpeed; private set => movementSpeed = value; }
    
    [SerializeField]
    float attackPower;

    public float AttackPower { get => attackPower; private set => attackPower = value; }

    [SerializeField]
    float projectileSpeed;

    public float ProjectileSpeed { get => projectileSpeed; private set => projectileSpeed = value; }

    [SerializeField]
    float magnetRange;

    public float MagnetRange { get => magnetRange; private set => magnetRange = value; }

    [SerializeField]
    Sprite charSprite;
    public Sprite CharSprite { get => charSprite; private set => charSprite = value; }

    [SerializeField]
    new string charName;
    public string CharName { get => charName; private set => charName = value; }
    
}
